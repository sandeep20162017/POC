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
