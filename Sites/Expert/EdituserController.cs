public IActionResult EditUser(int userId)
{
    var user = _userService.GetUserById(userId);
    user.Sites = GetSitesForUser(userId);
    return View(user);
}
