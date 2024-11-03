namespace BCES.Models
{
    public class RebuiltParts
    {
        public int RebuiltPartID { get; set; }
        public string RebuiltStockCode { get; set; }
        public string Description { get; set; }
        public string Keyword { get; set; }
        public string JobNumber { get; set; }
        public string MMSBuyCode { get; set; }
        public string CoreCode { get; set; }
        public decimal CoreCost { get; set; }
        public string SOPNumber { get; set; }
        public int BusSeriesID { get; set; }
        public int PartTypeID { get; set; }
    }
}