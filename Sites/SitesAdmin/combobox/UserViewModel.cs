using System.Collections.Generic;

namespace YourNamespace.Models
{
    public class UserViewModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserContactNo { get; set; }
        public string Location { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<int> SiteId { get; set; } = new List<int>(); // Selected site IDs
        public List<string> SiteNames { get; set; } = new List<string>(); // Selected site names
    }

    public class SiteModel
    {
        public int SiteId { get; set; }
        public string SiteName { get; set; }
    }
}
