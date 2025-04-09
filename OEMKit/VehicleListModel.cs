using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourProjectNamespace.Models
{
    [Table("VehicleList", Schema = "SBCES")]
    public class VehicleListModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // Assuming ID is manually assigned
        [Display(Name = "Vehicle List ID")]
        public int VehicleListId { get; set; }

        [Required(ErrorMessage = "Vehicle Series Code is required")]
        [StringLength(500)]
        [Display(Name = "Vehicle Series Code")]
        public string VehSeriesCode { get; set; }

        [Required(ErrorMessage = "Number of Vehicles is required")]
        [StringLength(10)]
        [Display(Name = "Number of Vehicles")]
        public string NumOfVehicles { get; set; }

        [Required(ErrorMessage = "Project Description is required")]
        [StringLength(200)]
        [Display(Name = "Project Description")]
        public string ProjDesc { get; set; }

        [Required]
        [Display(Name = "Date Entered")]
        public DateTime DateEntered { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Entered By is required")]
        [StringLength(25)]
        [Display(Name = "Entered By")]
        public string EnteredBy { get; set; }

        [Required(ErrorMessage = "Last Modified By is required")]
        [StringLength(25)]
        [Display(Name = "Last Modified By")]
        public string ModifiedLastBy { get; set; }

        [Required]
        [Display(Name = "Last Modified Date")]
        public DateTime ModifiedLastDate { get; set; }

        [StringLength(25)]
        public string? Make { get; set; }

        [StringLength(25)]
        public string? Model { get; set; }

        [StringLength(4)]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Year must be 4 digits")]
        public string? Year { get; set; }

        [StringLength(25)]
        public string? Engine { get; set; }

        [StringLength(25)]
        public string? Transmission { get; set; }

        [StringLength(25)]
        public string? Differential { get; set; }

        [StringLength(30)]
        [Display(Name = "SOP Number")]
        public string? Sopnumber { get; set; }

        public VehicleListModel()
        {
            // Initialize required fields
            VehSeriesCode ??= string.Empty;
            NumOfVehicles ??= string.Empty;
            ProjDesc ??= string.Empty;
            EnteredBy ??= string.Empty;
            ModifiedLastBy ??= string.Empty;
        }
    }
}
