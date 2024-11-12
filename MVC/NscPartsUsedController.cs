using Dapper;
using Microsoft.AspNetCore.Mvc;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BCES.Models.Parts;

namespace BCES.Parts.Controllers
{
    public class NscPartsUsedController : Controller
    {
        private readonly string _connectionString;

        public NscPartsUsedController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // GET: NscPartsUsed/Index
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // POST: NscPartsUsed/GetNscPartsUsedView
        [HttpPost]
        public async Task<ActionResult> GetNscPartsUsedView([DataSourceRequest] DataSourceRequest request)
        {
            // Fetch data for the grid (you can use Dapper or any other method)
            var model = await GetNscPartsUsedData();
            return Json(model.ToDataSourceResult(request));
        }

        // POST: NscPartsUsed/CreateNscPartsUsed
        [HttpPost]
        public async Task<IActionResult> CreateNscPartsUsed([FromBody] NscPartsUsedViewModel nscPartsUsed)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var insertQuery = @"
                INSERT INTO SCES.NscPartsUsed (OrigSuppNum, OrigSupplierName, Keyword, PartDescription, PerUnitCost)
                VALUES (@OrigSuppNum, @OrigSupplierName, @Keyword, @PartDescription, @PerUnitCost);";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(insertQuery, nscPartsUsed);
                return Ok();
            }
        }

        // PUT: NscPartsUsed/UpdateNscPartsUsed/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNscPartsUsed(int id, [FromBody] NscPartsUsedViewModel nscPartsUsed)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updateQuery = @"
                UPDATE SCES.NscPartsUsed
                SET 
                    OrigSuppNum = @OrigSuppNum,
                    OrigSupplierName = @OrigSupplierName,
                    Keyword = @Keyword,
                    PartDescription = @PartDescription,
                    PerUnitCost = @PerUnitCost
                WHERE 
                    Id = @Id;";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(updateQuery, nscPartsUsed);
                return Ok();
            }
        }

        // DELETE: NscPartsUsed/DeleteNscPartsUsed/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNscPartsUsed(int id)
        {
            var deleteQuery = @"
                DELETE FROM SCES.NscPartsUsed
                WHERE Id = @Id;";

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.ExecuteAsync(deleteQuery, new { Id = id });
                return Ok();
            }
        }

        private async Task<IEnumerable<NscPartsUsedViewModel>> GetNscPartsUsedData()
        {
            var query = @"
                SELECT 
                    nspu.OrigSuppNum,
                    nspu.OrigSupplierName,
                    nspu.Keyword,
                    nspu.PartDescription,
                    nspu.PerUnitCost,
                    nspu.Id
                FROM 
                    SCES.NscPartsUsed nspu;";

            using (var connection = new SqlConnection(_connectionString))
            {
                return await connection.QueryAsync<NscPartsUsedViewModel>(query);
            }
        }
    }
}