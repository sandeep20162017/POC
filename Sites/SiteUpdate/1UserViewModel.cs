public class UserViewModel
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; }
    public List<SiteModel> Sites { get; set; }
    public List<int> SelectedSites { get; set; }

    public UserViewModel()
    {
        SelectedSites = new List<int>();
    }
}