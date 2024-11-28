SELECT
    rl.RebuiltStockNum AS RebuiltStockCode,
    rl.DetailedDesc AS Description,
    rl.Keyword AS Keywords,
    rl.JobNumber,
    rl.MmsStockCode AS MMSBuyCode,
    rl.CorePartNum AS CoreCode,
    rl.SopNumber,
    rl.BuyNewCost AS BuyCost,
    rl.RemanCost AS RemanufacturedCode,
    STRING_AGG(vl.VehicleSeriesCode, ', ') WITHIN GROUP (ORDER BY vl.VehicleSeriesCode) AS ListOfVehicles
FROM
    BCES.RbMasterlist rl
LEFT JOIN
    BCES.VehicleSeries vs ON rl.RbMasterlistId = vs.RbMasterlistId
LEFT JOIN
    BCES.VehicleList vl ON vs.VehicleSeriesId = vl.VehicleSeriesId
GROUP BY
    rl.RbMasterlistId, rl.RebuiltStockNum, rl.DetailedDesc, rl.Keyword, rl.JobNumber, rl.MmsStockCode, rl.CorePartNum, rl.SopNumber, rl.BuyNewCost, rl.RemanCost;