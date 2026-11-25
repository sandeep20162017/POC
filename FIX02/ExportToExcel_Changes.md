# Export to Excel - all changes (11 CHANGE blocks, 7 files)

Pages affected: Vehicles, RebuiltParts, MakeVsBuy (the only views that render the four grid partials).
No Model, Controller or DB change. Client-side Kendo Excel export (jszip is already loaded in _Layout.cshtml:53-54).

Result:
- "Export to Excel" button in the toolbar of the NSC, SC, RB and Labour grids (all roles, incl. read-only/archived).
- Button disabled when the grid is empty.
- Exports all pages. File name: <estimate number>_<Section>_<yyyy-MM-dd>.xlsx  e.g. MBH0161_StockCodedParts_2026-11-25.xlsx

Apply order does not matter. Anchors are verbatim from your files (CRLF line endings, line numbers as of the files I read).

======================================================================
## A. PARTIALS (toolbar button + wiring) - 8 changes
======================================================================

### CHANGE 1: View - Views/Shared/_NscPartsUsed.cshtml
OP: MODIFY
ANCHOR (verbatim, line 182):
<<<
    .Events(e => e.Edit("onNscPartsEdit"))
>>>
CODE:
<<<
    .Events(e => e.Edit("onNscPartsEdit").DataBound("onGridExcelDataBound").ExcelExport("onGridExcelExport"))
>>>
WHY: Wires the disable-when-empty and file-name handlers to the grid.

### CHANGE 2: View - Views/Shared/_NscPartsUsed.cshtml
OP: MODIFY
ANCHOR (verbatim, lines 161-168):
<<<
     .ToolBar(toolbar =>
    {
        if (!isArchived && isAdmin) //ArchiveDisable
        {
            toolbar.Create();
           // toolbar.Save();
        }
    })
>>>
CODE:
<<<
     .ToolBar(toolbar =>
    {
        if (!isArchived && isAdmin) //ArchiveDisable
        {
            toolbar.Create();
           // toolbar.Save();
        }
        toolbar.Excel().Text("Export to Excel"); // all roles; disabled when the grid is empty
    })
    .Excel(excel => excel.FileName("NonStockCodedParts.xlsx").AllPages(true))
>>>
WHY: Adds the Export to Excel button and exports all pages.

----------------------------------------------------------------------

### CHANGE 3: View - Views/Shared/_ScPartsUsed.cshtml
OP: MODIFY
ANCHOR (verbatim, line 241):
<<<
    .Events(e => e.Edit("onScPartsEdit"))
>>>
CODE:
<<<
    .Events(e => e.Edit("onScPartsEdit").DataBound("onGridExcelDataBound").ExcelExport("onGridExcelExport"))
>>>
WHY: Wires the disable-when-empty and file-name handlers to the grid.

### CHANGE 4: View - Views/Shared/_ScPartsUsed.cshtml
OP: MODIFY
ANCHOR (verbatim, lines 217-224):
<<<
     .ToolBar(toolbar =>
    {
        if (!isArchived && isAdmin) //ArchiveDisable
        {
            toolbar.Create();
            //  toolbar.Save();
        }
    })
>>>
CODE:
<<<
     .ToolBar(toolbar =>
    {
        if (!isArchived && isAdmin) //ArchiveDisable
        {
            toolbar.Create();
            //  toolbar.Save();
        }
        toolbar.Excel().Text("Export to Excel"); // all roles; disabled when the grid is empty
    })
    .Excel(excel => excel.FileName("StockCodedParts.xlsx").AllPages(true))
>>>
WHY: Adds the Export to Excel button and exports all pages.

----------------------------------------------------------------------

### CHANGE 5: View - Views/Shared/_RbPartsUsed.cshtml
OP: MODIFY
ANCHOR (verbatim, line 27):
<<<
    .Events(e => e.Edit("onRbPartsEdit"))
>>>
CODE:
<<<
    .Events(e => e.Edit("onRbPartsEdit").DataBound("onGridExcelDataBound").ExcelExport("onGridExcelExport"))
>>>
WHY: Wires the disable-when-empty and file-name handlers to the grid.

### CHANGE 6: View - Views/Shared/_RbPartsUsed.cshtml
OP: MODIFY
ANCHOR (verbatim, lines 126-133):
<<<
    .ToolBar(toolbar =>
    {
        if (!isArchived && isAdmin) //ArchiveDisable
        {
            toolbar.Create();
           // toolbar.Save();
        }
    })
>>>
CODE:
<<<
    .ToolBar(toolbar =>
    {
        if (!isArchived && isAdmin) //ArchiveDisable
        {
            toolbar.Create();
           // toolbar.Save();
        }
        toolbar.Excel().Text("Export to Excel"); // all roles; disabled when the grid is empty
    })
    .Excel(excel => excel.FileName("RebuiltParts.xlsx").AllPages(true))
>>>
WHY: Adds the Export to Excel button and exports all pages.

----------------------------------------------------------------------

### CHANGE 7: View - Views/Shared/_LabourDetails.cshtml
OP: MODIFY
ANCHOR (verbatim, line 113):
<<<
                .Events(e => e.Edit("onLabourEdit"))
>>>
CODE:
<<<
                .Events(e => e.Edit("onLabourEdit").DataBound("onGridExcelDataBound").ExcelExport("onGridExcelExport"))
>>>
WHY: Wires the disable-when-empty and file-name handlers to the grid.

### CHANGE 8: View - Views/Shared/_LabourDetails.cshtml
OP: MODIFY
ANCHOR (verbatim, lines 92-99):
<<<
                .ToolBar(toolbar =>
                {
                    if (!isArchived && isAdmin) //ArchiveDisable
                    {
                        toolbar.Create();
                        @* toolbar.Save(); *@
                    }
                })
>>>
CODE:
<<<
                .ToolBar(toolbar =>
                {
                    if (!isArchived && isAdmin) //ArchiveDisable
                    {
                        toolbar.Create();
                        @* toolbar.Save(); *@
                    }
                    toolbar.Excel().Text("Export to Excel"); // all roles; disabled when the grid is empty
                })
                .Excel(excel => excel.FileName("LabourDetails.xlsx").AllPages(true))
>>>
WHY: Adds the Export to Excel button and exports all pages.

======================================================================
## B. INDEX PAGES (shared JS, once per page) - 3 changes
======================================================================
The CODE is identical in all three. It goes INSIDE an existing <script> block (no <script> tags),
directly before `function onNscPartsEdit(e) {`. The last line of CODE repeats the anchor line so that
function stays intact. The anchor line is unique in each file and is not inside a Razor comment.

### CHANGE 9: JS - Views/Vehicles/Index.cshtml
OP: ADD_BEFORE
ANCHOR (verbatim, line 2397, 12-space indent):
<<<
            function onNscPartsEdit(e) {
>>>
CODE:
<<<
// Export to Excel: disabled on an empty grid; file = <estimate number>_<section>_<yyyy-MM-dd>.xlsx
function onGridExcelDataBound(e) {
    var empty = e.sender.dataSource.total() === 0;
    e.sender.wrapper.find(".k-grid-excel")
        .toggleClass("k-state-disabled", empty)
        .attr("aria-disabled", empty ? "true" : "false")
        .css("pointer-events", empty ? "none" : "");
}

function onGridExcelExport(e) {
    if (e.sender.dataSource.total() === 0) { e.preventDefault(); return; }
    var sections = {
        NscPartsMaterialDetailsRebuiltPartsGrid: "NonStockCodedParts",
        ScPartsMaterialDetailsRebuiltPartsGrid: "StockCodedParts",
        RbPartsMaterialDetailsRebuiltPartsGrid: "RebuiltParts",
        LabourDetailsGrid: "LabourDetails"
    };
    var id = e.sender.element.attr("id") || "";
    var sep = id.indexOf("_");
    var section = sections[sep > -1 ? id.substring(0, sep) : id] || "Grid";
    var part = (sep > -1 ? id.substring(sep + 1) : "").replace(/[^A-Za-z0-9_-]/g, "_");
    var name = (part ? part + "_" : "") + section + "_" + kendo.toString(new Date(), "yyyy-MM-dd") + ".xlsx";
    e.workbook.fileName = name;
    e.sender.options.excel.fileName = name;
}

            function onNscPartsEdit(e) {
>>>
WHY: Defines the two Excel handlers once for the Vehicles page.

### CHANGE 10: JS - Views/Parts/RebuiltParts/Index.cshtml
OP: ADD_BEFORE
ANCHOR (verbatim, line 1897, 12-space indent):
<<<
            function onNscPartsEdit(e) {
>>>
CODE:
<<<
// Export to Excel: disabled on an empty grid; file = <estimate number>_<section>_<yyyy-MM-dd>.xlsx
function onGridExcelDataBound(e) {
    var empty = e.sender.dataSource.total() === 0;
    e.sender.wrapper.find(".k-grid-excel")
        .toggleClass("k-state-disabled", empty)
        .attr("aria-disabled", empty ? "true" : "false")
        .css("pointer-events", empty ? "none" : "");
}

function onGridExcelExport(e) {
    if (e.sender.dataSource.total() === 0) { e.preventDefault(); return; }
    var sections = {
        NscPartsMaterialDetailsRebuiltPartsGrid: "NonStockCodedParts",
        ScPartsMaterialDetailsRebuiltPartsGrid: "StockCodedParts",
        RbPartsMaterialDetailsRebuiltPartsGrid: "RebuiltParts",
        LabourDetailsGrid: "LabourDetails"
    };
    var id = e.sender.element.attr("id") || "";
    var sep = id.indexOf("_");
    var section = sections[sep > -1 ? id.substring(0, sep) : id] || "Grid";
    var part = (sep > -1 ? id.substring(sep + 1) : "").replace(/[^A-Za-z0-9_-]/g, "_");
    var name = (part ? part + "_" : "") + section + "_" + kendo.toString(new Date(), "yyyy-MM-dd") + ".xlsx";
    e.workbook.fileName = name;
    e.sender.options.excel.fileName = name;
}

            function onNscPartsEdit(e) {
>>>
WHY: Defines the two Excel handlers once for the Rebuilt Parts page.

### CHANGE 11: JS - Views/MakeVsBuy/Index.cshtml
OP: ADD_BEFORE
ANCHOR (verbatim, line 2639, 8-space indent):
<<<
        function onNscPartsEdit(e) {
>>>
CODE:
<<<
// Export to Excel: disabled on an empty grid; file = <estimate number>_<section>_<yyyy-MM-dd>.xlsx
function onGridExcelDataBound(e) {
    var empty = e.sender.dataSource.total() === 0;
    e.sender.wrapper.find(".k-grid-excel")
        .toggleClass("k-state-disabled", empty)
        .attr("aria-disabled", empty ? "true" : "false")
        .css("pointer-events", empty ? "none" : "");
}

function onGridExcelExport(e) {
    if (e.sender.dataSource.total() === 0) { e.preventDefault(); return; }
    var sections = {
        NscPartsMaterialDetailsRebuiltPartsGrid: "NonStockCodedParts",
        ScPartsMaterialDetailsRebuiltPartsGrid: "StockCodedParts",
        RbPartsMaterialDetailsRebuiltPartsGrid: "RebuiltParts",
        LabourDetailsGrid: "LabourDetails"
    };
    var id = e.sender.element.attr("id") || "";
    var sep = id.indexOf("_");
    var section = sections[sep > -1 ? id.substring(0, sep) : id] || "Grid";
    var part = (sep > -1 ? id.substring(sep + 1) : "").replace(/[^A-Za-z0-9_-]/g, "_");
    var name = (part ? part + "_" : "") + section + "_" + kendo.toString(new Date(), "yyyy-MM-dd") + ".xlsx";
    e.workbook.fileName = name;
    e.sender.options.excel.fileName = name;
}

        function onNscPartsEdit(e) {
>>>
WHY: Defines the two Excel handlers once for the Make vs Buy page.

======================================================================
## TEST STEPS
======================================================================
1. Open a Vehicle estimate, a Make vs Buy estimate and a Rebuilt Part; each of the 4 grids shows "Export to Excel".
2. Empty grid: button greyed out and not clickable. Add a row: it enables without a page reload.
3. Grid with more than 5 rows: export contains every row; file named like MBH0161_NonStockCodedParts_2026-11-25.xlsx.
4. Read-only role: button visible and working.
5. If a button shows but the console says "onGridExcelDataBound is not defined", that page is missing CHANGE 9/10/11.

## RISKS
- Not run. If your Kendo build ignores e.workbook.fileName and options.excel.fileName inside excelExport, the file falls back to the static name (e.g. NonStockCodedParts.xlsx), without estimate number/date.
- Link columns (ClientTemplate) export their raw field value; the Delete column is not exported.
- "Empty" includes a filter that leaves zero rows.
- Anchors verified 11/11 against the staged copies (CRLF). If you already applied the earlier partial-script changes (old CHANGE 1, 4, 7, 10), delete those script blocks.
