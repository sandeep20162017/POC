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

        // Constructor to inject database connection directly
        public UserManagementGridController(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection; // Initialize the database connection
        }

        /// <summary>
        /// Read action to retrieve users for Kendo Grid
        /// </summary>
        public async Task<IActionResult> ReadUsers([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var userViews = await GetUserViews();
                return Json(userViews.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, "An error occurred while fetching users.");
            }
        }

        /// <summary>
        /// Update action to modify user details
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> UpdateUser([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<UserViewModel> userViewModels)
        {
            try
            {
                var userViewModel = userViewModels.FirstOrDefault();
                if (userViewModel != null)
                {
                    userViewModel.RoleModel = userViewModel.RoleModel ?? new RoleModel();

                    // Update user data in the database
                    await UpdateUserAsync(userViewModel.UserId, userViewModel.UserName, userViewModel.RoleModel.RoleId);
                }

                return Json(userViewModels.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, "An error occurred while updating the user.");
            }
        }

        /// <summary>
        /// Delete action to remove user and their role association
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> DeleteUser([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<UserViewModel> userViewModels)
        {
            try
            {
                var userViewModel = userViewModels.FirstOrDefault();
                if (userViewModel != null)
                {
                    // Delete user and role association from the database
                    await DeleteUserAsync(userViewModel.UserId);
                }

                return Json(userViewModels.ToDataSourceResult(request, ModelState));
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return StatusCode(500, "An error occurred while deleting the user.");
            }
        }

        /// <summary>
        /// Optional Cancel action
        /// </summary>
        [HttpPost]
        public IActionResult Cancel([DataSourceRequest] DataSourceRequest request)
        {
            // Perform any necessary cleanup or logging
            return Json(new { message = "Operation canceled" });
        }

        // Helper methods for Dapper database operations
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
