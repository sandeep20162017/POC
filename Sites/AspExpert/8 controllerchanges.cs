[HttpPost]
public IActionResult GridUpdate(YourModel model, string SelectedSites)
{
    // SelectedSites will contain "2,3"
    var siteIds = SelectedSites.Split(',').Select(int.Parse).ToList();

    // Process the model and the selected sites
    return Json(new { success = true });
}
