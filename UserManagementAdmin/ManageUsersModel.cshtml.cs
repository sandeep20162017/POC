using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System;

namespace BCES.Pages.Admin
{
    public class UserManagementModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public UserManagementModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public void OnGet()
        {
            try
            {
                // Get roles from the database
                var roles = GetRolesFromDatabase();
                ViewData["Roles"] = roles;
            }
            catch (Exception ex)
            {
                // Handle exception
                ViewData["ErrorMessage"] = "An error occurred while fetching roles from the database: " + ex.Message;
            }
        }

        public JsonResult OnPostGetUsers()
        {
            try
            {
                var users = GetUsersFromDatabase();
                return new JsonResult(users);
            }
            catch (Exception ex)
            {
                // Handle exception
                return new JsonResult(new { error = "An error occurred while fetching users from the database: " + ex.Message });
            }
        }

        public JsonResult OnPostUpdateUser([FromBody] UserView user)
        {
            try
            {
                var roleId = GetRoleIdByName(user.RoleName);
                UpdateUserRoleInDatabase(user.UserId, roleId);
                return new JsonResult(new { success = true });
            }
            catch (Exception ex)
            {
                // Handle exception
                return new JsonResult(new { error = "An error occurred while updating the user role in the database: " + ex.Message });
            }
        }

        private List<Role> GetRolesFromDatabase()
        {
            return _db.Roles.ToList();
        }

        private List<UserView> GetUsersFromDatabase()
        {
            return _db.Users
                .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
                .Select(u => new UserView
                {
                    UserId = u.UserId,
                    UserName = u.UserName,
                    RoleName = u.UserRoles.FirstOrDefault().Role.RoleName
                })
                .ToList();
        }

        private int GetRoleIdByName(string roleName)
        {
            var role = _db.Roles.FirstOrDefault(r => r.RoleName == roleName);
            return role?.RoleId ?? 0;
        }

        private void UpdateUserRoleInDatabase(int userId, int roleId)
        {
            var userRole = _db.UserRoles.FirstOrDefault(ur => ur.UserId == userId);
            if (userRole != null)
            {
                userRole.RoleId = roleId;
                _db.SaveChanges();
            }
        }
    }

    public class UserView
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
    }

    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}