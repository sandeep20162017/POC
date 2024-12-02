function openEstimates(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var rebuiltNumber = dataItem.RebuiltNumber;
    $.ajax({
        url: '@Url.Action("GetRebuiltPartEstimates", "RebuiltParts")',
        type: 'GET',
        data: { rebuiltNumber: rebuiltNumber },
        success: function(result) {
            $('#estimates-container').html(result);
        },
        error: function(xhr, status, error) {
            console.error('An error occurred: ' + error);
        }
    });
}