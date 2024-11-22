[HttpPost]
public IActionResult Update(UserViewModel model)
{
    if (ModelState.IsValid)
    {
        // Update user details if needed
        // ...

        // Update user sites
        UpdateUserSites(model.UserId, model.SelectedSites);

        return RedirectToAction("Index");
    }

    return View(model);
}

private void UpdateUserSites(int userId, List<int> selectedSites)
{
    using (var connection = _connectionFactory.CreateConnection())
    {
        // Delete existing user sites
        connection.Execute("DELETE FROM UserSite WHERE UserId = @UserId", new { UserId = userId });

        // Insert new user sites
        if (selectedSites != null && selectedSites.Any())
        {
            var records = selectedSites.Select(siteId => new { UserId = userId, SiteId = siteId });
            connection.Insert(records);
        }
    }
}