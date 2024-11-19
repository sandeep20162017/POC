public class UserSiteService
{
    private readonly string _connectionString;

    public UserSiteService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public void InsertUserSite(int userId, int siteId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var sql = "INSERT INTO SCES.UserSites (UserID, SiteID) VALUES (@UserID, @SiteID);";
            connection.Execute(sql, new { UserID = userId, SiteID = siteId });
        }
    }
}