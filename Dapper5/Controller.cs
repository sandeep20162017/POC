@model IEnumerable<BCES.Models.Admin.UserViewModel>

@{
    ViewData["Title"] = "User Management";
}

<h2>User Management</h2>

@* Button to add a new row for creating a user *@
<button type="button" class="btn btn-primary" id="addNewUserButton">Add New User</button>

@(Html.Kendo().Grid<BCES.Models.Admin.UserViewModel>()
    .Name("userGrid")
    .Columns(columns =>
    {
        columns.Bound(u => u.UserName).Title("User Name");
        columns.Bound(u => u.RoleModel.RoleName)
            .Title("Role Name")
            .EditorTemplateName("RoleNameDropDown"); // Uses the custom dropdown template for role selection

        columns.Command(command =>
        {
            command.Edit(); // Adds an "Edit" button to enable inline editing, which includes built-in "Save" and "Cancel" actions
            command.Destroy(); // Delete button, which triggers the destroy controller action
        }).Title("Actions");
    })
    .ToolBar(toolbar => toolbar.Create()) // Toolbar button to add a new row
    .Editable(editable => editable.Mode(GridEditMode.InLine)) // Sets grid to use inline editing mode
    .Pageable()
    .Sortable()
    .Scrollable()
    .DataSource(dataSource => dataSource
        .Ajax()
        .Read(read => read.Url(Url.Action("ReadUsers", "UserManagementGrid")).Type(HttpVerbs.Get))
        .Create(create => create.Url(Url.Action("CreateUser", "UserManagementGrid")).Type(HttpVerbs.Post))
        .Update(update => update.Url(Url.Action("UpdateUser", "UserManagementGrid")).Type(HttpVerbs.Post))
        .Destroy(delete => delete.Url(Url.Action("DeleteUser", "UserManagementGrid")).Type(HttpVerbs.Post))
        .Model(model =>
        {
            model.Id(u => u.UserId);
            model.Field(u => u.UserName).Editable(true);
            model.Field(u => u.RoleModel.RoleName).Editable(true);
        })
    )
)

@section Scripts {
    <script>
        $("#addNewUserButton").click(function () {
            var grid = $("#userGrid").data("kendoGrid");
            grid.addRow(); // Triggers the grid's inline row creation functionality
        });
    </script>
}
