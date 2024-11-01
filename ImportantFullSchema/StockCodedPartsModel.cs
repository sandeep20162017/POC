namespace BCES.Models
{
    public class StockCodedParts
    {
        public int StockPartID { get; set; }
        public int RebuiltPartID { get; set; }
        public string MMSStockCode { get; set; }
        public string PartType { get; set; }
        public string PartDescription { get; set; }
        public int CostCentreID { get; set; }
        public decimal PercentUsage { get; set; }
        public int Quantity { get; set; }
        public DateTime LastRevised { get; set; }
        public decimal CoreCost { get; set; }
    }
}