using Dapper;
using Microsoft.AspNetCore.Mvc;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;
using BCES.Models.Admin;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using DataSourceRequest = Kendo.Mvc.UI.DataSourceRequest;
using BCES.Data;
using Telerik.SvgIcons;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using BCES.Controllers.Base;
using BCES.Models.Parts;
using BCES.Models.Common;

using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;
using Azure.Core;
namespace BCES.Controllers
{
    public class RebuiltPartsController : BaseController
    {
        private readonly DapperContext _db;
        private readonly IDbConnection _dbConnection;
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
            // return View();
            return View("~/Views/Parts/RebuiltParts/Index.cshtml");
        }

        // POST: NscPartsUsed/GetNscPartsUsedView
        [HttpGet]
        public async Task<ActionResult> GetRebuiltPartsView([DataSourceRequest] DataSourceRequest request)
        {
            // Fetch data for the grid (you can use Dapper or any other method)
            var model = await GetRebuiltPartsData();
            return Json(model.ToDataSourceResult(request));
        }

        private async Task<IEnumerable<RebuiltPartsViewModel>> GetRebuiltPartsData()
        {
            try
            {
                var query = @"   
                           SELECT rbm.RbMasterlistId
                           ,rbm.[RbMasterlistId]
                          ,rbm.[RebuiltStockNum]
                          ,rbm.[MmsStockCode]
                          ,rbm.[Keyword]
                          ,rbm.JobNumber
                          ,rbm.[CorePartNum]
                          ,rbm.[DetailedDesc]
                          ,rbm.[CoreCharge]
                          ,rbm.[EstimatedCost]
                          ,rbm.[BuyNewCost]
                          ,rbm.[RemanCost]
                          ,rbm.[ExternalCost]
                         ,lb.Description AS VehicleSeries
                           from [SBCES].[RbMasterlist] rbm
                          INNER JOIN [SBCES].[RbListOfBuses] rlb ON rlb.[RebuiltStockNum] = rbm.[RebuiltStockNum]
                          INNER JOIN [SBCES].[ListOfBuses] lb ON rlb.ListId = lb.ListId
                             ";
                var rbParts = _dbConnection.Query<RebuiltPartsViewModel>(query).ToList();

                foreach (var rbPart in rbParts)
                {
                    rbPart.VehicleSeries = null;
                }

                return rbParts.Distinct().ToList();
            
            

            // return await _dbConnection.QueryAsync<RebuiltPartsViewModel>(query);
        }
            catch (Exception ex)
            {
                return null;
            }

        }

        [HttpGet]
        public async Task<IActionResult> ReadSites([DataSourceRequest] DataSourceRequest request)
        {
            var sites = await GetSites();
            return Json(sites);
        }

        private async Task<IEnumerable<ListOfBusesModel>> GetSites()
        {
            var query = "SELECT ListId, Description FROM SBCES.ListOfBuses";
            var vehicles = await _dbConnection.QueryAsync<ListOfBusesModel>(query);
            return vehicles.ToList();

        }

        [HttpPost]
        public async Task<IActionResult> GetSelectedVehicles([FromBody] dynamic data)
        {
            var sites = data?.sites?.ToObject<List<string>>() ?? Enumerable.Empty<string>();
           // _selectedSites = sites;
            var viewModel = new UserViewModel { SelectedSites = sites };
            return Json(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetRebuiltPartEstimates(string rebuiltNumber)
        {
            try
            {
                var model = await GetRebuiltPartEstimatesData(rebuiltNumber);
                return View("~/Views/Parts/RebuiltParts/_RebuiltPartEstimates.cshtml");
               // return PartialView("~/Views/Parts/RebuiltParts/_RebuiltPartEstimates.cshtml", model);
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
                RebuiltStockNum = rebuiltNumber,
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

        [HttpGet]
        public async Task<IActionResult> GetLabour([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                //var model = await GetRebuiltPartsData();
               // return Json(model.ToDataSourceResult(request));
                var model = await GetLabourData(1);
                return Json(model);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private async Task<LabourGridModel> GetLabourData(int id)
        {
            var model = new LabourGridModel
            {
               Id =1,
               LabourCode = "LC 001",
               Description = "Labor Description",
               Hours = 3,
               Rate= 25,
               TotalCost = 30
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

        [HttpPost]
        public IActionResult ActionHandler(string action)
        {
            return Json(new { success = true, message = "{action} button clicked" });
        }
        //public IActionResult Index()
        //{
        //    var mockData = new List<RebuiltPartsViewModel>
        //    {
        //        new RebuiltPartsViewModel
        //        {
        //            StockCode = "SC001",
        //            Description = "Rebuilt Engine",
        //            Keyword = "Engine",
        //            JobNumber = "JN123",
        //            MMSBuyCode = "MMS001",
        //            CoreCost = 250.00M,
        //            SOPNumber = "SOP001",
        //            BuyCost = 300.00M,
        //            RemanufacturedCost = 400.00M,
        //            IsActive = true,
        //            Vehicles = new List<string> { "Toyota", "Ford" }
        //        }
        //    };

        //    // return View(mockData);
        //    return View("~/Views/Parts/RebuiltParts/Index.cshtml", mockData);
        //}

        //[HttpPost]
        //public IActionResult Create(RebuiltPartsViewModel model)
        //{
        //    //using (var connection = new SqlConnection(_connectionString))
        //    //{
        //    //    var sql = @"INSERT INTO RebuiltParts (StockCode, Description, Keyword, JobNumber, MMSBuyCode, CoreCost, SOPNumber, BuyCost, RemanufacturedCost, IsActive) 
        //    //                VALUES (@StockCode, @Description, @Keyword, @JobNumber, @MMSBuyCode, @CoreCost, @SOPNumber, @BuyCost, @RemanufacturedCost, @IsActive)";
        //    //    connection.Execute(sql, model);
        //    //}
        //    return RedirectToAction("Index");
        //}

        //[HttpPost]
        //public IActionResult Update(RebuiltPartsViewModel model)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        //var sql = @"UPDATE RebuiltParts 
        //        //            SET Description = @Description, Keyword = @Keyword, JobNumber = @JobNumber, MMSBuyCode = @MMSBuyCode,
        //        //                CoreCost = @CoreCost, SOPNumber = @SOPNumber, BuyCost = @BuyCost, RemanufacturedCost = @RemanufacturedCost, IsActive = @IsActive
        //        //            WHERE StockCode = @StockCode";
        //       //connection.Execute(sql, model);
        //    }
        //    return RedirectToAction("Index");
        //}

        //[HttpPost]
        //public IActionResult Delete(string stockCode)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        //var sql = "DELETE FROM RebuiltParts WHERE StockCode = @StockCode";
        //        //connection.Execute(sql, new { StockCode = stockCode });
        //    }
        //    return RedirectToAction("Index");
        //}
    }
}
