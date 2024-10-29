using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

public class RoleMiddleware
{
    private readonly RequestDelegate _next;
    private readonly DapperHelper _dapperHelper;

    public RoleMiddleware(RequestDelegate next, DapperHelper dapperHelper)
    {
        _next = next;
        _dapperHelper = dapperHelper;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity.IsAuthenticated)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var role = await _dapperHelper.QuerySingleOrDefaultAsync<Role>(
                "SELECT r.RoleId, r.RoleName FROM Users u JOIN Roles r ON u.RoleId = r.RoleId WHERE u.UserId = @UserId",
                new { UserId = userId });

            if (role != null)
            {
                context.Items["RoleId"] = role.RoleId;
                context.Items["RoleName"] = role.RoleName;
            }
        }

        await _next(context);
    }
}

public static class RoleMiddlewareExtensions
{
    public static IApplicationBuilder UseRoleMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RoleMiddleware>();
    }
}