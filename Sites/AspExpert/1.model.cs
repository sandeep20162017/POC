public class UserViewModel
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; }
    public List<int> SiteIds { get; set; } // To hold selected site IDs
}

public class SiteModel
{
    public int SiteId { get; set; }
    public string SiteName { get; set; }
}
