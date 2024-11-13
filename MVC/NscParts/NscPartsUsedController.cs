using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using BCES.Models.Parts;

namespace BCES.Controllers
{
    [Route("NscPartsUsed")]
    public class NscPartsUsedController : BaseController
    {
        private readonly ILogger<NscPartsUsedController> _logger;
        private readonly IConfiguration _configuration;

        public NscPartsUsedController(ILogger<NscPartsUsedController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View("~/Views/Parts/NonStockCodedParts/Index.cshtml");
        }

        [HttpGet]
        public async Task<ActionResult> GetNscPartsUsedView([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var model = await GetNSCPartUsedData();
                return Json(model.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching NScPartsUsed data.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult> CreateNscPartsUsed([DataSourceRequest] DataSourceRequest request, [FromBody] NscPartsUsedViewModel nscPartsUsed)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Insert into database using Dapper - direct SQL call
                    // Your logic here

                    return Json(new[] { nscPartsUsed }.ToDataSourceResult(request, ModelState));
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating NScPartsUsed record.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateNscPartsUsed([DataSourceRequest] DataSourceRequest request, [FromBody] NscPartsUsedViewModel nscPartsUsed)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Update the record in the database using Dapper - direct SQL call
                    // Your logic here

                    return Json(new[] { nscPartsUsed }.ToDataSourceResult(request, ModelState));
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating NScPartsUsed record.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult> DestroyNscPartsUsed([DataSourceRequest] DataSourceRequest request, [FromBody] NscPartsUsedViewModel nscPartsUsed)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Delete the record from the database using Dapper - direct SQL call
                    // Your logic here

                    return Json(new[] { nscPartsUsed }.ToDataSourceResult(request, ModelState));
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting NScPartsUsed record.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetSupplierNames()
        {
            try
            {
                var supplierNames = await GetSupplierNamesFromDatabase();
                return Json(supplierNames);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching supplier names.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetSupplierNumbers()
        {
            try
            {
                var supplierNumbers = await GetSupplierNumbersFromDatabase();
                return Json(supplierNumbers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching supplier numbers.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetKeywords()
        {
            try
            {
                var keywords = await GetKeywordsFromDatabase();
                return Json(keywords);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching keywords.");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet]
        public async Task<ActionResult> GetPartDescriptions()
        {
            try
            {
                var partDescriptions = await GetPartDescriptionsFromDatabase();
                return Json(partDescriptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching part descriptions.");
                return StatusCode(500, "Internal server error");
            }
        }

        private async Task<IEnumerable<string>> GetSupplierNamesFromDatabase()
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var supplierNames = await connection.QueryAsync<string>("SELECT Name FROM SupplierNames");
                return supplierNames;
            }
        }

        private async Task<IEnumerable<string>> GetSupplierNumbersFromDatabase()
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var supplierNumbers = await connection.QueryAsync<string>("SELECT Number FROM SupplierNumbers");
                return supplierNumbers;
            }
        }

        private async Task<IEnumerable<string>> GetKeywordsFromDatabase()
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var keywords = await connection.QueryAsync<string>("SELECT Keyword FROM Keywords");
                return keywords;
            }
        }

        private async Task<IEnumerable<string>> GetPartDescriptionsFromDatabase()
        {
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var partDescriptions = await connection.QueryAsync<string>("SELECT Description FROM PartDescriptions");
                return partDescriptions;
            }
        }
    }
}