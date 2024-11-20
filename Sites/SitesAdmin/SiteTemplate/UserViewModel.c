public class UserViewModel
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; }
    public List<Site> Sites { get; set; } = new List<Site>();
}

public class Site
{
    public int SiteId { get; set; }
    public string Sitename { get; set; }
    public bool Selected { get; set; }
}