using Dapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;

namespace BCES.Controllers.Admin
{
    public class UserManagementGridController : Controller
    {
        private readonly IDbConnection _dbConnection;

        public UserManagementGridController(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<IActionResult> ReadUsers([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var users = await GetUsersWithRoles();
                return Json(users.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching users.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UserViewAdd([DataSourceRequest] DataSourceRequest request, [FromForm] UserViewModel user)
        {
            try
            {
                if (user.RoleId <= 0)
                {
                    ModelState.AddModelError("RoleId", "Please select a valid role.");
                    return Json(new[] { user }.ToDataSourceResult(request, ModelState));
                }

                var userId = await AddUserAsync(user.UserName, user.RoleId);
                user.UserId = userId;

                return Json(new[] { user }.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while adding the user.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<UserViewModel> userViewModels)
        {
            try
            {
                var user = userViewModels.FirstOrDefault();
                if (user != null && user.RoleId > 0)
                {
                    await UpdateUserAsync(user.UserId, user.UserName, user.RoleId);
                }

                return Json(userViewModels.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while updating the user.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<UserViewModel> userViewModels)
        {
            try
            {
                var user = userViewModels.FirstOrDefault();
                if (user != null)
                {
                    await DeleteUserAsync(user.UserId);
                }

                return Json(userViewModels.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the user.");
            }
        }

        private async Task<IEnumerable<UserViewModel>> GetUsersWithRoles()
        {
            var query = @"
                SELECT u.UserId, u.UserName, u.RoleId, r.RoleName
                FROM Users u
                LEFT JOIN Roles r ON u.RoleId = r.RoleId";

            return await _dbConnection.QueryAsync<UserViewModel>(query);
        }

        private async Task<int> AddUserAsync(string userName, int roleId)
        {
            var query = "INSERT INTO Users (UserName, RoleId) VALUES (@UserName, @RoleId); SELECT CAST(SCOPE_IDENTITY() as int);";
            return await _dbConnection.ExecuteScalarAsync<int>(query, new { UserName = userName, RoleId = roleId });
        }

        private async Task UpdateUserAsync(int userId, string userName, int roleId)
        {
            var query = "UPDATE Users SET UserName = @UserName, RoleId = @RoleId WHERE UserId = @UserId;";
            await _dbConnection.ExecuteAsync(query, new { UserId = userId, UserName = userName, RoleId = roleId });
        }

        private async Task DeleteUserAsync(int userId)
        {
            var query = "DELETE FROM Users WHERE UserId = @UserId;";
            await _dbConnection.ExecuteAsync(query, new { UserId = userId });
        }

        private async Task<IEnumerable<RoleModel>> GetRoles()
        {
            var query = "SELECT RoleId, RoleName FROM Roles";
            return await _dbConnection.QueryAsync<RoleModel>(query);
        }

        public async Task<IActionResult> Index()
        {
            var roles = await GetRoles();
            ViewData["Roles"] = roles;
            return View();
        }
    }
}
