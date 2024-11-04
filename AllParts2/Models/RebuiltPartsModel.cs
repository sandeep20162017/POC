// RebuiltPartsModel.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCES.Models.Parts
{
    [Table("RebuiltParts", Schema = "BCES")]
    public class RebuiltPartsModel
    {
        [Key]
        [ForeignKey("AllPartsModel")]
        public int PartID { get; set; }

        public string RebuiltStockCode { get; set; }
        public string Description { get; set; }
        public string Keyword { get; set; }
        public string JobNumber { get; set; }
        public decimal MMSBuyCode { get; set; }
        public decimal ToreCode { get; set; }
        public decimal CoreCost { get; set; }
        public string SOPNumber { get; set; }

        public virtual AllPartsModel AllPartsModel { get; set; }
    }
}