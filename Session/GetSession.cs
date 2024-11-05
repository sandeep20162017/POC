public class YourController : Controller
{
    public IActionResult AnotherAction()
    {
        // Retrieve a string variable from the session
        var userName = HttpContext.Session.GetString("UserName");

        // Retrieve an integer variable from the session
        var userId = HttpContext.Session.GetInt32("UserId");

        // Retrieve a complex object from the session (requires deserialization)
        var userJson = HttpContext.Session.GetString("UserObject");
        var user = JsonSerializer.Deserialize<User>(userJson);

        // Use the retrieved values as needed
        ViewBag.UserName = userName;
        ViewBag.UserId = userId;
        ViewBag.UserObject = user;

        return View();
    }
}