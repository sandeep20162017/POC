public class UserViewModel
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; }

    public string SiteIds { get; set; } // Comma-separated Site IDs from SQL
    public string SiteNames { get; set; } // Comma-separated Site Names from SQL

    public List<int> SiteIdsList { get; set; } // Parsed Site IDs
    public List<string> SiteNamesList { get; set; } // Parsed Site Names
}
