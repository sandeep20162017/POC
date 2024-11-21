public List<SiteModel> GetSitesForUser(int userId)
{
    // Fetch sites for the user, marking those assigned as IsSelected = true
    var sites = _siteRepository.GetAllSites();
    var userSites = _siteRepository.GetUserSiteIds(userId);

    foreach (var site in sites)
    {
        site.IsSelected = userSites.Contains(site.SiteId);
    }

    return sites;
}
