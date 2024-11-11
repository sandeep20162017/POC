USE SBCES; -- Replace with your actual database name
GO

-- Disable all foreign key constraints
DECLARE @DisableFkSQL NVARCHAR(MAX)
DECLARE @TableName NVARCHAR(255)
DECLARE @ConstraintName NVARCHAR(255)

DECLARE DisableFkCursor CURSOR FOR
SELECT 
    OBJECT_NAME(parent_object_id) AS TableName,
    name AS ConstraintName
FROM 
    sys.foreign_keys
WHERE 
    referenced_object_id = OBJECT_ID('SBCES.VehicleList')
    OR referenced_object_id = OBJECT_ID('SBCES.Transmissions')
    OR referenced_object_id = OBJECT_ID('SBCES.SOPNumber')
    OR referenced_object_id = OBJECT_ID('SBCES.RbMasterlist')
    OR referenced_object_id = OBJECT_ID('SBCES.MbListOfBuses')
    OR referenced_object_id = OBJECT_ID('SBCES.LabourTaskDescriptions')
    OR referenced_object_id = OBJECT_ID('SBCES.KitsUsed')
    OR referenced_object_id = OBJECT_ID('SBCES.KitsMasterlist')
    OR referenced_object_id = OBJECT_ID('SBCES.Engines')
    OR referenced_object_id = OBJECT_ID('SBCES.Differentials')
    OR referenced_object_id = OBJECT_ID('SBCES.CostCenters')
    OR referenced_object_id = OBJECT_ID('SBCES.CESSettings')

OPEN DisableFkCursor
FETCH NEXT FROM DisableFkCursor INTO @TableName, @ConstraintName

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @DisableFkSQL = 'ALTER TABLE SBCES.' + QUOTENAME(@TableName) + ' NOCHECK CONSTRAINT ' + QUOTENAME(@ConstraintName)
    EXEC sp_executesql @DisableFkSQL
    FETCH NEXT FROM DisableFkCursor INTO @TableName, @ConstraintName
END

CLOSE DisableFkCursor
DEALLOCATE DisableFkCursor
GO

-- Drop tables in the SBCES schema
DECLARE @TableName NVARCHAR(255)
DECLARE @SQL NVARCHAR(MAX)

DECLARE TableCursor CURSOR FOR
SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'SBCES'
ORDER BY TABLE_NAME DESC; -- Order by table name to handle dependencies

OPEN TableCursor
FETCH NEXT FROM TableCursor INTO @TableName

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @SQL = 'DROP TABLE SBCES.' + QUOTENAME(@TableName)
    EXEC sp_executesql @SQL
    FETCH NEXT FROM TableCursor INTO @TableName
END

CLOSE TableCursor
DEALLOCATE TableCursor
GO

-- Re-enable all foreign key constraints
DECLARE @EnableFkSQL NVARCHAR(MAX)
DECLARE @TableName NVARCHAR(255)
DECLARE @ConstraintName NVARCHAR(255)

DECLARE EnableFkCursor CURSOR FOR
SELECT 
    OBJECT_NAME(parent_object_id) AS TableName,
    name AS ConstraintName
FROM 
    sys.foreign_keys
WHERE 
    referenced_object_id = OBJECT_ID('SBCES.VehicleList')
    OR referenced_object_id = OBJECT_ID('SBCES.Transmissions')
    OR referenced_object_id = OBJECT_ID('SBCES.SOPNumber')
    OR referenced_object_id = OBJECT_ID('SBCES.RbMasterlist')
    OR referenced_object_id = OBJECT_ID('SBCES.MbListOfBuses')
    OR referenced_object_id = OBJECT_ID('SBCES.LabourTaskDescriptions')
    OR referenced_object_id = OBJECT_ID('SBCES.KitsUsed')
    OR referenced_object_id = OBJECT_ID('SBCES.KitsMasterlist')
    OR referenced_object_id = OBJECT_ID('SBCES.Engines')
    OR referenced_object_id = OBJECT_ID('SBCES.Differentials')
    OR referenced_object_id = OBJECT_ID('SBCES.CostCenters')
    OR referenced_object_id = OBJECT_ID('SBCES.CESSettings')

OPEN EnableFkCursor
FETCH NEXT FROM EnableFkCursor INTO @TableName, @ConstraintName

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @EnableFkSQL = 'ALTER TABLE SBCES.' + QUOTENAME(@TableName) + ' WITH CHECK CHECK CONSTRAINT ' + QUOTENAME(@ConstraintName)
    EXEC sp_executesql @EnableFkSQL
    FETCH NEXT FROM EnableFkCursor INTO @TableName, @ConstraintName
END

CLOSE EnableFkCursor
DEALLOCATE EnableFkCursor
GO