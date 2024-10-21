using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

public class ManageUsersModel : PageModel
{
    // Sample data for demonstration purposes
    private static List<User> _users = new List<User>
    {
        new User { UserId = 1, UserName = "User1" },
        new User { UserId = 2, UserName = "User2" }
    };

    private static List<Role> _roles = new List<Role>
    {
        new Role { RoleId = 1, RoleName = "Admin" },
        new Role { RoleId = 2, RoleName = "User" },
        new Role { RoleId = 3, RoleName = "Guest" }
    };

    private static List<UserRole> _userRoles = new List<UserRole>
    {
        new UserRole { UserId = 1, RoleId = 1 },
        new UserRole { UserId = 2, RoleId = 2 }
    };

    public List<UserView> UserViews { get; set; }

    public void OnGet()
    {
        // Populate UserViews
        UserViews = _users.Select(u => new UserView
        {
            UserId = u.UserId,
            UserName = u.UserName,
            RoleName = _roles.FirstOrDefault(r => r.RoleId == _userRoles.FirstOrDefault(ur => ur.UserId == u.UserId)?.RoleId)?.RoleName
        }).ToList();

        // Populate ViewData with roles
        ViewData["Roles"] = _roles;
    }

    public IActionResult OnPost(UserView user)
    {
        // Handle the update logic here
        // You can update the user's role in the database

        return new JsonResult(new { success = true });
    }
}