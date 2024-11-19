using Dapper;
using System.Data.SqlClient;

public class SiteService
{
    private readonly string _connectionString;

    public SiteService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public int InsertSite(string siteName)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var sql = "INSERT INTO SCES.Sites (SiteName) VALUES (@SiteName); SELECT SCOPE_IDENTITY();";
            var siteId = connection.QuerySingle<int>(sql, new { SiteName = siteName });
            return siteId;
        }
    }
}