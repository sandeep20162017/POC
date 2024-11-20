using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

public class UserService
{
    private readonly string _connectionString;

    public UserService(string connectionString)
    {
        _connectionString = connectionString;
    }

    /// <summary>
    /// Retrieves all users, their roles, and associated sites.
    /// </summary>
    /// <returns>List of UserViewModel objects.</returns>
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

        try
        {
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
        catch (Exception ex)
        {
            throw new Exception("Error retrieving users with roles and sites.", ex);
        }
    }

    /// <summary>
    /// Adds a new user and associates it with sites.
    /// </summary>
    /// <param name="username">The username of the user.</param>
    /// <param name="roleId">The role ID of the user.</param>
    /// <param name="siteIds">List of site IDs to associate with the user.</param>
    /// <returns>The ID of the newly created user.</returns>
    public async Task<int> AddUserAsync(string username, int roleId, List<int> siteIds)
    {
        const string insertUserQuery = @"
            INSERT INTO SCES.Users (UserName, RoleId) 
            VALUES (@UserName, @RoleId);
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        const string insertUserSitesQuery = @"
            INSERT INTO SCES.UserSites (UserId, SiteId) 
            VALUES (@UserId, @SiteId);";

        try
        {
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
        catch (Exception ex)
        {
            throw new Exception("Error adding a new user.", ex);
        }
    }

    /// <summary>
    /// Updates an existing user and reassigns their sites.
    /// </summary>
    /// <param name="userId">The ID of the user to update.</param>
    /// <param name="username">The new username.</param>
    /// <param name="roleId">The new role ID.</param>
    /// <param name="siteIds">List of new site IDs to associate with the user.</param>
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

        try
        {
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
        catch (Exception ex)
        {
            throw new Exception("Error updating the user.", ex);
        }
    }

    /// <summary>
    /// Deletes a user and their associated site mappings.
    /// </summary>
    /// <param name="userId">The ID of the user to delete.</param>
    public async Task DeleteUserAsync(int userId)
    {
        const string deleteUserSitesQuery = @"
            DELETE FROM SCES.UserSites 
            WHERE UserId = @UserId;";

        const string deleteUserQuery = @"
            DELETE FROM SCES.Users 
            WHERE UserId = @UserId;";

        try
        {
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
        catch (Exception ex)
        {
            throw new Exception("Error deleting the user.", ex);
        }
    }
}
