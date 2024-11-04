// StockCodedPartsController.cs
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
    public class StockCodedPartsController : Controller
    {
        private readonly string _connectionString = "YourConnectionStringHere";

        // GET: StockCodedParts
        public ActionResult Index()
        {
            return View();
        }

        // GET: StockCodedParts/Read
        // Returns a JSON result of StockCodedParts
        public JsonResult Read()
        {
            try
            {
                var stockCodedParts = GetStockCodedParts();
                return Json(stockCodedParts, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // Log the exception
                return Json(new { error = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        // GET: StockCodedParts/GetStockCodedParts
        // Makes a Dapper SQL call to retrieve StockCodedParts
        private List<StockCodedPartsModel> GetStockCodedParts()
        {
            using (IDbConnection db = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT 
                        SCP.*,
                        AP.*
                    FROM 
                        BCES.StockCodedParts SCP
                    INNER JOIN 
                        BCES.AllParts AP ON SCP.PartID = AP.PartID";
                return db.Query<StockCodedPartsModel, AllPartsModel, StockCodedPartsModel>(query, (scp, ap) =>
                {
                    scp.AllPartsModel = ap;
                    return scp;
                }, splitOn: "PartID").ToList();
            }
        }

        // POST: StockCodedParts/Add
        // Adds a new StockCodedPart
        [HttpPost]
        public JsonResult Add(StockCodedPartsModel stockCodedPart)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    // Insert into AllParts table
                    string allPartsQuery = @"
                        INSERT INTO BCES.AllParts (MMSStockCode, CorePartNumber, PartType, JobNumber, DetailedDesc, DateEntered, ItemRefNumber, OverheadType, CoreCharge, PartCost, AddedBy, MMSNewCost, MSSyncDate, OrigSupplierNum, OrigSupplierName, LastModifiedBy, LastModifiedDate)
                        VALUES (@MMSStockCode, @CorePartNumber, @PartType, @JobNumber, @DetailedDesc, @DateEntered, @ItemRefNumber, @OverheadType, @CoreCharge, @PartCost, @AddedBy, @MMSNewCost, @MSSyncDate, @OrigSupplierNum, @OrigSupplierName, @LastModifiedBy, @LastModifiedDate);
                        SELECT CAST(SCOPE_IDENTITY() as int)";
                    int partID = db.Query<int>(allPartsQuery, stockCodedPart).Single();

                    // Insert into StockCodedParts table
                    string stockCodedPartsQuery = @"
                        INSERT INTO BCES.StockCodedParts (PartID, MMSStockCode, CorePartNumber, PartType, JobNumber, DetailedDesc, DateEntered, ItemRefNumber, OverheadType, CoreCharge, PartCost, AddedBy, MMSNewCost, MSSyncDate, OrigSupplierNum, OrigSupplierName, LastModifiedBy, LastModifiedDate)
                        VALUES (@PartID, @MMSStockCode, @CorePartNumber, @PartType, @JobNumber, @DetailedDesc, @DateEntered, @ItemRefNumber, @OverheadType, @CoreCharge, @PartCost, @AddedBy, @MMSNewCost, @MSSyncDate, @OrigSupplierNum, @OrigSupplierName, @LastModifiedBy, @LastModifiedDate)";
                    db.Execute(stockCodedPartsQuery, new { PartID = partID, stockCodedPart.MMSStockCode, stockCodedPart.CorePartNumber, stockCodedPart.PartType, stockCodedPart.JobNumber, stockCodedPart.DetailedDesc, stockCodedPart.DateEntered, stockCodedPart.ItemRefNumber, stockCodedPart.OverheadType, stockCodedPart.CoreCharge, stockCodedPart.PartCost, stockCodedPart.AddedBy, stockCodedPart.MMSNewCost, stockCodedPart.MSSyncDate, stockCodedPart.OrigSupplierNum, stockCodedPart.OrigSupplierName, stockCodedPart.LastModifiedBy, stockCodedPart.LastModifiedDate });
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception
                return Json(new { error = ex.Message });
            }
        }

        // POST: StockCodedParts/Update
        // Updates an existing StockCodedPart
        [HttpPost]
        public JsonResult Update(StockCodedPartsModel stockCodedPart)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    // Update AllParts table
                    string allPartsQuery = @"
                        UPDATE BCES.AllParts
                        SET MMSStockCode = @MMSStockCode, CorePartNumber = @CorePartNumber, PartType = @PartType, JobNumber = @JobNumber, DetailedDesc = @DetailedDesc, DateEntered = @DateEntered, ItemRefNumber = @ItemRefNumber, OverheadType = @OverheadType, CoreCharge = @CoreCharge, PartCost = @PartCost, AddedBy = @AddedBy, MMSNewCost = @MMSNewCost, MSSyncDate = @MSSyncDate, OrigSupplierNum = @OrigSupplierNum, OrigSupplierName = @OrigSupplierName, LastModifiedBy = @LastModifiedBy, LastModifiedDate = @LastModifiedDate
                        WHERE PartID = @PartID";
                    db.Execute(allPartsQuery, stockCodedPart);

                    // Update StockCodedParts table
                    string stockCodedPartsQuery = @"
                        UPDATE BCES.StockCodedParts
                        SET MMSStockCode = @MMSStockCode, CorePartNumber = @CorePartNumber, PartType = @PartType, JobNumber = @JobNumber, DetailedDesc = @DetailedDesc, DateEntered = @DateEntered, ItemRefNumber = @ItemRefNumber, OverheadType = @OverheadType, CoreCharge = @CoreCharge, PartCost = @PartCost, AddedBy = @AddedBy, MMSNewCost = @MMSNewCost, MSSyncDate = @MSSyncDate, OrigSupplierNum = @OrigSupplierNum, OrigSupplierName = @OrigSupplierName, LastModifiedBy = @LastModifiedBy, LastModifiedDate = @LastModifiedDate
                        WHERE PartID = @PartID";
                    db.Execute(stockCodedPartsQuery, stockCodedPart);
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log the exception
                return Json(new { error = ex.Message });
            }
        }

        // POST: StockCodedParts/Delete
        // Deletes an existing StockCodedPart
        [HttpPost]
        public JsonResult Delete(int partID)
        {
            try
            {
                using (IDbConnection db = new SqlConnection(_connectionString))
                {
                    // Delete from StockCodedParts table
                    string stockCodedPartsQuery = "DELETE FROM BCES.StockCodedParts WHERE PartID = @PartID";
                    db.Execute(stockCodedPartsQuery, new { PartID = partID });

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