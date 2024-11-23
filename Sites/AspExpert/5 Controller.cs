[HttpGet]
public IActionResult GetUsers()
{
    // Fetch aggregated data with the SQL query
    var users = db.Query<UserViewModel>(
        @"SELECT u.UserId, u.UserName, u.RoleId, r.RoleName,
                 STRING_AGG(CAST(us.SiteId AS VARCHAR), ',') AS SiteIdsString
          FROM SCES.[User] u
          INNER JOIN SCES.[Role] r ON u.RoleId = r.RoleId
          LEFT JOIN SCES.UserSite us ON u.UserId = us.UserId
          GROUP BY u.UserId, u.UserName, u.RoleId, r.RoleName").ToList();

    // Parse SiteIdsString into SiteIds
    foreach (var user in users)
    {
        user.SiteIds = string.IsNullOrEmpty(user.SiteIdsString)
            ? new List<int>()
            : user.SiteIdsString.Split(',').Select(int.Parse).ToList();
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
