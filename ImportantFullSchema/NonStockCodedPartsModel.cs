namespace BCES.Models
{
    public class NonStockCodedParts
    {
        public int NonStockPartID { get; set; }
        public int RebuiltPartID { get; set; }
        public int SupplierID { get; set; }
        public string PartDescription { get; set; }
        public string Keyword { get; set; }
        public int Quantity { get; set; }
        public decimal UnitCost { get; set; }
        public decimal CoreCost { get; set; }
        public int CostCentreID { get; set; }
        public string SupplierName { get; set; }
        public string SupplierNumber { get; set; }
        public decimal PartUnitCost { get; set; }
    }
}