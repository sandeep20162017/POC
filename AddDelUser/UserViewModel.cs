public class UserViewModel
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public RoleModel RoleModel { get; set; } = new RoleModel(); // Initialize to avoid null reference errors
}
