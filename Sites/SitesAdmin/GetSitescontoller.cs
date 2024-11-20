public IActionResult UserManagement()
{
    var sites = GetSites(); // Fetch sites from your database using Dapper or any other method
    ViewData["Sites"] = sites;
    return View();
}
