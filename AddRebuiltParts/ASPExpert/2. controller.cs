using Microsoft.AspNetCore.Mvc;
using BCES.Models.RebuiltParts;
using Dapper;
using System.Data.SqlClient;

namespace BCES.Controllers
{
    public class RebuiltPartsController : Controller
    {
        private readonly string _connectionString = "YourConnectionStringHere";

        public IActionResult Index()
        {
            var mockData = new List<RebuiltPartViewModel>
            {
                new RebuiltPartViewModel
                {
                    StockCode = "SC001",
                    Description = "Rebuilt Engine",
                    Keyword = "Engine",
                    JobNumber = "JN123",
                    MMSBuyCode = "MMS001",
                    CoreCost = 250.00M,
                    SOPNumber = "SOP001",
                    BuyCost = 300.00M,
                    RemanufacturedCost = 400.00M,
                    IsActive = true,
                    Vehicles = new List<string> { "Toyota", "Ford" }
                }
            };

            return View(mockData);
        }

        [HttpPost]
        public IActionResult Create(RebuiltPartViewModel model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"INSERT INTO RebuiltParts (StockCode, Description, Keyword, JobNumber, MMSBuyCode, CoreCost, SOPNumber, BuyCost, RemanufacturedCost, IsActive) 
                            VALUES (@StockCode, @Description, @Keyword, @JobNumber, @MMSBuyCode, @CoreCost, @SOPNumber, @BuyCost, @RemanufacturedCost, @IsActive)";
                connection.Execute(sql, model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Update(RebuiltPartViewModel model)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = @"UPDATE RebuiltParts 
                            SET Description = @Description, Keyword = @Keyword, JobNumber = @JobNumber, MMSBuyCode = @MMSBuyCode,
                                CoreCost = @CoreCost, SOPNumber = @SOPNumber, BuyCost = @BuyCost, RemanufacturedCost = @RemanufacturedCost, IsActive = @IsActive
                            WHERE StockCode = @StockCode";
                connection.Execute(sql, model);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(string stockCode)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "DELETE FROM RebuiltParts WHERE StockCode = @StockCode";
                connection.Execute(sql, new { StockCode = stockCode });
            }
            return RedirectToAction("Index");
        }
    }
}
