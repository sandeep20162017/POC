SELECT 
    CC, 
    SUM(ISNULL(TRY_CAST(ExtendedTime AS DECIMAL(18,2)), 0)) AS TotalExtendedTime, 
    @ArchId
FROM 
    SBCES.TempLabourList
GROUP BY 
    CC
