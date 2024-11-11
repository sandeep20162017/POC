using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

public class BaseController : Controller
{
    private readonly ApplicationDbContext _context;

    public BaseController(ApplicationDbContext context)
    {
        _context = context;
    }

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var username = Environment.UserName; // Get the username from the environment
        var userRole = HttpContext.Session.GetString("UserRole");

        if (userRole == null)
        {
            userRole = FetchUserRoleAsync(username).Result;
            HttpContext.Session.SetString("UserRole", userRole);
        }

        ViewBag.UserRole = userRole;

        base.OnActionExecuting(context);
    }

    private async Task<string> FetchUserRoleAsync(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        return user?.Role ?? "Unknown";
    }
}