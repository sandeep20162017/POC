using Dapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;

public class UserManagementGridController : Controller
{
    private readonly DapperContext _db;

    // Constructor to inject DapperContext
    public UserManagementGridController(DapperContext dapper)
    {
        _db = dapper; // DapperContext for database access
    }

    /// <summary>
    /// Reads user views for the Telerik Kendo Grid, including associated roles.
    /// </summary>
    /// <param name="request">Telerik's DataSourceRequest object.</param>
    /// <returns>JSON result with user views formatted for the grid.</returns>
    public async Task<IActionResult> UserViewRead([DataSourceRequest] DataSourceRequest request)
    {
        try
        {
            // Fetch the list of user views
            var userViews = await GetUserViews();

            // Return the result in a format that Telerik Grid can consume
            var dataSourceResult = userViews.ToDataSourceResult(request);

            return Json(dataSourceResult);
        }
        catch (Exception ex)
        {
            // Handle and log exception (Consider injecting ILogger to log the error)
            return StatusCode(500, "An error occurred while fetching the data.");
        }
    }

    /// <summary>
    /// Retrieves user views by joining the Users and Roles tables.
    /// </summary>
    /// <returns>A list of UserViewModel objects.</returns>
    public async Task<IEnumerable<UserViewModel>> GetUserViews()
    {
        // SQL query to retrieve user and role information
        var query = @"
            SELECT u.UserId, u.UserName, r.RoleId, r.RoleName
            FROM BCES.Users u
            INNER JOIN BCES.UserRoles ur ON ur.UserId = u.UserId
            INNER JOIN BCES.Roles r ON r.RoleId = ur.RoleId";

        try
        {
            using (var connection = _db.CreateConnection())
            {
                // Query the database and manually map the result to UserViewModel and RoleModel
                var userViews = await connection.QueryAsync<UserViewModel, RoleModel, UserViewModel>(
                    query,
                    (user, role) =>
                    {
                        // Assign RoleModel to UserViewModel
                        user.RoleModel = role;
                        return user;
                    },
                    splitOn: "RoleId" // Tells Dapper to split the result between UserViewModel and RoleModel at RoleId
                );

                return userViews.ToList(); // Return as a list of UserViewModel
            }
        }
        catch (Exception ex)
        {
            // Handle and log any exceptions that occur during the query
            throw new Exception("Error fetching user views from the database.", ex);
        }
    }

    /// <summary>
    /// Adds a new user and assigns them a role.
    /// </summary>
    /// <param name="userViewModel">The model containing user and role data.</param>
    /// <returns>JSON result indicating success or failure.</returns>
    [HttpPost]
    public async Task<IActionResult> UserViewAdd([FromBody] UserViewModel userViewModel)
    {
        // SQL queries for inserting user and role
        var insertUserQuery = "INSERT INTO BCES.Users (UserName) VALUES (@UserName); SELECT CAST(SCOPE_IDENTITY() as int);";
        var insertUserRoleQuery = "INSERT INTO BCES.UserRoles (UserId, RoleId) VALUES (@UserId, @RoleId);";

        try
        {
            using (var connection = _db.CreateConnection())
            {
                // Begin transaction
                using (var transaction = connection.BeginTransaction())
                {
                    // Insert user and retrieve the new UserId
                    var newUserId = await connection.ExecuteScalarAsync<int>(insertUserQuery, new { userViewModel.UserName }, transaction);

                    // Insert user-role association
                    await connection.ExecuteAsync(insertUserRoleQuery, new { UserId = newUserId, RoleId = userViewModel.RoleModel.RoleId }, transaction);

                    // Commit the transaction
                    transaction.Commit();

                    return Json(new { success = true, message = "User added successfully." });
                }
            }
        }
        catch (Exception ex)
        {
            // Handle and log any exceptions
            return StatusCode(500, "An error occurred while adding the user.");
        }
    }

    /// <summary>
    /// Updates an existing user and their associated role.
    /// </summary>
    /// <param name="userViewModel">The model containing updated user and role data.</param>
    /// <returns>JSON result indicating success or failure.</returns>
    [HttpPost]
    public async Task<IActionResult> UserViewUpdate([FromBody] UserViewModel userViewModel)
    {
        // SQL queries for updating user and role association
        var updateUserQuery = "UPDATE BCES.Users SET UserName = @UserName WHERE UserId = @UserId;";
        var updateUserRoleQuery = "UPDATE BCES.UserRoles SET RoleId = @RoleId WHERE UserId = @UserId;";

        try
        {
            using (var connection = _db.CreateConnection())
            {
                // Begin transaction
                using (var transaction = connection.BeginTransaction())
                {
                    // Update user information
                    await connection.ExecuteAsync(updateUserQuery, new { userViewModel.UserName, userViewModel.UserId }, transaction);

                    // Update user-role association
                    await connection.ExecuteAsync(updateUserRoleQuery, new { RoleId = userViewModel.RoleModel.RoleId, UserId = userViewModel.UserId }, transaction);

                    // Commit the transaction
                    transaction.Commit();

                    return Json(new { success = true, message = "User updated successfully." });
                }
            }
        }
        catch (Exception ex)
        {
            // Handle and log any exceptions
            return StatusCode(500, "An error occurred while updating the user.");
        }
    }

    /// <summary>
    /// Deletes an existing user and their associated role.
    /// </summary>
    /// <param name="userId">The ID of the user to be deleted.</param>
    /// <returns>JSON result indicating success or failure.</returns>
    [HttpPost]
    public async Task<IActionResult> UserViewDelete(int userId)
    {
        // SQL queries for deleting user and role associations
        var deleteUserRoleQuery = "DELETE FROM BCES.UserRoles WHERE UserId = @UserId;";
        var deleteUserQuery = "DELETE FROM BCES.Users WHERE UserId = @UserId;";

        try
        {
            using (var connection = _db.CreateConnection())
            {
                // Begin transaction
                using (var transaction = connection.BeginTransaction())
                {
                    // First delete user-role association
                    await connection.ExecuteAsync(deleteUserRoleQuery, new { UserId = userId }, transaction);

                    // Then delete the user
                    await connection.ExecuteAsync(deleteUserQuery, new { UserId = userId }, transaction);

                    // Commit the transaction
                    transaction.Commit();

                    return Json(new { success = true, message = "User deleted successfully." });
                }
            }
        }
        catch (Exception ex)
        {
            // Handle and log any exceptions
            return StatusCode(500, "An error occurred while deleting the user.");
        }
    }
}
