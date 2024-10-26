using Dapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;

public class UserManagementGridController : Controller
{
    private readonly DapperContext _db;

    public UserManagementGridController(DapperContext dapper)
    {
        _db = dapper;
    }

    /// <summary>
    /// Retrieves user views for the grid.
    /// </summary>
    public async Task<IActionResult> UserViewRead([DataSourceRequest] DataSourceRequest request)
    {
        try
        {
            var userViews = await GetUserViews();
            return Json(userViews.ToDataSourceResult(request));
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while fetching data: " + ex.Message);
        }
    }

    /// <summary>
    /// Adds a new user and assigns a role.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> UserViewAdd([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<UserViewModel> userViewModels)
    {
        try
        {
            var userViewModel = userViewModels?.FirstOrDefault();
            if (userViewModel != null)
            {
                userViewModel.RoleModel = userViewModel.RoleModel ?? new RoleModel();
                var userId = await AddUserAsync(userViewModel.UserName, userViewModel.RoleModel.RoleId);
                userViewModel.UserId = userId;
            }

            return Json(userViewModels.ToDataSourceResult(request, ModelState));
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while adding the user: " + ex.Message);
        }
    }

    /// <summary>
    /// Updates an existing user and their role.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> UserViewUpdate([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<UserViewModel> userViewModels)
    {
        try
        {
            var userViewModel = userViewModels?.FirstOrDefault();
            if (userViewModel != null)
            {
                userViewModel.RoleModel = userViewModel.RoleModel ?? new RoleModel();
                await UpdateUserAsync(userViewModel.UserId, userViewModel.UserName, userViewModel.RoleModel.RoleId);
            }

            return Json(userViewModels.ToDataSourceResult(request, ModelState));
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while updating the user: " + ex.Message);
        }
    }

    /// <summary>
    /// Deletes a user and their role association.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> UserViewDelete([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")] IEnumerable<UserViewModel> userViewModels)
    {
        try
        {
            var userViewModel = userViewModels?.FirstOrDefault();
            if (userViewModel != null)
            {
                await DeleteUserAsync(userViewModel.UserId);
            }

            return Json(userViewModels.ToDataSourceResult(request, ModelState));
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while deleting the user: " + ex.Message);
        }
    }

    /// <summary>
    /// Renders the index view.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        try
        {
            var roles = await GetRolesAsync();
            ViewData["Roles"] = roles;
            return View("~/Views/Admin/Index.cshtml");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while fetching roles: " + ex.Message);
        }
    }

    #region Database Methods

    private async Task<IEnumerable<UserViewModel>> GetUserViews()
    {
        var query = @"
            SELECT u.UserId, u.UserName, r.RoleId, r.RoleName
            FROM BCES.Users u
            INNER JOIN BCES.UserRoles ur ON ur.UserId = u.UserId
            INNER JOIN BCES.Roles r ON r.RoleId = ur.RoleId";

        using (var connection = _db.CreateConnection())
        {
            var userViews = await connection.QueryAsync<UserViewModel, RoleModel, UserViewModel>(
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
    }

    private async Task<IEnumerable<RoleModel>> GetRolesAsync()
    {
        var query = "SELECT RoleId, RoleName FROM BCES.Roles";
        using (var connection = _db.CreateConnection())
        {
            return await connection.QueryAsync<RoleModel>(query);
        }
    }

    private async Task<int> AddUserAsync(string userName, int roleId)
    {
        var insertUserQuery = "INSERT INTO BCES.Users (UserName) VALUES (@UserName); SELECT CAST(SCOPE_IDENTITY() as int);";
        var insertUserRoleQuery = "INSERT INTO BCES.UserRoles (UserId, RoleId) VALUES (@UserId, @RoleId);";

        using (var connection = _db.CreateConnection())
        {
            using (var transaction = connection.BeginTransaction())
            {
                var newUserId = await connection.ExecuteScalarAsync<int>(insertUserQuery, new { UserName = userName }, transaction);
                await connection.ExecuteAsync(insertUserRoleQuery, new { UserId = newUserId, RoleId = roleId }, transaction);
                transaction.Commit();
                return newUserId;
            }
        }
    }

    private async Task UpdateUserAsync(int userId, string userName, int roleId)
    {
        var updateUserQuery = "UPDATE BCES.Users SET UserName = @UserName WHERE UserId = @UserId;";
        var updateUserRoleQuery = "UPDATE BCES.UserRoles SET RoleId = @RoleId WHERE UserId = @UserId;";

        using (var connection = _db.CreateConnection())
        {
            using (var transaction = connection.BeginTransaction())
            {
                await connection.ExecuteAsync(updateUserQuery, new { UserName = userName, UserId = userId }, transaction);
                await connection.ExecuteAsync(updateUserRoleQuery, new { RoleId = roleId, UserId = userId }, transaction);
                transaction.Commit();
            }
        }
    }

    private async Task DeleteUserAsync(int userId)
    {
        var deleteUserRoleQuery = "DELETE FROM BCES.UserRoles WHERE UserId = @UserId;";
        var deleteUserQuery = "DELETE FROM BCES.Users WHERE UserId = @UserId;";

        using (var connection = _db.CreateConnection())
        {
            using (var transaction = connection.BeginTransaction())
            {
                await connection.ExecuteAsync(deleteUserRoleQuery, new { UserId = userId }, transaction);
                await connection.ExecuteAsync(deleteUserQuery, new { UserId = userId }, transaction);
                transaction.Commit();
            }
        }
    }

    #endregion
}
