// RebuiltPartsController.cs
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using BCES.Models.Parts;
using Dapper;

namespace BCES.Controllers
{
    public class RebuiltPartsController : Controller
    {
        private readonly string _connectionString = "YourConnectionStringHere";

        // GET: RebuiltParts
        public ActionResult Index()
        {
            return View();
        }

        // GET: RebuiltParts/Read
        // Returns a JSON result of RebuiltParts
        public JsonResult Read()
        {
            try
            {
                var rebuiltParts = GetRebuiltParts();
                return Json(rebuiltParts, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log the exception
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: RebuiltParts/GetRebuiltParts
        // Makes a Dapper SQL call to retrieve RebuiltParts
        private List<RebuiltPartsModel> GetRebuiltParts()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT 
                        RP.*,
                        AP.*
                    FROM 
                        BCES.RebuiltParts RP
                    INNER JOIN 
                        BCES.AllParts AP ON RP.PartID = AP.PartID";
                return db.Query<RebuiltPartsModel, AllPartsModel, RebuiltPartsModel>(query, (rp, ap) =>
                {
                    rp.AllPartsModel = ap;
                    return rp;
                }, splitOn: "PartID").ToList();
            }
        }

        // POST: RebuiltParts/Add
        // Adds a new RebuiltPart
        [HttpPost]
        public JsonResult Add(RebuiltPartsModel rebuiltPart)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    // Insert into AllParts table
                    string allPartsQuery = @"
                        INSERT INTO BCES.AllParts (MMSStockCode, CorePartNumber, JobNumber, DetailedDesc, DateEntered, CoreCharge, AddedBy, LastModifiedBy, LastModifiedDate)
                        VALUES (@RebuiltStockCode, @CorePartNumber, @JobNumber, @Description, @DateEntered, @CoreCost, @AddedBy, @LastModifiedBy, @LastModifiedDate);
                        SELECT CAST(SCOPE_IDENTITY() as int)";
                    int partID = db.Query<int>(allPartsQuery, rebuiltPart).Single();

                    // Insert into RebuiltParts table
                    string rebuiltPartsQuery = @"
                        INSERT INTO BCES.RebuiltParts (PartID, RebuiltStockCode, Description, Keyword, JobNumber, MMSBuyCode, ToreCode, CoreCost, SOPNumber)
                        VALUES (@PartID, @RebuiltStockCode, @Description, @Keyword, @JobNumber, @MMSBuyCode, @ToreCode, @CoreCost, @SOPNumber)";
                    db.Execute(rebuiltPartsQuery, new { PartID = partID, rebuiltPart.RebuiltStockCode, rebuiltPart.Description, rebuiltPart.Keyword, rebuiltPart.JobNumber, rebuiltPart.MMSBuyCode, rebuiltPart.ToreCode, rebuiltPart.CoreCost, rebuiltPart.SOPNumber });
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception
                return Json(new { error = ex.Message });
            }
        }

        // POST: RebuiltParts/Update
        // Updates an existing RebuiltPart
        [HttpPost]
        public JsonResult Update(RebuiltPartsModel rebuiltPart)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    // Update AllParts table
                    string allPartsQuery = @"
                        UPDATE BCES.AllParts
                        SET MMSStockCode = @RebuiltStockCode, CorePartNumber = @CorePartNumber, JobNumber = @JobNumber, DetailedDesc = @Description, DateEntered = @DateEntered, CoreCharge = @CoreCost, AddedBy = @AddedBy, LastModifiedBy = @LastModifiedBy, LastModifiedDate = @LastModifiedDate
                        WHERE PartID = @PartID";
                    db.Execute(allPartsQuery, rebuiltPart);

                    // Update RebuiltParts table
                    string rebuiltPartsQuery = @"
                        UPDATE BCES.RebuiltParts
                        SET RebuiltStockCode = @RebuiltStockCode, Description = @Description, Keyword = @Keyword, JobNumber = @JobNumber, MMSBuyCode = @MMSBuyCode, ToreCode = @ToreCode, CoreCost = @CoreCost, SOPNumber = @SOPNumber
                        WHERE PartID = @PartID";
                    db.Execute(rebuiltPartsQuery, rebuiltPart);
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception
                return Json(new { error = ex.Message });
            }
        }

        // POST: RebuiltParts/Delete
        // Deletes an existing RebuiltPart
        [HttpPost]
        public JsonResult Delete(int partID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    // Delete from RebuiltParts table
                    string rebuiltPartsQuery = "DELETE FROM BCES.RebuiltParts WHERE PartID = @PartID";
                    db.Execute(rebuiltPartsQuery, new { PartID = partID });

                    // Delete from AllParts table
                    string allPartsQuery = "DELETE FROM BCES.AllParts WHERE PartID = @PartID";
                    db.Execute(allPartsQuery, new { PartID = partID });
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception
                return Json(new { error = ex.Message });
            }
        }
    }
}