using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using BCES.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace BCES.Pages
{
    public class ManageUsersModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public ManageUsersModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetUsersAsync()
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var users = await db.QueryAsync<UserViewModel>("BCES.GetAllUsersWithRoles", commandType: CommandType.StoredProcedure);
                return new JsonResult(users);
            }
        }

        public async Task<IActionResult> OnPostCreateUserAsync([FromBody] UserViewModel user)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserName", user.UserName);
                parameters.Add("@RoleId", user.RoleId);

                await db.ExecuteAsync("BCES.AddUser", parameters, commandType: CommandType.StoredProcedure);
                return new JsonResult(new { success = true });
            }
        }

        public async Task<IActionResult> OnPostUpdateUserAsync([FromBody] UserViewModel user)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", user.UserId);
                parameters.Add("@UserName", user.UserName);
                parameters.Add("@RoleId", user.RoleId);

                await db.ExecuteAsync("BCES.UpdateUser", parameters, commandType: CommandType.StoredProcedure);
                return new JsonResult(new { success = true });
            }
        }

        public async Task<IActionResult> OnPostDeleteUserAsync([FromBody] int userId)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@UserId", userId);

                await db.ExecuteAsync("BCES.DeleteUser", parameters, commandType: CommandType.StoredProcedure);
                return new JsonResult(new { success = true });
            }
        }
    }

    public class UserViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}