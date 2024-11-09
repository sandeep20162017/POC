DECLARE @TableName NVARCHAR(128)
DECLARE @SQL NVARCHAR(MAX)

DECLARE TableCursor CURSOR FOR
SELECT TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = 'SBCES'
  AND TABLE_TYPE = 'BASE TABLE'

OPEN TableCursor

FETCH NEXT FROM TableCursor INTO @TableName

WHILE @@FETCH_STATUS = 0
BEGIN
    -- Check and add CreatedDate column with default value GETDATE()
    IF NOT EXISTS (
        SELECT 1
        FROM INFORMATION_SCHEMA.COLUMNS
        WHERE TABLE_SCHEMA = 'SBCES'
          AND TABLE_NAME = @TableName
          AND COLUMN_NAME = 'CreatedDate'
    )
    BEGIN
        SET @SQL = 'ALTER TABLE SBCES.' + @TableName + ' ADD CreatedDate DATETIME DEFAULT GETDATE()'
        EXEC sp_executesql @SQL
    END

    -- Check and add CreatedBy column with default value 'System'
    IF NOT EXISTS (
        SELECT 1
        FROM INFORMATION_SCHEMA.COLUMNS
        WHERE TABLE_SCHEMA = 'SBCES'
          AND TABLE_NAME = @TableName
          AND COLUMN_NAME = 'CreatedBy'
    )
    BEGIN
        SET @SQL = 'ALTER TABLE SBCES.' + @TableName + ' ADD CreatedBy VARCHAR(50) DEFAULT ''System'''
        EXEC sp_executesql @SQL
    END

    -- Check and add ModifiedDate column with default value GETDATE()
    IF NOT EXISTS (
        SELECT 1
        FROM INFORMATION_SCHEMA.COLUMNS
        WHERE TABLE_SCHEMA = 'SBCES'
          AND TABLE_NAME = @TableName
          AND COLUMN_NAME = 'ModifiedDate'
    )
    BEGIN
        SET @SQL = 'ALTER TABLE SBCES.' + @TableName + ' ADD ModifiedDate DATETIME DEFAULT GETDATE()'
        EXEC sp_executesql @SQL
    END

    -- Check and add ModifiedBy column with default value 'System'
    IF NOT EXISTS (
        SELECT 1
        FROM INFORMATION_SCHEMA.COLUMNS
        WHERE TABLE_SCHEMA = 'SBCES'
          AND TABLE_NAME = @TableName
          AND COLUMN_NAME = 'ModifiedBy'
    )
    BEGIN
        SET @SQL = 'ALTER TABLE SBCES.' + @TableName + ' ADD ModifiedBy VARCHAR(50) DEFAULT ''System'''
        EXEC sp_executesql @SQL
    END

    FETCH NEXT FROM TableCursor INTO @TableName
END

CLOSE TableCursor
DEALLOCATE TableCursor