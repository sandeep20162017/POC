namespace BCES.Models.Admin
{
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public RoleModel RoleModel { get; set; } = new RoleModel(); // Initialize RoleModel
    }

    public class RoleModel
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}