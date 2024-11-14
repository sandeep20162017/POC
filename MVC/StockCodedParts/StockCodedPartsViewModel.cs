using System;
using System.ComponentModel.DataAnnotations;

namespace BCES.Models.Parts
{
    public class StockCodedPartsViewModel
    {
        [Required]
        [StringLength(30)]
        public string PartNo { get; set; }

        [StringLength(30)]
        public string CorePartNumber { get; set; }

        [StringLength(50)]
        public string PartType { get; set; }

        [StringLength(30)]
        public string JobNumber { get; set; }

        [StringLength(500)]
        public string DetailedDesc { get; set; }

        [StringLength(8)]
        public string DateEntered { get; set; }

        [StringLength(16)]
        public string ItemRefNumber { get; set; }

        public int? OverheadType { get; set; }

        public int? CoreCharge { get; set; }

        public int? PartCost { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        [StringLength(30)]
        public string LastModifiedBy { get; set; }

        [StringLength(30)]
        public string AddedBy { get; set; }

        public decimal? MmsNewCost { get; set; }

        public DateTime? MmsSyncDate { get; set; }

        [StringLength(25)]
        public string OrigSupplierNum { get; set; }

        [StringLength(40)]
        public string OrigSupplierName { get; set; }
    }
}