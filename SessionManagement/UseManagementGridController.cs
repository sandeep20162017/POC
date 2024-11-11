using Microsoft.AspNetCore.Http;

public class UserManagementGridController : Controller
{
    private readonly DapperContext _db;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserManagementGridController(DapperContext dapper, IHttpContextAccessor httpContextAccessor)
    {
        _db = dapper;
        _httpContextAccessor = httpContextAccessor;

        // Access the session variable
        var roleId = _httpContextAccessor.HttpContext.Session.GetString("RoleId");
        ViewBag.RoleId = roleId;
    }

    // Other actions
}