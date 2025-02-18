using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourNamespace.Models
{
    public class EmployeeLabour
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(50)]
        public string? LabourDefn { get; set; }

        public DateTime? DateEntered { get; set; }

        [StringLength(25)]
        public string? LinkNumber { get; set; }

        [StringLength(5)]
        public string? TypeId { get; set; }

        [StringLength(5)]
        public string? CostCentre { get; set; }

        [StringLength(50)]
        public string? Task { get; set; }

        public int? LabourType { get; set; }

        [StringLength(25)]
        public string? Usage { get; set; }

        [StringLength(25)]
        public string? HrsReqd { get; set; }

        [StringLength(25)]
        public string? AdjHrs { get; set; }

        public DateOnly? DateRevised { get; set; }  // Using DateOnly for date (no time component)

        [Column(TypeName = "numeric(18,2)")]
        public decimal? TimeAddition { get; set; }

        [StringLength(30)]
        public string? RebuiltPartNum { get; set; }

        [StringLength(25)]
        public string? LastModifiedBy { get; set; }
    }
}
