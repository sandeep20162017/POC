// NonStockCodedPartsController.cs
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
    public class NonStockCodedPartsController : Controller
    {
        private readonly string _connectionString = "YourConnectionStringHere";

        // GET: NonStockCodedParts
        public ActionResult Index()
        {
            return View();
        }

        // GET: NonStockCodedParts/Read
        // Returns a JSON result of NonStockCodedParts
        public JsonResult Read()
        {
            try
            {
                var nonStockCodedParts = GetNonStockCodedParts();
                return Json(nonStockCodedParts, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log the exception
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: NonStockCodedParts/GetNonStockCodedParts
        // Makes a Dapper SQL call to retrieve NonStockCodedParts
        private List<NonStockCodedPartsModel> GetNonStockCodedParts()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT 
                        NSCP.*,
                        AP.*
                    FROM 
                        BCES.NonStockCodedParts NSCP
                    INNER JOIN 
                        BCES.AllParts AP ON NSCP.PartID = AP.PartID";
                return db.Query<NonStockCodedPartsModel, AllPartsModel, NonStockCodedPartsModel>(query, (nscp, ap) =>
                {
                    nscp.AllPartsModel = ap;
                    return nscp;
                }, splitOn: "PartID").ToList();
            }
        }

        // POST: NonStockCodedParts/Add
        // Adds a new NonStockCodedPart
        [HttpPost]
        public JsonResult Add(NonStockCodedPartsModel nonStockCodedPart)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    // Insert into AllParts table
                    string allPartsQuery = @"
                        INSERT INTO BCES.AllParts (DetailedDesc, DateEntered, PartCost, AddedBy, OrigSupplierNum, OrigSupplierName, LastModifiedBy, LastModifiedDate)
                        VALUES (@DetailedDesc, @DateEntered, @PartCost, @AddedBy, @SupplierNumber, @SupplierName, @LastModifiedBy, @LastModifiedDate);
                        SELECT CAST(SCOPE_IDENTITY() as int)";
                    int partID = db.Query<int>(allPartsQuery, nonStockCodedPart).Single();

                    // Insert into NonStockCodedParts table
                    string nonStockCodedPartsQuery = @"
                        INSERT INTO BCES.NonStockCodedParts (PartID, SupplierName, SupplierNumber, PartUnitCost)
                        VALUES (@PartID, @SupplierName, @SupplierNumber, @PartUnitCost)";
                    db.Execute(nonStockCodedPartsQuery, new { PartID = partID, nonStockCodedPart.SupplierName, nonStockCodedPart.SupplierNumber, nonStockCodedPart.PartUnitCost });
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception
                return Json(new { error = ex.Message });
            }
        }

        // POST: NonStockCodedParts/Update
        // Updates an existing NonStockCodedPart
        [HttpPost]
        public JsonResult Update(NonStockCodedPartsModel nonStockCodedPart)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    // Update AllParts table
                    string allPartsQuery = @"
                        UPDATE BCES.AllParts
                        SET DetailedDesc = @DetailedDesc, DateEntered = @DateEntered, PartCost = @PartCost, AddedBy = @AddedBy, OrigSupplierNum = @SupplierNumber, OrigSupplierName = @SupplierName, LastModifiedBy = @LastModifiedBy, LastModifiedDate = @LastModifiedDate
                        WHERE PartID = @PartID";
                    db.Execute(allPartsQuery, nonStockCodedPart);

                    // Update NonStockCodedParts table
                    string nonStockCodedPartsQuery = @"
                        UPDATE BCES.NonStockCodedParts
                        SET SupplierName = @SupplierName, SupplierNumber = @SupplierNumber, PartUnitCost = @PartUnitCost
                        WHERE PartID = @PartID";
                    db.Execute(nonStockCodedPartsQuery, nonStockCodedPart);
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception
                return Json(new { error = ex.Message });
            }
        }

        // POST: NonStockCodedParts/Delete
        // Deletes an existing NonStockCodedPart
        [HttpPost]
        public JsonResult Delete(int partID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    // Delete from NonStockCodedParts table
                    string nonStockCodedPartsQuery = "DELETE FROM BCES.NonStockCodedParts WHERE PartID = @PartID";
                    db.Execute(nonStockCodedPartsQuery, new { PartID = partID });

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