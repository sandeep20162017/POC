[HttpGet]
public IActionResult GetUsers()
{
    // Fetch aggregated data using the SQL query
    var users = db.Query<dynamic>(
        @"SELECT u.UserId, u.UserName, u.RoleId, r.RoleName,
                 STRING_AGG(CAST(us.SiteId AS VARCHAR), ',') AS SiteIds
          FROM SCES.[User] u
          INNER JOIN SCES.[Role] r ON u.RoleId = r.RoleId
          LEFT JOIN SCES.UserSite us ON u.UserId = us.UserId
          GROUP BY u.UserId, u.UserName, u.RoleId, r.RoleName").ToList();

    // Map the raw data to UserViewModel
    var userModels = users.Select(u => new UserViewModel
    {
        UserId = u.UserId,
        UserName = u.UserName,
        RoleId = u.RoleId,
        RoleName = u.RoleName,
        // Parse SiteIds from comma-separated string to List<int>
        SiteIds = string.IsNullOrEmpty(u.SiteIds)
            ? new List<int>()
            : u.SiteIds.Split(',').Select(int.Parse).ToList()
    }).ToList();

    // Fetch roles and sites for dropdowns
    var roles = db.Query<RoleModel>("SELECT RoleId, RoleName FROM SCES.[Role]").ToList();
    var sites = db.Query<SiteModel>("SELECT SiteId, SiteName FROM SCES.Site").ToList();

    // Pass dropdown data to the view
    ViewData["Roles"] = roles;
    ViewData["Sites"] = sites;

    // Return JSON result for the grid
    return Json(userModels);
}
