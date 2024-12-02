using BCES.Models.Parts;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BCES.Controllers.Parts
{
    public class RebuiltPartsController : BaseController
    {
        private readonly DapperContext _db;
        private readonly IDBConnection _dbConnection;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RebuiltPartsController(DapperContext dapper, IHttpContextAccessor httpContextAccessor) : base(dapper, httpContextAccessor)
        {
            _db = dapper;
            _dbConnection = _db.CreateConnection();
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("RebuiltPartsIndex")]
        public IActionResult RebuiltPartsIndex()
        {
            return View("/Views/Parts/RebuiltParts/Index.cshtml");
        }

        [HttpGet]
        public async Task<ActionResult> GetRebuiltPartsView([DataSourceRequest] DataSourceRequest request)
        {
            var model = await GetRebuiltPartsData();
            return Json(model.ToDataSourceResult(request));
        }

        private async Task<IEnumerable<RebuiltPartsViewModel>> GetRebuiltPartsData()
        {
            var data = new List<RebuiltPartsViewModel>
            {
                new RebuiltPartsViewModel { RebuiltNumber = "RN001", MMSStockCode = "MMS001", Keyword = "KEY001", Description = "Description 1" },
                new RebuiltPartsViewModel { RebuiltNumber = "RN002", MMSStockCode = "MMS002", Keyword = "KEY002", Description = "Description 2" }
            };
            return data;
        }

        [HttpGet]
        public async Task<IActionResult> GetRebuiltPartEstimates(string rebuiltNumber)
        {
            try
            {
                var model = await GetRebuiltPartEstimatesData(rebuiltNumber);
                return PartialView("/Views/Parts/RebuiltParts/_RebuiltPartEstimates.cshtml", model);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<RebuiltPartEstimates> GetRebuiltPartEstimatesData(string rebuiltNumber)
        {
            var model = new RebuiltPartEstimates
            {
                JobNumber = "JN001",
                RebuiltNumber = rebuiltNumber,
                MMSBuyCode = "MMSB001",
                SOPNumber = "SOP001",
                Keyword = "KEY001",
                CoreCode = "CORE001",
                Description = "Sample Description",
                VehicleSeries = "Series A",
                LastModified = "2023-10-01",
                LabourGrid = new List<LabourGridModel>
                {
                    new LabourGridModel { Id = 1, LabourCode = "L001", Description = "Labour 1", Hours = 2, Rate = 50, TotalCost = 100 },
                    new LabourGridModel { Id = 2, LabourCode = "L002", Description = "Labour 2", Hours = 3, Rate = 50, TotalCost = 150 }
                },
                LabourHourSummary = new List<SummaryGridModel>
                {
                    new SummaryGridModel { Category = "Category 1", Amount = 100 },
                    new SummaryGridModel { Category = "Category 2", Amount = 200 }
                },
                MaterialCostSummary = new List<SummaryGridModel>
                {
                    new SummaryGridModel { Category = "Material 1", Amount = 150 },
                    new SummaryGridModel { Category = "Material 2", Amount = 250 }
                },
                CoreCost = 50,
                MaterialsCost = 400,
                LabourCost = 250,
                LabourOverheadCost = 50,
                LabourTotal = 300,
                TotalRebuiltPartCost = 700,
                TaxTotal = 70,
                TotalRebuiltPartWithCost = 770,
                AdditionalInfo1 = "Info 1",
                AdditionalInfo2 = "Info 2"
            };
            return model;
        }

        [HttpPost]
        public IActionResult ArchiveEstimate()
        {
            return Json(new { success = true, message = "Archived" });
        }

        [HttpPost]
        public IActionResult SaveEstimate()
        {
            return Json(new { success = true, message = "Saved" });
        }

        [HttpPost]
        public IActionResult UpdateEstimate()
        {
            return Json(new { success = true, message = "Updated" });
        }
    }
}