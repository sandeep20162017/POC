// NSCPartsUsedModel.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BCES.Models.Parts
{
    [Table("NSCPartsUsed", Schema = "BCES")]
    public class NSCPartsUsedModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UsageID { get; set; }

        [ForeignKey("AllPartsModel")]
        public int PartID { get; set; }

        public string CostCentre { get; set; }
        public string QtyReqd { get; set; }
        public string PercentUsage { get; set; }
        public DateTime DateEntered { get; set; }
        public string EnteredBy { get; set; }
        public string LinkCode { get; set; }
        public decimal Cost { get; set; }
        public string LinkType { get; set; }
        public decimal CoreCost { get; set; }
        public string OrigSupNum { get; set; }
        public string OrigSupplierName { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }

        public virtual AllPartsModel AllPartsModel { get; set; }
    }
}