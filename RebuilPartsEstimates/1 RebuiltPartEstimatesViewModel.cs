namespace BCES.Models.Parts
{
    public class RebuiltPartEstimates
    {
        public string JobNumber { get; set; }
        public string RebuiltNumber { get; set; }
        public string MMSBuyCode { get; set; }
        public string SOPNumber { get; set; }
        public string Keyword { get; set; }
        public string CoreCode { get; set; }
        public string Description { get; set; }
        public string VehicleSeries { get; set; }
        public string LastModified { get; set; }
        public List<LabourGridModel> LabourGrid { get; set; }
        public List<SummaryGridModel> LabourHourSummary { get; set; }
        public List<SummaryGridModel> MaterialCostSummary { get; set; }
        public decimal CoreCost { get; set; }
        public decimal MaterialsCost { get; set; }
        public decimal LabourCost { get; set; }
        public decimal LabourOverheadCost { get; set; }
        public decimal LabourTotal { get; set; }
        public decimal TotalRebuiltPartCost { get; set; }
        public decimal TaxTotal { get; set; }
        public decimal TotalRebuiltPartWithCost { get; set; }
        public string AdditionalInfo1 { get; set; }
        public string AdditionalInfo2 { get; set; }
    }

    public class LabourGridModel
    {
        public int Id { get; set; }
        public string LabourCode { get; set; }
        public string Description { get; set; }
        public decimal Hours { get; set; }
        public decimal Rate { get; set; }
        public decimal TotalCost { get; set; }
    }

    public class SummaryGridModel
    {
        public string Category { get; set; }
        public decimal Amount { get; set; }
    }
}