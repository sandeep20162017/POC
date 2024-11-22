using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BCES.Admin.Models
{
    public class UserViewModel
    {
        public int UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public int SelectedRole { get; set; }

        [Required]
        public List<int> SelectedSites { get; set; }
    }
}