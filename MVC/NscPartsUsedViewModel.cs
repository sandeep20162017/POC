using System;

namespace BCES.Models.Parts
{
    public class NscPartsUsedViewModel
    {
        // NScPartsUsed Table Columns
        public string OrigSuppNum { get; set; }
        public string OrigSupplierName { get; set; }
        public string CostCentre { get; set; }
        public string QtyReqd { get; set; }
        public string PercentUsage { get; set; }
        public DateTime? DateEntered { get; set; }
        public string EnteredBy { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
        public string LinkCode { get; set; }
        public decimal? Cost { get; set; }
        public string LinkType { get; set; }
        public decimal? CoreCost { get; set; }
        public int Id { get; set; }

        // CostCenters Table Columns
        public string CostCentreName { get; set; }

        // Add other fields from related tables as needed
    }
}