using Microsoft.AspNetCore.Mvc;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using BCES.Models.Admin;

namespace BCES.Controllers.Admin
{
    [Route("Admin/[controller]")]
    public class UserManagementController : Controller
    {
        private readonly IDbConnection _db;

        public UserManagementController(IDbConnection db)
        {
            _db = db;
        }

        #region CRUD Operations

        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            try
            {
                var sql = @"SELECT u.UserId, u.UserName, r.RoleId, r.RoleName
                            FROM Users u
                            LEFT JOIN Roles r ON u.RoleId = r.RoleId";
                var users = _db.Query<UserViewModel, RoleModel, UserViewModel>(
                    sql,
                    (user, role) =>
                    {
                        user.RoleModel = role;
                        return user;
                    },
                    splitOn: "RoleId"
                );
                return Json(users);
            }
            catch (Exception ex)
            {
                // Log exception
                return BadRequest("Error fetching users.");
            }
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser([FromBody] UserViewModel user)
        {
            try
            {
                var sql = @"INSERT INTO Users (UserName, RoleId) VALUES (@UserName, @RoleId);
                            SELECT CAST(SCOPE_IDENTITY() as int);";
                user.UserId = _db.ExecuteScalar<int>(sql, new { user.UserName, user.RoleModel.RoleId });
                return Json(user);
            }
            catch (Exception ex)
            {
                return BadRequest("Error adding user.");
            }
        }

        [HttpPost("UpdateUser")]
        public IActionResult UpdateUser([FromBody] UserViewModel user)
        {
            try
            {
                var sql = @"UPDATE Users SET UserName = @UserName, RoleId = @RoleId WHERE UserId = @UserId";
                _db.Execute(sql, new { user.UserName, user.RoleModel.RoleId, user.UserId });
                return Json(user);
            }
            catch (Exception ex)
            {
                return BadRequest("Error updating user.");
            }
        }

        [HttpPost("DeleteUser")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var sql = @"DELETE FROM Users WHERE UserId = @UserId";
                _db.Execute(sql, new { UserId = id });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest("Error deleting user.");
            }
        }

        #endregion
    }
}
