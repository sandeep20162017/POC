using Microsoft.AspNetCore.Mvc;
using BCES.Models.Admin;
using Dapper;
using System.Data;

namespace BCES.Controllers.Admin
{
    [Route("Admin/[controller]")]
    public class UserManagementGridController : Controller
    {
        private readonly IDbConnection _dbConnection;

        public UserManagementGridController(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        #region Read Users
        [HttpGet("ReadUsers")]
        public IActionResult ReadUsers()
        {
            try
            {
                // SQL query to fetch users and their roles from UserView
                var sql = @"
                    SELECT u.UserId, u.UserName, r.RoleId, r.RoleName
                    FROM BCES.Users u
                    LEFT JOIN BCES.UserRoles ur ON u.UserId = ur.UserId
                    LEFT JOIN BCES.Roles r ON ur.RoleId = r.RoleId";

                // Fetch users with roles and map to UserViewModel with nested RoleModel
                var userDictionary = new Dictionary<int, UserViewModel>();

                var users = _dbConnection.Query<UserViewModel, RoleModel, UserViewModel>(
                    sql,
                    (user, role) =>
                    {
                        if (!userDictionary.TryGetValue(user.UserId, out var userEntry))
                        {
                            userEntry = user;
                            userEntry.RoleModel = role; // Assign RoleModel to the user
                            userDictionary.Add(userEntry.UserId, userEntry);
                        }
                        return userEntry;
                    },
                    splitOn: "RoleId"
                );

                return Json(userDictionary.Values);
            }
            catch (Exception ex)
            {
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
                // Insert user into Users table and retrieve UserId
                var insertUserSql = "INSERT INTO BCES.Users (UserName) VALUES (@UserName); SELECT CAST(SCOPE_IDENTITY() as int)";
                var userId = _dbConnection.ExecuteScalar<int>(insertUserSql, new { user.UserName });

                // Insert role association in UserRoles table if RoleModel exists
                if (user.RoleModel?.RoleId > 0)
                {
                    var insertRoleSql = "INSERT INTO BCES.UserRoles (UserId, RoleId) VALUES (@UserId, @RoleId)";
                    _dbConnection.Execute(insertRoleSql, new { UserId = userId, RoleId = user.RoleModel.RoleId });
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
                // Update user in Users table
                var updateUserSql = "UPDATE BCES.Users SET UserName = @UserName WHERE UserId = @UserId";
                _dbConnection.Execute(updateUserSql, new { user.UserName, user.UserId });

                // Update or insert role association in UserRoles table
                if (user.RoleModel?.RoleId > 0)
                {
                    var roleQuery = @"
                        IF EXISTS (SELECT 1 FROM BCES.UserRoles WHERE UserId = @UserId)
                            UPDATE BCES.UserRoles SET RoleId = @RoleId WHERE UserId = @UserId
                        ELSE
                            INSERT INTO BCES.UserRoles (UserId, RoleId) VALUES (@UserId, @RoleId)";
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
                // Remove role associations from UserRoles
                _dbConnection.Execute("DELETE FROM BCES.UserRoles WHERE UserId = @UserId", new { UserId = id });

                // Delete user from Users table
                _dbConnection.Execute("DELETE FROM BCES.Users WHERE UserId = @UserId", new { UserId = id });

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
                // SQL query to fetch all roles
                var roles = _dbConnection.Query<RoleModel>("SELECT RoleId, RoleName FROM BCES.Roles").ToList();
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
