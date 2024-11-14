WITH SupplierNamesCTE AS (
                SELECT DISTINCT [OrigSupplierName]
                FROM [SCES].[NscPartsUsed]
            ),
            SupplierNamesWithRowNumber AS (
                SELECT ROW_NUMBER() OVER (ORDER BY OrigSupplierName) AS Value, OrigSupplierName AS Text
                FROM SupplierNamesCTE
            )
            SELECT Value, Text
            FROM SupplierNamesWithRowNumber