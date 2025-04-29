function markRequiredColumns(grid) {
    var schema = grid.dataSource.options.schema.model.fields;
    grid.columns.forEach(function (col, idx) {
        if (schema[col.field] && schema[col.field].validation && schema[col.field].validation.required) {
            var th = grid.thead.find("th").eq(idx);
            th.addClass("required-cell");
        }
    });
}
