using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        var roleId = HttpContext.Session.GetString("RoleId");
        ViewBag.RoleId = roleId;
        return View();
    }
}