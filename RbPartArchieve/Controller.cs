private async Task<IEnumerable<RebuiltPartsViewModel>> GetRebuiltPartsData()
{
    try
    {
        var query = @"
            SELECT 
                rbm.RbMasterlistId,
                rbm.[RebuiltStockNum],
                rbm.[MmsStockCode],
                rbm.[Keyword],
                rbm.JobNumber,
                rbm.[CorePartNum],
                rbm.[DetailedDesc],
                rbm.[CoreCharge],
                rbm.[EstimatedCost],
                rbm.[BuyNewCost],
                rbm.[RemanCost],
                rbm.[ExternalCost],
                lb.ListId,
                lb.Description AS Description,
                ld.LabourType,
                ld.CC,
                ld.Task,
                ld.Usage,
                ld.Time,
                ld.RatePerHour,
                ld.TimeAddition,
                ld.WrenchTime,
                ld.DateRevised,
                ld.LabourHours
            FROM [SBCES].[RbMasterlist] rbm
            INNER JOIN [SBCES].[RbListOfBuses] rlb ON rlb.[RebuiltStockNum] = rbm.[RebuiltStockNum]
            INNER JOIN [SBCES].[ListOfBuses] lb ON rlb.ListId = lb.ListId
            LEFT JOIN [SBCES].[LabourDetails] ld ON ld.RebuiltStockNum = rbm.RebuiltStockNum";

        var rbParts = _dbConnection.Query<RebuiltPartsViewModel, ListOfBusesModel, LabourDetailsRebuiltPartsViewModel, RebuiltPartsViewModel>(
            query,
            (rbPart, vehicle, labourDetail) =>
            {
                rbPart.VehicleSeries.Add(vehicle);
                if (labourDetail != null)
                {
                    rbPart.LabourDetails.Add(labourDetail);
                }
                return rbPart;
            },
            splitOn: "ListId,LabourType"
        ).GroupBy(rbPart => rbPart.RbMasterlistId).Select(g =>
        {
            var groupedRbPart = g.First();
            groupedRbPart.VehicleSeries = g.SelectMany(rbPart => rbPart.VehicleSeries).Distinct().ToList();
            groupedRbPart.LabourDetails = g.SelectMany(rbPart => rbPart.LabourDetails).Distinct().ToList();
            return groupedRbPart;
        }).ToList();

        return rbParts;
    }
    catch (Exception ex)
    {
        return null;
    }
}
