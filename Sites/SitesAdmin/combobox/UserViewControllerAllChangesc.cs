private async Task<IEnumerable<UserViewModel>> GetUsersWithRolesAsync()
{
    const string query = @"
        SELECT 
            u.UserID, 
            u.UserName, 
            u.UserEmail,
            u.UserContactNo,
            u.Location,
            r.RoleID, 
            r.RoleName, 
            s.SiteID, 
            s.SiteName
        FROM 
            sces.Users u
        LEFT JOIN 
            sces.Roles r ON u.RoleID = r.RoleID
        LEFT JOIN 
            sces.UserSites us ON u.UserID = us.UserID
        LEFT JOIN 
            sces.Sites s ON us.SiteID = s.SiteID
        ORDER BY 
            u.UserID, s.SiteName;";

    using (var connection = _dbConnection)
    {
        // A dictionary to hold users and group related sites
        var userDictionary = new Dictionary<int, UserViewModel>();

        var result = await connection.QueryAsync<UserViewModel, SiteModel, UserViewModel>(
            query,
            (user, site) =>
            {
                // Check if the user already exists in the dictionary
                if (!userDictionary.TryGetValue(user.UserID, out var userEntry))
                {
                    userEntry = user;
                    userEntry.SiteId = new List<int>();
                    userEntry.SiteNames = new List<string>();
                    userDictionary.Add(user.UserID, userEntry);
                }

                // Add site information if it's available
                if (site != null)
                {
                    if (!userEntry.SiteId.Contains(site.SiteId))
                    {
                        userEntry.SiteId.Add(site.SiteId);
                        userEntry.SiteNames.Add(site.SiteName);
                    }
                }

                return userEntry;
            },
            splitOn: "SiteID" // Tells Dapper where to split the mapping
        );

        // Return grouped results
        return userDictionary.Values;
    }
}
/////////////////
public async Task<IActionResult> UserViewAdd([DataSourceRequest] DataSourceRequest request, [FromForm] UserViewModel user)
{
    if (!ModelState.IsValid)
    {
        return Json(ModelState.ToDataSourceResult(request, ModelState));
    }

    try
    {
        var userId = await AddUserAsync(user.Username, user.RoleId); // Ensure AddUserAsync validates RoleId
        user.Id = userId; // Populate the user object with the new ID

        return Json(new[] { user }.ToDataSourceResult(request, ModelState));
    }
    catch (DbUpdateException dbEx) when (dbEx.InnerException?.Message.Contains("FK_") == true)
    {
        // Handle FK constraint violation
        ModelState.AddModelError(string.Empty, "Role ID is invalid or does not exist.");
        return Json(new[] { user }.ToDataSourceResult(request, ModelState));
    }
    catch (Exception ex)
    {
        // Handle general exceptions
        ModelState.AddModelError(string.Empty, "An error occurred while adding the user.");
        return Json(new[] { user }.ToDataSourceResult(request, ModelState));
    }
}
/////////////////
public async Task<IActionResult> EditUser([DataSourceRequest] DataSourceRequest request, [FromForm] UserViewModel user)
{
    if (!ModelState.IsValid)
    {
        return Json(ModelState.ToDataSourceResult(request, ModelState));
    }

    try
    {
        await UpdateUserAsync(user.Id, user.Username, user.RoleId); // Ensure this method updates FK properly
        return Json(new[] { user }.ToDataSourceResult(request, ModelState));
    }
    catch (DbUpdateException dbEx) when (dbEx.InnerException?.Message.Contains("FK_") == true)
    {
        ModelState.AddModelError(string.Empty, "Role ID is invalid or does not exist.");
        return Json(new[] { user }.ToDataSourceResult(request, ModelState));
    }
    catch (Exception ex)
    {
        ModelState.AddModelError(string.Empty, "An error occurred while editing the user.");
        return Json(new[] { user }.ToDataSourceResult(request, ModelState));
    }
}
/////////////////
public async Task<IActionResult> DeleteUser([DataSourceRequest] DataSourceRequest request, [FromForm] UserViewModel user)
{
    try
    {
        await DeleteUserAsync(user.Id); // Ensure this method handles related data cleanup
        return Json(new[] { user }.ToDataSourceResult(request, ModelState));
    }
    catch (DbUpdateException dbEx) when (dbEx.InnerException?.Message.Contains("FK_") == true)
    {
        ModelState.AddModelError(string.Empty, "Cannot delete the user. It may be referenced by other records.");
        return Json(new[] { user }.ToDataSourceResult(request, ModelState));
    }
    catch (Exception ex)
    {
        ModelState.AddModelError(string.Empty, "An error occurred while deleting the user.");
        return Json(new[] { user }.ToDataSourceResult(request, ModelState));
    }
}
///////////////

public async Task<List<UserViewModel>> GetUsersWithRolesAsync()
{
    const string query = @"
        SELECT 
            u.UserId, 
            u.UserName, 
            r.RoleId, 
            r.RoleName, 
            s.SiteId, 
            s.Sitename
        FROM SCES.Users u
        INNER JOIN SCES.Roles r ON u.RoleId = r.RoleId
        LEFT JOIN SCES.UserSites us ON u.UserId = us.UserId
        LEFT JOIN SCES.sites s ON us.SiteId = s.SiteId";

    using (var connection = new SqlConnection(_connectionString))
    {
        var userDictionary = new Dictionary<int, UserViewModel>();

        var users = await connection.QueryAsync<UserViewModel, Site, UserViewModel>(
            query,
            (user, site) =>
            {
                if (!userDictionary.TryGetValue(user.UserId, out var userEntry))
                {
                    userEntry = user;
                    userEntry.Sites = new List<Site>();
                    userDictionary.Add(user.UserId, userEntry);
                }

                if (site != null)
                {
                    userEntry.Sites.Add(site);
                }

                return userEntry;
            },
            splitOn: "SiteId"
        );

        return users.Distinct().ToList();
    }
}
////////////////
public async Task<int> AddUserAsync(string username, int roleId, List<int> siteIds)
{
    const string insertUserQuery = @"
        INSERT INTO SCES.Users (UserName, RoleId) 
        VALUES (@UserName, @RoleId);
        SELECT CAST(SCOPE_IDENTITY() AS INT);";

    const string insertUserSitesQuery = @"
        INSERT INTO SCES.UserSites (UserId, SiteId) 
        VALUES (@UserId, @SiteId);";

    using (var connection = new SqlConnection(_connectionString))
    {
        using (var transaction = connection.BeginTransaction())
        {
            try
            {
                // Insert user
                var userId = await connection.ExecuteScalarAsync<int>(
                    insertUserQuery,
                    new { UserName = username, RoleId = roleId },
                    transaction
                );

                // Insert associated sites
                foreach (var siteId in siteIds)
                {
                    await connection.ExecuteAsync(
                        insertUserSitesQuery,
                        new { UserId = userId, SiteId = siteId },
                        transaction
                    );
                }

                transaction.Commit();
                return userId;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
//////////////
public async Task UpdateUserAsync(int userId, string username, int roleId, List<int> siteIds)
{
    const string updateUserQuery = @"
        UPDATE SCES.Users 
        SET UserName = @UserName, RoleId = @RoleId 
        WHERE UserId = @UserId;";

    const string deleteUserSitesQuery = @"
        DELETE FROM SCES.UserSites 
        WHERE UserId = @UserId;";

    const string insertUserSitesQuery = @"
        INSERT INTO SCES.UserSites (UserId, SiteId) 
        VALUES (@UserId, @SiteId);";

    using (var connection = new SqlConnection(_connectionString))
    {
        using (var transaction = connection.BeginTransaction())
        {
            try
            {
                // Update user details
                await connection.ExecuteAsync(
                    updateUserQuery,
                    new { UserId = userId, UserName = username, RoleId = roleId },
                    transaction
                );

                // Clear and reassign sites
                await connection.ExecuteAsync(deleteUserSitesQuery, new { UserId = userId }, transaction);
                foreach (var siteId in siteIds)
                {
                    await connection.ExecuteAsync(insertUserSitesQuery, new { UserId = userId, SiteId = siteId }, transaction);
                }

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
///////////////////
public async Task DeleteUserAsync(int userId)
{
    const string deleteUserSitesQuery = @"
        DELETE FROM SCES.UserSites 
        WHERE UserId = @UserId;";

    const string deleteUserQuery = @"
        DELETE FROM SCES.Users 
        WHERE UserId = @UserId;";

    using (var connection = new SqlConnection(_connectionString))
    {
        using (var transaction = connection.BeginTransaction())
        {
            try
            {
                // Delete user-site associations
                await connection.ExecuteAsync(deleteUserSitesQuery, new { UserId = userId }, transaction);

                // Delete user
                await connection.ExecuteAsync(deleteUserQuery, new { UserId = userId }, transaction);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
/////////////////

public class UserViewModel
{
    public int UserId { get; set; }
    public string UserName { get; set; }
    public int RoleId { get; set; }
    public string RoleName { get; set; }
    public List<Site> Sites { get; set; } = new();
}

public class Site
{
    public int SiteId { get; set; }
    public string Sitename { get; set; }
}
