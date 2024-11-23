public class UserViewModel
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; }

    // Temporary property to store the comma-separated SiteIds from SQL
    public string SiteIdsString { get; set; }

    // Final parsed property
    public List<int> SiteIds { get; set; }
}
