using BCES.Models.Parts;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;

namespace BCES.Controllers.Parts
{
    public class PartsController : Controller
    {
        private readonly IDbConnection _dbConnection;

        public PartsController(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        // GET: Parts
        public IActionResult Index()
        {
            return View();
        }

        #region Grid Operations

        /// <summary>
        /// Reads the data for the Kendo Grid using the stored procedure [BCES].[SearchStockCodedParts].
        /// </summary>
        /// <param name="request">The DataSourceRequest object.</param>
        /// <returns>A JSON result containing the grid data.</returns>
        public async Task<IActionResult> ReadParts([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var parts = await _dbConnection.QueryAsync<RebuildPartsModel>("[BCES].[SearchStockCodedParts]", commandType: CommandType.StoredProcedure);
                return Json(parts.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching parts.");
            }
        }

        /// <summary>
        /// Adds a new RebuildPartsModel using a stored procedure (commented out for now).
        /// </summary>
        /// <param name="request">The DataSourceRequest object.</param>
        /// <param name="part">The RebuildPartsModel object to be added.</param>
        /// <returns>A JSON result containing the added part.</returns>
        [HttpPost]
        public async Task<IActionResult> AddPart([DataSourceRequest] DataSourceRequest request, [FromForm] RebuildPartsModel part)
        {
            try
            {
                // TODO: Uncomment and implement the stored procedure call to add a new part.
                // var partId = await _dbConnection.ExecuteScalarAsync<int>("[BCES].[AddRebuiltPart]", part, commandType: CommandType.StoredProcedure);
                // part.StockPartID = partId;

                return Json(new[] { part }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding the part.");
            }
        }

        #endregion
    }
}