@(Html.Kendo().Grid<UserViewModel>()
    .Name("UserGrid")
    .Editable(editable => editable.Mode(GridEditMode.InCell)) // In-cell editing for better UX
    .ToolBar(toolbar => toolbar.Save()) // Adds save button to submit data
    .Columns(columns =>
    {
        columns.Bound(u => u.UserName).Title("User Name");
        columns.Bound(u => u.RoleId)
            .Title("Role")
            .EditorTemplateName("RoleDropDown"); // Custom dropdown editor
        columns.Bound(u => u.SiteIds)
            .Title("Sites")
            .EditorTemplateName("SitesMultiSelect"); // Custom multiselect editor
        columns.Command(command => command.Edit()).Width(200);
    })
    .DataSource(dataSource => dataSource
        .Ajax()
        .Model(model =>
        {
            model.Id(u => u.UserId); // Primary key
            model.Field(u => u.SiteIds).DefaultValue(new List<int>()); // Initialize site list
        })
        .Read(read => read.Action("GetUsers", "User"))
        .Update(update => update.Action("UpdateUser", "User"))
    )
)
