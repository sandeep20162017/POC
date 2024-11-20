@section scripts {
    <script>
        // Handle changes in the CheckBoxGroup and update the grid data
        function onSiteChange(e) {
            var selectedValues = $(e.sender.element).val(); // Get selected site IDs
            var grid = $("#userGrid").data("kendoGrid");
            var currentRow = $(e.sender.element).closest("tr");
            var dataItem = grid.dataItem(currentRow);

            // Update the grid dataItem with selected site IDs and names
            dataItem.set("SiteId", selectedValues);
        }

        // Pre-populate selected sites in edit mode
        function onEdit(e) {
            if (e.model.SiteId) {
                var checkBoxGroup = e.container.find("[name='SiteId']").data("kendoCheckBoxGroup");
                if (checkBoxGroup) {
                    checkBoxGroup.value(e.model.SiteId); // Pre-select sites
                }
            }
        }
    </script>
}
