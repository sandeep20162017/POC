public class UserModel
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public int RoleId { get; set; }    // Direct RoleId reference for each user
}
