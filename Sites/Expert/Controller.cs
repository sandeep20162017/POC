using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCES.Models.Admin;

public class UserManagementController : Controller
{
    private readonly string _connectionString;

    public UserManagementController(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IActionResult> Index()
    {
        // Fetch users and sites
        var users = await GetUsersWithSitesAsync();
        var sites = await GetSitesAsync();

        // Pass sites to the editor template via ViewData
        ViewData["Sites"] = sites;

        return View(users);
    }

    private async Task<IEnumerable<UserViewModel>> GetUsersWithSitesAsync()
    {
        const string query = @"
            SELECT 
                u.UserID, 
                u.UserName, 
                u.RoleID, 
                r.RoleName, 
                s.SiteID, 
                s.SiteName
            FROM 
                SCES.Users u
            LEFT JOIN 
                SCES.Roles r ON u.RoleID = r.RoleID
            LEFT JOIN 
                SCES.UserSites us ON u.UserID = us.UserID
            LEFT JOIN 
                SCES.Sites s ON us.SiteID = s.SiteID
            ORDER BY 
                u.UserID";

        using (var connection = new SqlConnection(_connectionString))
        {
            var userDictionary = new Dictionary<int, UserViewModel>();

            var result = await connection.QueryAsync<UserViewModel, Site, UserViewModel>(
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
                splitOn: "SiteID"
            );

            return userDictionary.Values;
        }
    }

    private async Task<IEnumerable<Site>> GetSitesAsync()
    {
        const string query = "SELECT SiteID, SiteName FROM SCES.Sites";

        using (var connection = new SqlConnection(_connectionString))
        {
            return await connection.QueryAsync<Site>(query);
        }
    }
}
