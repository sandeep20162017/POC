[HttpPost]
[AllowAnonymous]
public async Task<IActionResult> AddRebuiltPartsView([DataSourceRequest] DataSourceRequest request, [FromForm] RebuiltPartsViewModel rebuiltPartsViewModel)
{
    try
    {
        if (rebuiltPartsViewModel == null)
        {
            return BadRequest("Invalid data.");
        }

        // Ensure ListOfBus and ListOfBusIds are initialized
        rebuiltPartsViewModel.ListOfBus = rebuiltPartsViewModel.ListOfBus ?? new List<BusesModel>();
        rebuiltPartsViewModel.ListOfBusIds = rebuiltPartsViewModel.ListOfBusIds ?? new List<int?>();

        // Insert the rebuilt part into the database
        var insertPartSql = @"
            INSERT INTO SBCES.RbMasterlist 
            (
                RebuiltStockNum, MmsStockCode, Keyword, JobNumber, CorePartNum, 
                DetailedDesc, CoreCharge, EstimatedCost, BuyNewCost, RemanCost, 
                ExternalCost, LastModifiedBy, DateEntered, IsActive
            )
            VALUES 
            (
                @RebuiltStockNum, @MmsStockCode, @Keyword, @JobNumber, @CorePartNum, 
                @DetailedDesc, @CoreCharge, @EstimatedCost, @BuyNewCost, @RemanCost, 
                @ExternalCost, @LastModifiedBy, GETDATE(), @IsActive
            )";

        await _dbConnection.ExecuteAsync(insertPartSql, new
        {
            rebuiltPartsViewModel.RebuiltStockNum,
            rebuiltPartsViewModel.MmsStockCode,
            rebuiltPartsViewModel.Keyword,
            rebuiltPartsViewModel.JobNumber,
            rebuiltPartsViewModel.CorePartNum,
            rebuiltPartsViewModel.DetailedDesc,
            rebuiltPartsViewModel.CoreCharge,
            rebuiltPartsViewModel.EstimatedCost,
            rebuiltPartsViewModel.BuyNewCost,
            rebuiltPartsViewModel.RemanCost,
            ExternalCost = "", // Add to model if needed
            LastModifiedBy = User.Identity.Name, // Assuming you have user context
            IsActive = rebuiltPartsViewModel.IsActive ? 1 : 0
        });

        // Insert associated buses
        if (rebuiltPartsViewModel.ListOfBusIds?.Any() == true)
        {
            var insertBusesSql = @"
                INSERT INTO SBCES.RbListOfBuses (RebuiltStockNum, ListId)
                VALUES (@RebuiltStockNum, @ListId)";

            foreach (var listId in rebuiltPartsViewModel.ListOfBusIds.Where(id => id.HasValue))
            {
                await _dbConnection.ExecuteAsync(insertBusesSql, new
                {
                    rebuiltPartsViewModel.RebuiltStockNum,
                    ListId = listId.Value
                });
            }
        }

        return Json(new { success = true });
    }
    catch (Exception ex)
    {
        return StatusCode(500, $"Internal server error: {ex.Message}");
    }
}
