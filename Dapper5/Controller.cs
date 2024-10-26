using Microsoft.AspNetCore.Mvc;
using BCES.Models.Admin;
using Dapper;
using System.Data;

namespace BCES.Controllers.Admin
{
    [Route("Admin/[controller]")]
    public class UserManagementController : Controller
    {
        private readonly IDbConnection _dbConnection;

        // Constructor injecting the database connection
        public UserManagementController(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        #region Read Users
        [HttpGet("ReadUsers")]
        public IActionResult ReadUsers()
        {
            try
            {
                // SQL query to retrieve user details and roles
                var users = _dbConnection.Query<UserViewModel>(@"
                    SELECT u.UserId, u.UserName, r.RoleId, r.RoleName
                    FROM UserModel u
                    LEFT JOIN UserRoleModel ur ON u.UserId = ur.UserId
                    LEFT JOIN RoleModel r ON ur.RoleId = r.RoleId");

                return Json(users);
            }
            catch (Exception ex)
            {
                // Return a 500 error with the exception message
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region Create User
        [HttpPost("CreateUser")]
        public IActionResult CreateUser([FromBody] UserViewModel user)
        {
            if (user == null)
                return BadRequest("User data is null.");

            try
            {
                // Insert new user and get the generated UserId
                var query = "INSERT INTO UserModel (UserName) VALUES (@UserName); SELECT CAST(SCOPE_IDENTITY() as int)";
                var userId = _dbConnection.ExecuteScalar<int>(query, new { user.UserName });

                // Insert role association if RoleModel is provided
                if (user.RoleModel != null)
                {
                    _dbConnection.Execute("INSERT INTO UserRoleModel (UserId, RoleId) VALUES (@UserId, @RoleId)",
                        new { UserId = userId, RoleId = user.RoleModel.RoleId });
                }

                return Json(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region Update User
        [HttpPost("UpdateUser")]
        public IActionResult UpdateUser([FromBody] UserViewModel user)
        {
            if (user == null)
                return BadRequest("User data is null.");

            try
            {
                // Update user information
                var query = "UPDATE UserModel SET UserName = @UserName WHERE UserId = @UserId";
                _dbConnection.Execute(query, user);

                // Update or insert role association based on existence
                if (user.RoleModel != null)
                {
                    var roleQuery = @"
                        IF EXISTS (SELECT 1 FROM UserRoleModel WHERE UserId = @UserId)
                            UPDATE UserRoleModel SET RoleId = @RoleId WHERE UserId = @UserId
                        ELSE
                            INSERT INTO UserRoleModel (UserId, RoleId) VALUES (@UserId, @RoleId)";
                    _dbConnection.Execute(roleQuery, new { UserId = user.UserId, RoleId = user.RoleModel.RoleId });
                }

                return Json(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region Delete User
        [HttpPost("DeleteUser")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                // Delete role association and user
                _dbConnection.Execute("DELETE FROM UserRoleModel WHERE UserId = @UserId", new { UserId = id });
                _dbConnection.Execute("DELETE FROM UserModel WHERE UserId = @UserId", new { UserId = id });

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion

        #region Get Roles for Dropdown
        [HttpGet("GetRoles")]
        public IActionResult GetRoles()
        {
            try
            {
                // Retrieve the list of roles for the dropdown
                var roles = _dbConnection.Query<RoleModel>("SELECT RoleId, RoleName FROM RoleModel").ToList();
                ViewData["Roles"] = roles; // Stores roles in ViewData for use in the dropdown

                return Json(roles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        #endregion
    }
}
