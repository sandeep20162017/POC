[HttpGet]
public IActionResult GetUsers()
{
    // Fetch aggregated data with SiteIds and SiteNames
    var users = db.Query<UserViewModel>(
        @"SELECT u.UserId, u.UserName, u.RoleId, r.RoleName,
                 STRING_AGG(CAST(us.SiteId AS VARCHAR), ',') AS SiteIds,
                 STRING_AGG(s.SiteName, ',') AS SiteNames
          FROM SCES.[User] u
          INNER JOIN SCES.[Role] r ON u.RoleId = r.RoleId
          LEFT JOIN SCES.UserSite us ON u.UserId = us.UserId
          LEFT JOIN SCES.Site s ON us.SiteId = s.SiteId
          GROUP BY u.UserId, u.UserName, u.RoleId, r.RoleName").ToList();

    // Map SiteIds and SiteNames to a usable format
    foreach (var user in users)
    {
        user.SiteIdsList = string.IsNullOrEmpty(user.SiteIds)
            ? new List<int>()
            : user.SiteIds.Split(',').Select(int.Parse).ToList();

        user.SiteNamesList = string.IsNullOrEmpty(user.SiteNames)
            ? new List<string>()
            : user.SiteNames.Split(',').ToList();
    }

    // Fetch roles and sites for dropdowns
    var roles = db.Query<RoleModel>("SELECT RoleId, RoleName FROM SCES.[Role]").ToList();
    var sites = db.Query<SiteModel>("SELECT SiteId, SiteName FROM SCES.Site").ToList();

    // Pass dropdown data to the view
    ViewData["Roles"] = roles;
    ViewData["Sites"] = sites;

    // Return JSON result for the grid
    return Json(users);
}
