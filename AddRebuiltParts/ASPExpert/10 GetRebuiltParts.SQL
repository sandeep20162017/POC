SELECT
    rb.RebuiltStockNum AS RebuiltStockCode,
    rb.DetailedDesc AS Description,
    rb.Keyword AS Keywords,
    rb.JobNumber,
    rb.BuyNewCost AS MMSBuyCode,
    rb.CorePartNum AS CoreCode,
    rb.SOPNumber,
    rb.BuyNewCost AS BuyCost,
    rb.RemanCost AS RemanufacturedCode,
    ISNULL(
        STRING_AGG(CAST(rblb.ListId AS VARCHAR), ','),
        ''
    ) AS ListOfVehicles
FROM
    BCES.RbMasterlist rb
LEFT JOIN
    SCES.RbListOfBuses rblb ON rb.RebuiltStockNum = rblb.RebuiltStockNum
GROUP BY
    rb.RebuiltStockNum,
    rb.DetailedDesc,
    rb.Keyword,
    rb.JobNumber,
    rb.BuyNewCost,
    rb.CorePartNum,
    rb.SOPNumber,
    rb.RemanCost;