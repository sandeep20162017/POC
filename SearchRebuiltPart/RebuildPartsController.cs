using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using BCES.Models.Parts;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace BCES.Controllers.Parts
{
    [Route("api/[controller]")]
    [ApiController]
    public class RebuiltPartsController : ControllerBase
    {
        private readonly IDbConnection _dbConnection;

        public RebuiltPartsController(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        #region Methods

        // GET: api/RebuiltParts
        [HttpGet]
        public async Task<IActionResult> GetRebuiltParts(string rebuildNumber = null, string keyword = null, string busSeries = null, string description = null)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("@RebuildNumber", rebuildNumber);
                parameters.Add("@Keyword", keyword);
                parameters.Add("@BusSeries", busSeries);
                parameters.Add("@Description", description);
                parameters.Add("@ReturnCount", dbType: DbType.Int32, direction: ParameterDirection.Output);

                var result = await _dbConnection.QueryAsync<RebuiltPart>(
                    "[BCES].[SearchRebuiltParts]", parameters, commandType: CommandType.StoredProcedure);

                int returnCount = parameters.Get<int>("@ReturnCount");

                return Ok(new { Data = result, Count = returnCount });
            }
            catch (Exception ex)
            {
                // Log exception as needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/RebuiltParts/Update
        [HttpPost("Update")]
        public IActionResult UpdateRebuiltPart([FromBody] RebuiltPart part)
        {
            try
            {
                if (part == null)
                    return BadRequest("Part data is null");

                // SQL command to update part record
                // Uncomment and replace SQL here when database update is ready.
                /*
                var query = "UPDATE BCES.StockCodedParts SET Quantity = @Quantity, CoreCost = @CoreCost WHERE StockPartID = @StockPartID";
                _dbConnection.Execute(query, new { part.Quantity, part.CoreCost, part.StockPartID });
                */

                return Ok("Update successful");
            }
            catch (Exception ex)
            {
                // Log exception as needed
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        #endregion
    }
}
