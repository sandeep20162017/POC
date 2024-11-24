<script>
    function onRequestStart(e) {
        if (e.type === "update") {
            const selectedSites = document.getElementById("SelectedSitesHidden").value;

            // Add the hidden field value to the request data
            if (e.data) {
                e.data.SelectedSites = selectedSites;
            }
        }
    }
</script>
