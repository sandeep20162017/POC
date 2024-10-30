using System;

namespace BCES.Models.Parts
{
    public class RebuildPartsModel
    {
        public int StockPartID { get; set; }
        public string MMSStockCode { get; set; }
        public string StockPartDescription { get; set; }
        public string StockPartKeyword { get; set; }
        public int Quantity { get; set; }
        public decimal CoreCost { get; set; }
        public string RebuiltStockCode { get; set; }
        public string RebuiltPartDescription { get; set; }
        public string RebuiltPartKeyword { get; set; }
        public string JobNumber { get; set; }
        public string CoreCode { get; set; }
        public decimal RebuiltPartCoreCost { get; set; }
        public string SOPNumber { get; set; }
        public string BusSeriesCode { get; set; }
        public string BusSeriesDescription { get; set; }
        public string PartTypeName { get; set; }
        public string CostCentreCode { get; set; }
        public string CostCentreDescription { get; set; }
    }
}