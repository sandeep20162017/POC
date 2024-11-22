using BCES.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Dapper;
using System.Collections.Generic;

namespace BCES.Admin.Controllers
{
    public class UsersController : Controller
    {
        private readonly IDbConnection _dbConnection;

        public UsersController(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public IActionResult Index()
        {
            ViewBag.Roles = GetRoles();
            ViewBag.Sites = GetSites();
            return View();
        }

        [HttpGet]
        public IActionResult Read([DataSourceRequest] DataSourceRequest request)
        {
            try
            {
                var users = _dbConnection.Query<UserViewModel, SiteModel, UserViewModel>(
                    "SELECT u.UserId, u.UserName, r.RoleId, r.RoleName, s.SiteId, s.SiteName " +
                    "FROM Users u " +
                    "JOIN Roles r ON u.RoleId = r.RoleId " +
                    "LEFT JOIN UserSites us ON u.UserId = us.UserId " +
                    "LEFT JOIN Sites s ON us.SiteId = s.SiteId",
                    (user, site) =>
                    {
                        if (user.SelectedSites == null)
                            user.SelectedSites = new List<string>().ToArray();
                        if (site != null)
                            user.SelectedSites = user.SelectedSites.Concat(new[] { site.SiteId.ToString() }).ToArray();
                        return user;
                    },
                    splitOn: "SiteId"
                ).Distinct();

                return Json(users.ToDataSourceResult(request));
            }
            catch (Exception ex)
            {
                // Log exception
                ModelState.AddModelError("", "An error occurred while fetching users.");
                return BadRequest(ModelState);
            }
        }

        [HttpPost]
        public IActionResult Create(UserViewModel model)
        {
            try
            {
                var userId = _dbConnection.QuerySingle<int>(
                    "INSERT INTO Users (UserName, RoleId) VALUES (@UserName, @SelectedRole); SELECT SCOPE_IDENTITY();",
                    new { model.UserName, model.SelectedRole }
                );

                foreach (var siteId in model.SelectedSites.Select(int.Parse))
                {
                    _dbConnection.Execute(
                        "INSERT INTO UserSites (UserId, SiteId) VALUES (@UserId, @SiteId);",
                        new { UserId = userId, SiteId = siteId }
                    );
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log exception
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Update(UserViewModel model)
        {
            try
            {
                _dbConnection.Execute(
                    "UPDATE Users SET UserName = @UserName, RoleId = @SelectedRole WHERE UserId = @UserId;",
                    new { model.UserName, model.SelectedRole, model.UserId }
                );

                _dbConnection.Execute(
                    "DELETE FROM UserSites WHERE UserId = @UserId;",
                    new { model.UserId }
                );

                foreach (var siteId in model.SelectedSites.Select(int.Parse))
                {
                    _dbConnection.Execute(
                        "INSERT INTO UserSites (UserId, SiteId) VALUES (@UserId, @SiteId);",
                        new { UserId = model.UserId, SiteId = siteId }
                    );
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log exception
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Destroy(int userId)
        {
            try
            {
                _dbConnection.Execute(
                    "DELETE FROM UserSites WHERE UserId = @UserId;",
                    new { UserId = userId }
                );

                _dbConnection.Execute(
                    "DELETE FROM Users WHERE UserId = @UserId;",
                    new { UserId = userId }
                );

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                // Log exception
                return Json(new { success = false, error = ex.Message });
            }
        }

        private List<RoleModel> GetRoles()
        {
            return _dbConnection.Query<RoleModel>("SELECT RoleId, RoleName FROM Roles").ToList();
        }

        private List<SiteModel> GetSites()
        {
            return _dbConnection.Query<SiteModel>("SELECT SiteId, SiteName FROM Sites").ToList();
        }
    }
}