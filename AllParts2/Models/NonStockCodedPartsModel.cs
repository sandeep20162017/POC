// NonStockCodedPartsModel.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCES.Models.Parts
{
    [Table("NonStockCodedParts", Schema = "BCES")]
    public class NonStockCodedPartsModel
    {
        [Key]
        [ForeignKey("AllPartsModel")]
        public int PartID { get; set; }

        public string SupplierName { get; set; }
        public string SupplierNumber { get; set; }
        public decimal PartUnitCost { get; set; }

        public virtual AllPartsModel AllPartsModel { get; set; }
    }
}