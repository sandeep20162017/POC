[HttpGet]
public IActionResult GetUsers()
{
    var users = db.Query<UserViewModel>(
        @"SELECT u.UserId, u.UserName, u.RoleId, r.RoleName,
            (SELECT SiteId FROM UserSite WHERE UserId = u.UserId) AS SiteIds
          FROM User u
          INNER JOIN Role r ON u.RoleId = r.RoleId").ToList();

    var roles = db.Query<RoleModel>("SELECT RoleId, RoleName FROM Role").ToList();
    var sites = db.Query<SiteModel>("SELECT SiteId, SiteName FROM Site").ToList();

    ViewData["Roles"] = roles;
    ViewData["Sites"] = sites;

    return Json(users);
}

[HttpPost]
public IActionResult UpdateUser([FromBody] UserViewModel model)
{
    // Update User table
    db.Execute("UPDATE User SET UserName = @UserName, RoleId = @RoleId WHERE UserId = @UserId",
        new { model.UserName, model.RoleId, model.UserId });

    // Update UserSite table
    db.Execute("DELETE FROM UserSite WHERE UserId = @UserId", new { model.UserId }); // Clear existing entries
    foreach (var siteId in model.SiteIds)
    {
        db.Execute("INSERT INTO UserSite (UserId, SiteId) VALUES (@UserId, @SiteId)",
            new { model.UserId, SiteId = siteId });
    }

    return Json(new { success = true });
}
