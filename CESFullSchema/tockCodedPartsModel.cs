using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourNamespace.Models
{
    [Table("StockCodedParts", Schema = "BCES")]
    public class StockCodedParts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PartId { get; set; }

        [Column("OrigSuppNum")]
        [StringLength(50)]
        public string OrigSuppNum { get; set; }

        [Column("OrigSupplierName")]
        [StringLength(50)]
        public string OrigSupplierName { get; set; }

        [Column("CostCentre")]
        [StringLength(4)]
        public string CostCentre { get; set; }

        [Column("QtyReqd")]
        [StringLength(10)]
        public string QtyReqd { get; set; }

        [Column("PercentUsage")]
        [StringLength(10)]
        public string PercentUsage { get; set; }

        [Column("DateEntered")]
        public DateTime? DateEntered { get; set; }

        [Column("EnteredBy")]
        [StringLength(25)]
        public string EnteredBy { get; set; }

        [Column("LastModifiedBy")]
        [StringLength(25)]
        public string LastModifiedBy { get; set; }

        [Column("LastModifiedDate")]
        public DateTime? LastModifiedDate { get; set; }

        [Column("LinkCode")]
        [StringLength(30)]
        public string LinkCode { get; set; }

        [Column("Cost")]
        [Column(TypeName = "numeric(18, 2)")]
        public decimal? Cost { get; set; }

        [Column("LinkType")]
        [StringLength(3)]
        public string LinkType { get; set; }

        [Column("CoreCost")]
        [Column(TypeName = "numeric(18, 2)")]
        public decimal? CoreCost { get; set; }

        [Column("MmsStockCode")]
        [StringLength(30)]
        [Required]
        public string MmsStockCode { get; set; }

        [Column("OtherStockCodedPartsColumns")]
        [StringLength(255)]
        public string OtherStockCodedPartsColumns { get; set; }

        [Column("CorePartNumber")]
        [StringLength(30)]
        public string CorePartNumber { get; set; }

        [Column("PartType")]
        [StringLength(50)]
        public string PartType { get; set; }

        [Column("JobNumber")]
        [StringLength(30)]
        public string JobNumber { get; set; }

        [Column("DetailedDesc")]
        [StringLength(500)]
        public string DetailedDesc { get; set; }

        [Column("ItemRefNumber")]
        [StringLength(16)]
        public string ItemRefNumber { get; set; }

        [Column("OverheadType")]
        [Column(TypeName = "numeric(9, 0)")]
        public decimal? OverheadType { get; set; }

        [Column("CoreCharge")]
        [Column(TypeName = "numeric(9, 0)")]
        public decimal? CoreCharge { get; set; }

        [Column("PartCost")]
        [Column(TypeName = "numeric(9, 0)")]
        public decimal? PartCost { get; set; }

        [Column("MmsNewCost")]
        [Column(TypeName = "numeric(15, 5)")]
        public decimal? MmsNewCost { get; set; }

        [Column("OemPartCost")]
        [Column(TypeName = "numeric(9, 0)")]
        public decimal? OemPartCost { get; set; }

        [Column("MmsSyncDate")]
        public DateTime? MmsSyncDate { get; set; }
    }
}