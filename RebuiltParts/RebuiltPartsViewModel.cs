using BCES.Models.Common;

namespace BCES.Models.Parts
{
    public class RebuiltPartsViewModel
    {
      public int RbMasterlistId {  get; set; }
        public string MmsStockCode { get; set; }
        public string DetailedDesc { get; set; }
        public string Keyword { get; set; }
        public string JobNumber { get; set; }
        public string RebuiltStockNum { get; set; }
        public string? CoreCharge { get; set; }
        public string? CorePartNum { get; set; }

        public List<ListOfBusesModel> VehicleSeries { get; set; } = new List<ListOfBusesModel>();

        public string LastModifiedBy { get; set; }
        public string LastModifiedDate { get; set; }
        public string SopNumber { get; set; }
        public string? BuyNewCost { get; set; }
        public string? RemanCost { get; set; }
        public bool IsActive { get; set; }
      
    }
}
