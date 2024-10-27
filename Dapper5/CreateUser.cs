#region Create User
[HttpPost("CreateUser")]
public IActionResult CreateUser([FromBody] UserViewModel user)
{
    if (user == null)
        return BadRequest("User data is null.");

    try
    {
        // Ensure RoleModel is initialized
        user.RoleModel ??= new RoleModel();

        // Insert user into Users table and retrieve UserId
        var insertUserSql = "INSERT INTO BCES.Users (UserName) VALUES (@UserName); SELECT CAST(SCOPE_IDENTITY() as int)";
        var userId = _dbConnection.ExecuteScalar<int>(insertUserSql, new { user.UserName });

        // Insert role association in UserRoles table if RoleModel exists and RoleId is valid
        if (user.RoleModel?.RoleId > 0)
        {
            var insertRoleSql = "INSERT INTO BCES.UserRoles (UserId, RoleId) VALUES (@UserId, @RoleId)";
            _dbConnection.Execute(insertRoleSql, new { UserId = userId, RoleId = user.RoleModel.RoleId });
        }

        // Return the user with the newly generated UserId
        user.UserId = userId;
        return Json(new { Data = user }); // Return data in a format that Kendo Grid expects
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Internal server error: {ex.Message}");
    }
}
#endregion