using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BCES.Controllers.Base;
using BCES.Data;
using BCES.Helpers;
using BCES.Models.AIHelp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace BCES.Controllers.AIHelp
{
    [Authorize]
    public class AIHelpController : BaseController
    {
        private readonly DapperContext _db;
        private readonly IDbConnection _dbConnection;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AIHelpController> _logger;

        public AIHelpController(
            DapperContext dapper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<AIHelpController> logger)
            : base(dapper, httpContextAccessor)
        {
            _db = dapper;
            _dbConnection = _db.CreateConnection();
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
            
            // Initialize OpenAIHelper with logger
            OpenAIHelper.InitializeLogger(logger);
        }

        // ===== VIEWS =====

        public IActionResult Index()
        {
            return View("~/Views/AIHelp/Index.cshtml");
        }

        public IActionResult AIAssistance()
        {
            _logger.LogInformation("AIAssistance view requested");
            return View("~/Views/AIHelp/AIAssistance.cshtml");
        }

        // ===== API ENDPOINTS =====

        [HttpPost]
        public async Task<IActionResult> AskQuestion([FromBody] AIHelpQuery query)
        {
            try
            {
                _logger.LogInformation("=== AskQuestion START ===");
                _logger.LogInformation($"Query Text: {query?.Text ?? "NULL"}");

                if (query == null)
                {
                    _logger.LogWarning("Query object is null");
                    return BadRequest(new { error = "Invalid request. Query is null." });
                }

                if (string.IsNullOrWhiteSpace(query?.Text))
                {
                    _logger.LogWarning("Query text is empty");
                    return BadRequest(new { error = "Please enter a question." });
                }

                _logger.LogInformation("Calling ProcessHelpQueryAsync...");
                var response = await ProcessHelpQueryAsync(query);
                _logger.LogInformation($"Response generated successfully. Answer length: {response?.Answer?.Length ?? 0}");

                // Log the full response for debugging
                var responseJson = JsonSerializer.Serialize(response, new JsonSerializerOptions { WriteIndented = true });
                _logger.LogInformation($"Full Response: {responseJson}");

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in AskQuestion");
                return StatusCode(500, new { error = $"An error occurred: {ex.Message}", stackTrace = ex.StackTrace });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPageMetadata(string pageName)
        {
            try
            {
                _logger.LogInformation($"GetPageMetadata called for: {pageName}");
                var metadata = await GetPageMetadataFromDbAsync(pageName);
                if (metadata == null)
                {
                    _logger.LogWarning($"Page metadata not found for: {pageName}");
                    return NotFound(new { error = "Page not found" });
                }

                var extracted = GetExtractedMetadata(metadata);

                return Ok(new
                {
                    metadata.PageName,
                    metadata.PageType,
                    metadata.Module,
                    metadata.Url,
                    ExtractedMetadata = extracted
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to retrieve metadata");
                return StatusCode(500, new { error = "Failed to retrieve metadata" });
            }
        }

        // ===== PRIVATE METHODS =====

        private async Task<AIHelpResponse> ProcessHelpQueryAsync(AIHelpQuery query)
        {
            try
            {
                _logger.LogInformation("=== ProcessHelpQueryAsync START ===");

                // 1. Classify intent
                _logger.LogInformation("Step 1: Classifying intent...");
                var intentStr = await OpenAIHelper.ClassifyIntentAsync(query.Text);
                var intent = Enum.TryParse<AIIntentType>(intentStr, out var result) ? result : AIIntentType.GENERAL;
                _logger.LogInformation($"Intent classified as: {intent}");

                // 2. Find relevant page
                _logger.LogInformation("Step 2: Finding relevant page...");
                var pageName = await OpenAIHelper.GetRelevantPageNameAsync(query.Text);
                _logger.LogInformation($"Page name identified: {pageName}");

                if (pageName != "GENERAL" && query.PageContext != null)
                {
                    query.PageContext.PageName = pageName;
                }

                // 3. Get page metadata
                _logger.LogInformation("Step 3: Getting page metadata...");
                var pageMetadata = await GetPageMetadataFromDbAsync(pageName);
                _logger.LogInformation($"Page metadata retrieved: {(pageMetadata != null ? "Yes" : "No")}");

                var context = BuildContextPrompt(query, pageMetadata);
                _logger.LogInformation($"Context built. Length: {context?.Length ?? 0}");

                // 4. Get AI response
                _logger.LogInformation("Step 4: Getting AI response...");
                var systemPrompt = GetSystemPrompt(intent, query, pageMetadata);
                _logger.LogInformation($"System prompt length: {systemPrompt?.Length ?? 0}");

                // Log the prompts for debugging
                _logger.LogInformation($"System Prompt: {systemPrompt}");
                _logger.LogInformation($"User Context: {context}");

                var answer = await OpenAIHelper.GetChatCompletionAsync(systemPrompt, context, maxTokens: 1500);
                _logger.LogInformation($"AI response received. Length: {answer?.Length ?? 0}");
                _logger.LogInformation($"AI response preview: {(answer?.Length > 200 ? answer.Substring(0, 200) + "..." : answer ?? "NULL")}");

                // 5. Extract workflows if applicable
                _logger.LogInformation("Step 5: Extracting workflows...");
                var workflowSteps = new List<AIWorkflowStep>();
                if (IsWorkflowQuery(query.Text))
                {
                    _logger.LogInformation("Query identified as workflow type");
                    var metadata = pageMetadata != null ? GetExtractedMetadata(pageMetadata) : null;
                    if (metadata?.Workflows != null)
                    {
                        _logger.LogInformation($"Found {metadata.Workflows.Count} workflows");
                        var workflow = GetRelevantWorkflow(query.Text, metadata.Workflows);
                        if (workflow != null)
                        {
                            _logger.LogInformation($"Relevant workflow found: {workflow.Name}");
                            workflowSteps = workflow.Steps;
                        }
                    }
                }

                // 6. Get related actions
                _logger.LogInformation("Step 6: Getting related actions...");
                var relatedActions = GetRelatedActions(intent, pageMetadata);
                _logger.LogInformation($"Related actions found: {relatedActions?.Count ?? 0}");

                var response = new AIHelpResponse
                {
                    Answer = answer ?? "I couldn't generate a response. Please try again.",
                    WorkflowSteps = workflowSteps,
                    RelatedActions = relatedActions
                };

                _logger.LogInformation("=== ProcessHelpQueryAsync COMPLETE ===");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in ProcessHelpQueryAsync");
                return new AIHelpResponse
                {
                    Answer = $"I encountered an error processing your request: {ex.Message}"
                };
            }
        }

        private async Task<AIHelpPageMetadata> GetPageMetadataFromDbAsync(string pageName)
        {
            if (pageName == "GENERAL" || string.IsNullOrEmpty(pageName))
            {
                return null;
            }

            try
            {
                var sql = @"
                    SELECT TOP 1 
                        Id, PageName, PageType, Module, Controller, Action, Url,
                        JsonMetadata, IsActive, Version, CreatedDate, LastUpdated,
                        CreatedBy, UpdatedBy
                    FROM SBCES.HelpPageMetadata
                    WHERE PageName = @PageName AND IsActive = 1
                    ORDER BY Version DESC";

                return await _dbConnection.QueryFirstOrDefaultAsync<AIHelpPageMetadata>(
                    sql, new { PageName = pageName });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting page metadata for {pageName}");
                return null;
            }
        }

        private AIExtractedPageMetadata GetExtractedMetadata(AIHelpPageMetadata page)
        {
            if (page == null || string.IsNullOrEmpty(page.JsonMetadata))
                return null;

            try
            {
                return JsonSerializer.Deserialize<AIExtractedPageMetadata>(page.JsonMetadata);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deserializing metadata");
                return null;
            }
        }

        private string BuildContextPrompt(AIHelpQuery query, AIHelpPageMetadata pageMetadata)
        {
            var metadata = pageMetadata != null ? GetExtractedMetadata(pageMetadata) : null;

            if (metadata == null)
            {
                return $"User Query: {query.Text}\nNo specific page metadata found.";
            }

            var sb = new StringBuilder();
            sb.AppendLine($"User Query: {query.Text}");
            sb.AppendLine($"Current Page: {metadata.PageInfo?.PageName ?? "Unknown"}");
            sb.AppendLine($"Module: {metadata.PageInfo?.Module ?? "Unknown"}");
            sb.AppendLine($"Page Type: {metadata.PageInfo?.PageType ?? "Unknown"}");
            sb.AppendLine($"Description: {metadata.PageInfo?.Description ?? "No description"}");
            sb.AppendLine();

            if (metadata.UiStructure != null)
            {
                sb.AppendLine("=== UI Structure ===");
                sb.AppendLine($"Has Grid: {metadata.UiStructure.HasGrid}");
                if (metadata.UiStructure.HasGrid)
                {
                    sb.AppendLine($"Grid Name: {metadata.UiStructure.GridName}");
                    sb.AppendLine("Grid Columns:");
                    foreach (var col in metadata.UiStructure.GridColumns ?? new List<AIGridColumn>())
                    {
                        var required = col.IsRequired ? " (Required)" : "";
                        sb.AppendLine($"  - {col.Title} ({col.DataType}){required}");
                    }
                }

                if (metadata.UiStructure.Popups != null && metadata.UiStructure.Popups.Count > 0)
                {
                    sb.AppendLine("Popups:");
                    foreach (var popup in metadata.UiStructure.Popups)
                    {
                        sb.AppendLine($"  - {popup.Title}");
                        if (popup.Sections != null)
                        {
                            foreach (var section in popup.Sections)
                            {
                                sb.AppendLine($"    {section.Name}:");
                                foreach (var field in section.Fields ?? new List<AIPopupField>())
                                {
                                    var required = field.IsRequired ? " *" : "";
                                    var readOnly = field.IsReadOnly ? " (Read-Only)" : "";
                                    sb.AppendLine($"      - {field.Label} ({field.Type}){required}{readOnly}");
                                }
                            }
                        }
                    }
                }

                if (metadata.UiStructure.Tabs != null && metadata.UiStructure.Tabs.Count > 0)
                {
                    sb.AppendLine("Tabs:");
                    foreach (var tab in metadata.UiStructure.Tabs)
                    {
                        var active = tab.IsActive ? " (Active)" : "";
                        sb.AppendLine($"  - {tab.Title}{active}");
                    }
                }
            }

            if (metadata.Fields != null)
            {
                sb.AppendLine();
                sb.AppendLine("=== Field Information ===");
                if (metadata.Fields.Required != null && metadata.Fields.Required.Count > 0)
                    sb.AppendLine($"Required Fields: {string.Join(", ", metadata.Fields.Required)}");
                if (metadata.Fields.ReadOnly != null && metadata.Fields.ReadOnly.Count > 0)
                    sb.AppendLine($"Read-Only Fields: {string.Join(", ", metadata.Fields.ReadOnly)}");
                if (metadata.Fields.AutoGenerated != null && metadata.Fields.AutoGenerated.Count > 0)
                    sb.AppendLine($"Auto-Generated Fields: {string.Join(", ", metadata.Fields.AutoGenerated)}");
            }

            if (metadata.Workflows != null && metadata.Workflows.Count > 0)
            {
                sb.AppendLine();
                sb.AppendLine("=== Available Workflows ===");
                foreach (var workflow in metadata.Workflows)
                {
                    sb.AppendLine($"  - {workflow.Name}: {workflow.Description}");
                }
            }

            if (metadata.Actions != null && metadata.Actions.Count > 0)
            {
                sb.AppendLine();
                sb.AppendLine("=== API Endpoints ===");
                foreach (var action in metadata.Actions)
                {
                    sb.AppendLine($"  - {action.Key}: {action.Value}");
                }
            }

            return sb.ToString();
        }

        private string GetSystemPrompt(AIIntentType intent, AIHelpQuery query, AIHelpPageMetadata pageMetadata)
        {
            var basePrompt = @"
You are a helpful AI assistant for the BCES Cost Estimation System.
Your goal is to help users understand and use the system effectively.

Guidelines:
1. Be specific and provide step-by-step instructions
2. Mention field names, button names, and locations
3. Highlight required fields and important information
4. Provide tips and best practices
5. If you don't know something, say so and suggest where to find help
6. Format responses with clear sections and bullet points

Format your response using:
- ### Headings for sections
- **Bold** for important items
- `Code` for field/button names
- - Bullet points for lists
- Numbered steps for procedures
- > Note: for important warnings or tips

";

            var agentSpecificPrompt = intent switch
            {
                AIIntentType.STOCK_PART => @"
You are a Stock Coded Parts expert.

Key information about Stock Coded Parts:
- MMS Stock Code: Unique identifier (required)
- Description: Part description
- Unit Cost: Cost per unit
- Core Cost: Refundable core charge
- Supplier Information: Orig Supplier Number and Name
- Total Cost calculation: TotalCost = UnitCost × Qty × Percentage/100

When helping:
- Explain the Add, Edit, Delete operations
- Mention the ComboBox auto-fill behavior
- Warn that deleting affects existing estimates
",
                AIIntentType.NON_STOCK_PART => @"
You are a Non-Stock Coded Parts expert.

Key information about Non-Stock Coded Parts:
- Orig Supplier Number: Supplier's part number (required)
- Orig Supplier Name: Supplier name (required)
- Not kept in regular inventory
- Ordered from suppliers as needed
- Refresh Details functionality updates from supplier master

When helping:
- Explain the supplier relationship
- Describe the Refresh Details functionality
- Note that Supplier Number is read-only in edit mode
",
                AIIntentType.REBUILT_PART => @"
You are a Rebuilt Parts expert.

Key information about Rebuilt Parts:
- Rebuilt Stock Num: Unique identifier (required, read-only in edit mode)
- MMS Stock Code: Stock code mapping (required)
- Keyword: Descriptive keyword (required)
- Buy New Cost: Cost to buy new (required)
- Reman Cost: Remanufacturing cost (required)
- Convert from Make vs Buy: Creates a rebuilt part from an MB estimate

When helping:
- Explain the Add, Edit, Delete operations
- Describe the Convert from Make vs Buy feature
- Mention cost implications
",
                AIIntentType.ESTIMATE => @"
You are an Estimate expert.

Types of estimates:
1. Vehicle Estimate - For vehicle repairs/maintenance
2. Make vs Buy Estimate - For make vs buy decisions

Key features:
- Main form with required fields
- Tabs for Labour, Stock Parts, Non-Stock Parts, Rebuilt Parts, Final Estimate
- Total Cost calculations
- Copy, Archive, Delete functionality

When helping:
- Explain the workflow from creation to final estimate
- Describe each tab's purpose
- Mention required fields
- Explain the Total Cost calculation
",
                AIIntentType.ADMIN => @"
You are an Administration expert.

Admin functions include:
- User Management (add, edit, delete users, assign roles)
- Labour Task Management (task descriptions)
- Differential Management (differential types)
- Engine Management (engine types)
- Make Model Year Management
- Transmission Management
- Employee Salary Management
- Labour Type Management
- List Of Vehicles
- Change Settings
- Announcement Settings

When helping:
- Explain which admin page to use
- Describe the CRUD operations
- Mention required fields
- Warn about implications of changes
",
                AIIntentType.WORKFLOW => @"
You are a Workflow expert.

Focus on providing:
1. Step-by-step instructions
2. Clear numbering
3. What to expect at each step
4. Helpful tips
5. Troubleshooting common issues

Always break down complex tasks into simple, actionable steps.
",
                _ => @"
You are a general assistant for the BCES Cost Estimation System.

Help users with any question about:
- System features and functionality
- Navigation
- Common tasks
- Troubleshooting

If you're unsure, suggest consulting the User Guide or Training Guide.
"
            };

            return basePrompt + "\n" + agentSpecificPrompt;
        }

        private AIWorkflow GetRelevantWorkflow(string query, List<AIWorkflow> workflows)
        {
            if (workflows == null || workflows.Count == 0)
                return null;

            foreach (var workflow in workflows)
            {
                if (query.Contains(workflow.Name, StringComparison.OrdinalIgnoreCase) ||
                    query.Contains(workflow.Name.Replace("Create ", ""), StringComparison.OrdinalIgnoreCase) ||
                    (query.Contains("add", StringComparison.OrdinalIgnoreCase) &&
                     workflow.Name.Contains("Create", StringComparison.OrdinalIgnoreCase)))
                {
                    return workflow;
                }
            }

            return null;
        }

        private bool IsWorkflowQuery(string query)
        {
            return query.Contains("how to", StringComparison.OrdinalIgnoreCase) ||
                   query.Contains("steps", StringComparison.OrdinalIgnoreCase) ||
                   query.Contains("guide", StringComparison.OrdinalIgnoreCase) ||
                   query.Contains("tutorial", StringComparison.OrdinalIgnoreCase);
        }

        private List<AIRelatedAction> GetRelatedActions(AIIntentType intent, AIHelpPageMetadata pageMetadata)
        {
            var actions = new List<AIRelatedAction>();

            if (pageMetadata != null)
            {
                var metadata = GetExtractedMetadata(pageMetadata);
                if (metadata?.Actions != null)
                {
                    foreach (var action in metadata.Actions)
                    {
                        actions.Add(new AIRelatedAction
                        {
                            Id = action.Key,
                            Label = action.Key.ToUpper(),
                            Type = "navigate",
                            Target = action.Value
                        });
                    }
                }
            }

            // Add common actions based on intent
            switch (intent)
            {
                case AIIntentType.STOCK_PART:
                    actions.Add(new AIRelatedAction { Id = "stock-add", Label = "Add Stock Part", Type = "navigate", Target = "/StockCodedParts/Index" });
                    break;
                case AIIntentType.NON_STOCK_PART:
                    actions.Add(new AIRelatedAction { Id = "nsc-add", Label = "Add Non-Stock Part", Type = "navigate", Target = "/NscPartsUsedIndex" });
                    break;
                case AIIntentType.REBUILT_PART:
                    actions.Add(new AIRelatedAction { Id = "rb-add", Label = "Add Rebuilt Part", Type = "navigate", Target = "/RebuiltPartsIndex" });
                    break;
                case AIIntentType.ESTIMATE:
                    actions.Add(new AIRelatedAction { Id = "estimate-add", Label = "Add Vehicle Estimate", Type = "navigate", Target = "/VehiclesIndex" });
                    actions.Add(new AIRelatedAction { Id = "estimate-mb-add", Label = "Add Make vs Buy", Type = "navigate", Target = "/MakeVsBuyIndex" });
                    break;
                case AIIntentType.ADMIN:
                    actions.Add(new AIRelatedAction { Id = "admin-users", Label = "User Management", Type = "navigate", Target = "/UserManagementGrid" });
                    break;
            }

            return actions;
        }
    }
}
