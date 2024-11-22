using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

public class YourController : Controller
{
    public IActionResult SaveToSession(IEnumerable<SiteModel> models)
    {
        // Save the models to session using the utility class
        HttpContext.Session.SetList("YourSessionKey", models);

        return RedirectToAction("Index");
    }

    public IActionResult RetrieveFromSession()
    {
        // Retrieve the models from session using the utility class
        var models = HttpContext.Session.GetList<SiteModel>("YourSessionKey");

        // Use the models as needed
        return View(models);
    }
}