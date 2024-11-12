using System;

namespace BCES.Models.Parts
{
    public class ScPartsUsedViewModel
    {
        // ScPartsUsed Table Columns
        public string MmsStockCode { get; set; }
        public DateTime? DateEntered { get; set; }
        public decimal? RebPartCost { get; set; }
        public string UserEntered { get; set; }
        public decimal? MmsCost { get; set; }
        public decimal? OemCost { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string MmsRebuiltCode { get; set; }
        public string CostCentre { get; set; }
        public string QtyReqd { get; set; }
        public string PercentUsage { get; set; }
        public string LinkCode { get; set; }
        public string RebuiltPart { get; set; }
        public string LinkType { get; set; }
        public decimal? CoreCost { get; set; }
        public string OrigSuppNum { get; set; }
        public string OrigSupplierName { get; set; }

        // CostCenters Table Columns
        public string CostCentreName { get; set; }

        // Add other fields from related tables as needed
    }
}