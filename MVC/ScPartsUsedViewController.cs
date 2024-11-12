using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using BCES.Models.Parts;

namespace BCES.Parts.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ScPartsUsedController : ControllerBase
    {
        private readonly string _connectionString;

        public ScPartsUsedController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet("view")]
        public async Task<ActionResult<IEnumerable<ScPartsUsedView>>> GetScPartsUsedView()
        {
            var query = @"
                SELECT 
                    spu.MmsStockCode,
                    spu.DateEntered,
                    spu.RebPartCost,
                    spu.UserEntered,
                    spu.MmsCost,
                    spu.OemCost,
                    spu.LastModifiedBy,
                    spu.LastModifiedDate,
                    spu.MmsRebuiltCode,
                    spu.CostCentre,
                    spu.QtyReqd,
                    spu.PercentUsage,
                    spu.LinkCode,
                    spu.RebuiltPart,
                    spu.LinkType,
                    spu.CoreCost,
                    spu.OrigSuppNum,
                    spu.OrigSupplierName,
                    cc.CostCentreName -- Example field from CostCenters table
                FROM 
                    SCES.ScPartsUsed spu
                LEFT JOIN 
                    SCES.CostCenters cc ON spu.CostCentre = cc.CostCentre;";

            using (var connection = new SqlConnection(_connectionString))
            {
                var results = await connection.QueryAsync<ScPartsUsedView>(query);
                return Ok(results.ToList());
            }
        }
    }
}