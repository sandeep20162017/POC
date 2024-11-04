// StockCodedPartsModel.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCES.Models.Parts
{
    [Table("StockCodedParts", Schema = "BCES")]
    public class StockCodedPartsModel
    {
        [Key]
        [ForeignKey("AllPartsModel")]
        public int PartID { get; set; }

        public string MMSStockCode { get; set; }
        public string CorePartNumber { get; set; }
        public string PartType { get; set; }
        public string JobNumber { get; set; }
        public string DetailedDesc { get; set; }
        public DateTime DateEntered { get; set; }
        public string ItemRefNumber { get; set; }
        public decimal OverheadType { get; set; }
        public decimal CoreCharge { get; set; }
        public decimal PartCost { get; set; }
        public string AddedBy { get; set; }
        public decimal MMSNewCost { get; set; }
        public DateTime MSSyncDate { get; set; }
        public string OrigSupplierNum { get; set; }
        public string OrigSupplierName { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public virtual AllPartsModel AllPartsModel { get; set; }
    }
}