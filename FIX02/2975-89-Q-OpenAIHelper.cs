using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace BCES.Helpers
{
    public static class OpenAIHelper
    {
        // ⚠️ HARDCODE YOUR OPENAI API KEY HERE FOR POC ⚠️
        private const string API_KEY = "YOUR_OPENAI_API_KEY_HERE";
        private const string API_URL = "https://api.openai.com/v1/chat/completions";
        private const string DEFAULT_MODEL = "gpt-4o";

        private static readonly HttpClient _httpClient = new HttpClient();
        private static ILogger _logger;

        static OpenAIHelper()
        {
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {API_KEY}");
        }

        public static void InitializeLogger(ILogger logger)
        {
            _logger = logger;
        }

        public static async Task<string> GetChatCompletionAsync(
            string systemPrompt,
            string userPrompt,
            int maxTokens = 1000,
            float temperature = 0.7f)
        {
            try
            {
                _logger?.LogInformation("=== OpenAI GetChatCompletionAsync START ===");
                _logger?.LogInformation($"API Key (first 10 chars): {API_KEY.Substring(0, Math.Min(10, API_KEY.Length))}...");
                _logger?.LogInformation($"Model: {DEFAULT_MODEL}");
                _logger?.LogInformation($"System Prompt Length: {systemPrompt?.Length ?? 0}");
                _logger?.LogInformation($"User Prompt Length: {userPrompt?.Length ?? 0}");

                var request = new
                {
                    model = DEFAULT_MODEL,
                    messages = new[]
                    {
                        new { role = "system", content = systemPrompt },
                        new { role = "user", content = userPrompt }
                    },
                    max_tokens = maxTokens,
                    temperature = temperature
                };

                var jsonRequest = JsonSerializer.Serialize(request);
                _logger?.LogInformation($"Request JSON (truncated): {jsonRequest.Substring(0, Math.Min(jsonRequest.Length, 1000))}...");

                var content = new StringContent(
                    jsonRequest,
                    Encoding.UTF8,
                    "application/json");

                _logger?.LogInformation("Sending request to OpenAI API...");
                var response = await _httpClient.PostAsync(API_URL, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger?.LogInformation($"Response Status Code: {(int)response.StatusCode} - {response.StatusCode}");
                _logger?.LogInformation($"Response Content Length: {responseContent?.Length ?? 0}");
                
                // Log the FULL raw response for debugging
                _logger?.LogInformation($"=== RAW OPENAI RESPONSE (FULL) ===");
                _logger?.LogInformation(responseContent);
                _logger?.LogInformation($"=== END RAW OPENAI RESPONSE ===");

                if (!response.IsSuccessStatusCode)
                {
                    _logger?.LogError($"OpenAI API Error: {response.StatusCode} - {responseContent}");
                    
                    // Try to parse error from OpenAI
                    try
                    {
                        using var errorDoc = JsonDocument.Parse(responseContent);
                        if (errorDoc.RootElement.TryGetProperty("error", out var errorElement))
                        {
                            var errorMsg = errorElement.TryGetProperty("message", out var msgProp) 
                                ? msgProp.GetString() 
                                : "Unknown error";
                            _logger?.LogError($"OpenAI Error Message: {errorMsg}");
                            return $"OpenAI API Error: {errorMsg}";
                        }
                    }
                    catch
                    {
                        // Ignore parsing errors
                    }
                    
                    return $"I'm having trouble connecting to the AI service. Status: {response.StatusCode}";
                }

                // Parse and log the response structure
                try
                {
                    using var doc = JsonDocument.Parse(responseContent);
                    _logger?.LogInformation("Response JSON parsed successfully");
                    
                    // Check if choices exist
                    if (doc.RootElement.TryGetProperty("choices", out var choicesElement))
                    {
                        _logger?.LogInformation($"Choices count: {choicesElement.GetArrayLength()}");
                        
                        if (choicesElement.GetArrayLength() > 0)
                        {
                            var firstChoice = choicesElement[0];
                            if (firstChoice.TryGetProperty("message", out var messageElement))
                            {
                                if (messageElement.TryGetProperty("content", out var contentElement))
                                {
                                    var result = contentElement.GetString();
                                    _logger?.LogInformation($"Content extracted successfully. Length: {result?.Length ?? 0}");
                                    _logger?.LogInformation($"Content preview: {(result?.Length > 200 ? result.Substring(0, 200) + "..." : result ?? "NULL")}");
                                    return result ?? "I couldn't generate a response. Please try again.";
                                }
                                else
                                {
                                    _logger?.LogError("Response has 'message' but no 'content' property");
                                    _logger?.LogInformation($"Message properties: {string.Join(", ", messageElement.EnumerateObject().Select(p => p.Name))}");
                                }
                            }
                            else
                            {
                                _logger?.LogError("Response has 'choices' but no 'message' property");
                                _logger?.LogInformation($"Choice properties: {string.Join(", ", firstChoice.EnumerateObject().Select(p => p.Name))}");
                            }
                        }
                        else
                        {
                            _logger?.LogError("Response has empty 'choices' array");
                        }
                    }
                    else
                    {
                        _logger?.LogError("Response has no 'choices' property");
                        _logger?.LogInformation($"Response root properties: {string.Join(", ", doc.RootElement.EnumerateObject().Select(p => p.Name))}");
                    }
                }
                catch (Exception parseEx)
                {
                    _logger?.LogError(parseEx, "Error parsing OpenAI response");
                }

                return "I couldn't parse the response from OpenAI. Please check the logs.";
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error calling OpenAI API");
                return $"An error occurred while processing your request: {ex.Message}";
            }
        }

        public static async Task<string> ClassifyIntentAsync(string query)
        {
            _logger?.LogInformation($"ClassifyIntentAsync: Query = {query}");
            
            try
            {
                var systemPrompt = @"
You are a query classifier for a Cost Estimation System. 
Classify the user's question into one of these categories:
- STOCK_PART: Questions about stock coded parts (add, edit, delete, search)
- NON_STOCK_PART: Questions about non-stock coded parts
- REBUILT_PART: Questions about rebuilt parts
- ESTIMATE: Questions about estimates (Vehicle, Make vs Buy)
- ADMIN: Questions about admin functions (users, settings, etc.)
- WORKFLOW: How-to questions, step-by-step guides
- GENERAL: Other questions

Return ONLY the category name.";

                var result = await GetChatCompletionAsync(systemPrompt, query, maxTokens: 20, temperature: 0.1f);
                _logger?.LogInformation($"ClassifyIntentAsync: Result = {result}");
                return result?.Trim() ?? "GENERAL";
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in ClassifyIntentAsync");
                return "GENERAL";
            }
        }

        public static async Task<string> GetRelevantPageNameAsync(string query)
        {
            _logger?.LogInformation($"GetRelevantPageNameAsync: Query = {query}");
            
            try
            {
                var systemPrompt = @"
You are a page classifier for a Cost Estimation System.
Based on the user's question, identify which page they are asking about.
Return ONLY the page name from this list:
- MakeVsBuyIndex
- VehiclesIndex
- StockCodedPartsIndex
- NscPartsUsedIndex
- RebuiltPartsIndex
- UserManagementGrid
- Task
- DifferentialIndex
- EngineIndex
- MakeModelYearIndex
- TransmissionIndex
- EmpSalaryIndex
- LabourTypeIndex
- Bus
- SettingIndex
- Content
- ArchivedIndex

If the question is not about a specific page, return 'GENERAL'.";

                var result = await GetChatCompletionAsync(systemPrompt, query, maxTokens: 30, temperature: 0.1f);
                _logger?.LogInformation($"GetRelevantPageNameAsync: Result = {result}");
                return result?.Trim() ?? "GENERAL";
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in GetRelevantPageNameAsync");
                return "GENERAL";
            }
        }
    }
}
