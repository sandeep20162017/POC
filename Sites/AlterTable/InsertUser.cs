public class UserService
{
    private readonly string _connectionString;

    public UserService(string connectionString)
    {
        _connectionString = connectionString;
    }

    public int InsertUser(UserViewModel userViewModel)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            connection.Open();
            var sql = @"
                INSERT INTO SCES.Users (UserName, UserEmail, UserContactNo, Location, RoleID)
                VALUES (@UserName, @UserEmail, @UserContactNo, @Location, @RoleID);
                SELECT SCOPE_IDENTITY();";
            var userId = connection.QuerySingle<int>(sql, new
            {
                UserName = userViewModel.UserName,
                UserEmail = userViewModel.UserEmail,
                UserContactNo = userViewModel.UserContactNo,
                Location = userViewModel.Location,
                RoleID = userViewModel.RoleName // Assuming RoleName is mapped to RoleID
            });
            return userId;
        }
    }
}