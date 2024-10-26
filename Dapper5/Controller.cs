using Microsoft.AspNetCore.Mvc;
using Dapper;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using BCES.Models.Admin;

namespace BCES.Controllers.Admin
{
    [Route("Admin/[controller]")]
    public class UserManagementController : Controller
    {
        private readonly DapperContext _db;

        // Constructor to inject DapperContext
        public UserManagementController(DapperContext dapper)
        {
            _db = dapper;
        }

        #region Index Action

        // Index action to return the main view and load roles
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                // Retrieve all roles to populate dropdown
                string roleSql = "SELECT RoleId, RoleName FROM Roles";
                var roles = _db.Connection.Query<RoleModel>(roleSql).ToList();
                
                // Store roles in ViewData for the dropdown control
                ViewData["Roles"] = roles;

                return View(); // Return the main grid view
            }
            catch (Exception ex)
            {
                // Log the exception if necessary
                return BadRequest("Error loading roles.");
            }
        }

        #endregion

        #region CRUD Operations

        [HttpGet("GetUsers")]
        public IActionResult GetUsers()
        {
            try
            {
                // Query to get users along with their roles
                string sql = @"SELECT u.UserId, u.UserName, r.RoleId, r.RoleName
                               FROM Users u
                               LEFT JOIN Roles r ON u.RoleId = r.RoleId";
                
                var users = _db.Connection.Query<UserViewModel, RoleModel, UserViewModel>(
                    sql,
                    (user, role) =>
                    {
                        user.RoleModel = role;
                        return user;
                    },
                    splitOn: "RoleId"
                ).ToList();
                
                return Json(users);
            }
            catch (Exception ex)
            {
                // Log the exception if needed
                return BadRequest("Error fetching users.");
            }
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser([FromBody] UserViewModel user)
        {
            try
            {
                // SQL to insert a new user and return the new UserId
                string sql = @"INSERT INTO Users (UserName, RoleId) VALUES (@UserName, @RoleId);
                               SELECT CAST(SCOPE_IDENTITY() as int);";

                // Execute the query and set the UserId
                user.UserId = _db.Connection.ExecuteScalar<int>(sql, new { user.UserName, user.RoleModel.RoleId });
                return Json(user);
            }
            catch (Exception ex)
            {
                // Log the exception
                return BadRequest("Error adding user.");
            }
        }

        [HttpPost("UpdateUser")]
        public IActionResult UpdateUser([FromBody] UserViewModel user)
        {
            try
            {
                // SQL to update user details
                string sql = @"UPDATE Users SET UserName = @UserName, RoleId = @RoleId WHERE UserId = @UserId";
                _db.Connection.Execute(sql, new { user.UserName, user.RoleModel.RoleId, user.UserId });
                return Json(user);
            }
            catch (Exception ex)
            {
                // Log the exception
                return BadRequest("Error updating user.");
            }
        }

        [HttpPost("DeleteUser")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                // SQL to delete a user by UserId
                string sql = @"DELETE FROM Users WHERE UserId = @UserId";
                _db.Connection.Execute(sql, new { UserId = id });
                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception
                return BadRequest("Error deleting user.");
            }
        }

        #endregion
    }
}
