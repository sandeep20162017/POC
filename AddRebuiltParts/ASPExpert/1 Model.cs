namespace BCES.Models.RebuiltParts
{
    public class RebuiltPartViewModel
    {
        public string StockCode { get; set; }
        public string Description { get; set; }
        public string Keyword { get; set; }
        public string JobNumber { get; set; }
        public string MMSBuyCode { get; set; }
        public decimal? CoreCost { get; set; }
        public string SOPNumber { get; set; }
        public decimal? BuyCost { get; set; }
        public decimal? RemanufacturedCost { get; set; }
        public bool IsActive { get; set; }
        public List<string> Vehicles { get; set; }
    }
}
