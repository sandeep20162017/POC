function onSiteChange() {
    var selectedSites = this.value();
    // Set the hidden field's value to the selected site IDs as a comma-separated string
    $("#SelectedSitesHidden").val(selectedSites.join(","));
}