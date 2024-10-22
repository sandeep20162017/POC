using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace BCES.Pages.Admin
{
    public class UserManagementModel : PageModel
    {
        private readonly string _connectionString;

        public UserManagementModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void OnGet()
        {
            try
            {
                // Get roles from the database
                var roles = GetRolesFromDatabase();
                ViewData["Roles"] = roles;
            }
            catch (SqlException ex)
            {
                // Handle SQL exception
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
            catch (SqlException ex)
            {
                // Handle SQL exception
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
            catch (SqlException ex)
            {
                // Handle SQL exception
                return new JsonResult(new { error = "An error occurred while updating the user role in the database: " + ex.Message });
            }
        }

        private List<Role> GetRolesFromDatabase()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<Role>("SELECT RoleId, RoleName FROM BCES.Roles").ToList();
            }
        }

        private List<UserView> GetUsersFromDatabase()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.Query<UserView>(@"
                    SELECT u.UserId, u.UserName, r.RoleName
                    FROM BCES.Users u
                    JOIN BCES.UserRoles ur ON u.UserId = ur.UserId
                    JOIN BCES.Roles r ON ur.RoleId = r.RoleId
                ").ToList();
            }
        }

        private int GetRoleIdByName(string roleName)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return connection.QueryFirstOrDefault<int>("SELECT RoleId FROM BCES.Roles WHERE RoleName = @RoleName", new { RoleName = roleName });
            }
        }

        private void UpdateUserRoleInDatabase(int userId, int roleId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                connection.Execute(@"
                    UPDATE BCES.UserRoles
                    SET RoleId = @RoleId
                    WHERE UserId = @UserId
                ", new { UserId = userId, RoleId = roleId });
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