using Dapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;
using BCES.Models.Admin;

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
                var userViews = await GetUserViews();
                return Json(userViews.ToDataSourceResult(request));
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
                if (user.RoleModel == null || user.RoleModel.RoleId <= 0)
                {
                    ModelState.AddModelError("RoleModel", "Please select a valid role.");
                    return Json(new[] { user }.ToDataSourceResult(request, ModelState));
                }

                var userId = await AddUserAsync(user.UserName, user.RoleModel.RoleId);
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
                var userViewModel = userViewModels.FirstOrDefault();
                if (userViewModel != null)
                {
                    userViewModel.RoleModel = userViewModel.RoleModel ?? new RoleModel();

                    await UpdateUserAsync(userViewModel.UserId, userViewModel.UserName, userViewModel.RoleModel.RoleId);
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
                var userViewModel = userViewModels.FirstOrDefault();
                if (userViewModel != null)
                {
                    await DeleteUserAsync(userViewModel.UserId);
                }

                return Json(userViewModels.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the user.");
            }
        }

        private async Task<IEnumerable<UserViewModel>> GetUserViews()
        {
            var query = @"
                SELECT u.UserId, u.UserName, r.RoleId, r.RoleName
                FROM BCES.Users u
                INNER JOIN BCES.UserRoles ur ON ur.UserId = u.UserId
                INNER JOIN BCES.Roles r ON r.RoleId = ur.RoleId";

            var userViews = await _dbConnection.QueryAsync<UserViewModel, RoleModel, UserViewModel>(
                query,
                (user, role) =>
                {
                    user.RoleModel = role;
                    return user;
                },
                splitOn: "RoleId"
            );
            return userViews.ToList();
        }

        private async Task<int> AddUserAsync(string userName, int roleId)
        {
            var insertUserQuery = "INSERT INTO BCES.Users (UserName) VALUES (@UserName); SELECT CAST(SCOPE_IDENTITY() as int);";
            var insertUserRoleQuery = "INSERT INTO BCES.UserRoles (UserId, RoleId) VALUES (@UserId, @RoleId);";

            using (var transaction = _dbConnection.BeginTransaction())
            {
                var newUserId = await _dbConnection.ExecuteScalarAsync<int>(insertUserQuery, new { UserName = userName }, transaction);
                await _dbConnection.ExecuteAsync(insertUserRoleQuery, new { UserId = newUserId, RoleId = roleId }, transaction);
                transaction.Commit();
                return newUserId;
            }
        }

        private async Task UpdateUserAsync(int userId, string userName, int roleId)
        {
            var updateUserQuery = "UPDATE BCES.Users SET UserName = @UserName WHERE UserId = @UserId;";
            var updateUserRoleQuery = "UPDATE BCES.UserRoles SET RoleId = @RoleId WHERE UserId = @UserId;";

            using (var transaction = _dbConnection.BeginTransaction())
            {
                await _dbConnection.ExecuteAsync(updateUserQuery, new { UserName = userName, UserId = userId }, transaction);
                await _dbConnection.ExecuteAsync(updateUserRoleQuery, new { RoleId = roleId, UserId = userId }, transaction);
                transaction.Commit();
            }
        }

        private async Task DeleteUserAsync(int userId)
        {
            var deleteUserRoleQuery = "DELETE FROM BCES.UserRoles WHERE UserId = @UserId;";
            var deleteUserQuery = "DELETE FROM BCES.Users WHERE UserId = @UserId;";

            using (var transaction = _dbConnection.BeginTransaction())
            {
                await _dbConnection.ExecuteAsync(deleteUserRoleQuery, new { UserId = userId }, transaction);
                await _dbConnection.ExecuteAsync(deleteUserQuery, new { UserId = userId }, transaction);
                transaction.Commit();
            }
        }
    }
}
