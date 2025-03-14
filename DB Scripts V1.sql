USE [master]
GO
/****** Object:  Database [BCES]    Script Date: 2024-10-15 7:24:19 PM ******/
CREATE DATABASE [BCES]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BCES', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\BCES.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BCES_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\BCES_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [BCES] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BCES].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BCES] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BCES] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BCES] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BCES] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BCES] SET ARITHABORT OFF 
GO
ALTER DATABASE [BCES] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [BCES] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BCES] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BCES] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BCES] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BCES] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BCES] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BCES] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BCES] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BCES] SET  DISABLE_BROKER 
GO
ALTER DATABASE [BCES] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BCES] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BCES] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BCES] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BCES] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BCES] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BCES] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BCES] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [BCES] SET  MULTI_USER 
GO
ALTER DATABASE [BCES] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BCES] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BCES] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BCES] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BCES] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [BCES] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [BCES] SET QUERY_STORE = ON
GO
ALTER DATABASE [BCES] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [BCES]
GO
/****** Object:  Schema [BCES]    Script Date: 2024-10-15 7:24:20 PM ******/
CREATE SCHEMA [BCES]
GO
/****** Object:  Table [BCES].[BusSeries]    Script Date: 2024-10-15 7:24:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [BCES].[BusSeries](
	[BusSeriesID] [int] IDENTITY(1,1) NOT NULL,
	[BusSeriesCode] [nvarchar](20) NOT NULL,
	[BusSeriesDescription] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[BusSeriesID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[BusSeriesCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [BCES].[CostCentres]    Script Date: 2024-10-15 7:24:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [BCES].[CostCentres](
	[CostCentreID] [int] IDENTITY(1,1) NOT NULL,
	[CostCentreCode] [nvarchar](20) NOT NULL,
	[CostCentreDescription] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[CostCentreID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[CostCentreCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [BCES].[LabourTasks]    Script Date: 2024-10-15 7:24:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [BCES].[LabourTasks](
	[LabourTaskID] [int] IDENTITY(1,1) NOT NULL,
	[RebuiltPartID] [int] NULL,
	[TaskDescription] [nvarchar](255) NOT NULL,
	[LabourPosition] [nvarchar](100) NOT NULL,
	[CostCentreID] [int] NULL,
	[PercentUsage] [decimal](5, 2) NOT NULL,
	[HoursRequiredH] [int] NULL,
	[HoursRequiredM] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[LabourTaskID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [BCES].[Materials]    Script Date: 2024-10-15 7:24:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [BCES].[Materials](
	[MaterialID] [int] IDENTITY(1,1) NOT NULL,
	[RebuiltPartID] [int] NULL,
	[MaterialType] [nvarchar](50) NOT NULL,
	[PartID] [int] NULL,
	[Quantity] [int] NOT NULL,
	[PercentUsage] [decimal](5, 2) NOT NULL,
	[UnitCost] [decimal](10, 2) NULL,
PRIMARY KEY CLUSTERED 
(
	[MaterialID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [BCES].[NonStockCodedParts]    Script Date: 2024-10-15 7:24:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [BCES].[NonStockCodedParts](
	[NonStockPartID] [int] IDENTITY(1,1) NOT NULL,
	[RebuiltPartID] [int] NULL,
	[SupplierID] [int] NOT NULL,
	[PartDescription] [nvarchar](255) NOT NULL,
	[Keyword] [nvarchar](100) NOT NULL,
	[Quantity] [int] NOT NULL,
	[UnitCost] [decimal](10, 2) NOT NULL,
	[CoreCost] [decimal](10, 2) NULL,
	[CostCentreID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[NonStockPartID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [BCES].[PartTypes]    Script Date: 2024-10-15 7:24:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [BCES].[PartTypes](
	[PartTypeID] [int] IDENTITY(1,1) NOT NULL,
	[PartTypeName] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PartTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[PartTypeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [BCES].[RebuiltParts]    Script Date: 2024-10-15 7:24:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [BCES].[RebuiltParts](
	[RebuiltPartID] [int] IDENTITY(1,1) NOT NULL,
	[RebuiltStockCode] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[Keyword] [nvarchar](100) NOT NULL,
	[JobNumber] [nvarchar](50) NOT NULL,
	[CoreCode] [nvarchar](50) NOT NULL,
	[CoreCost] [decimal](10, 2) NOT NULL,
	[SOPNumber] [nvarchar](50) NULL,
	[BusSeriesID] [int] NULL,
	[PartTypeID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[RebuiltPartID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [BCES].[StockCodedParts]    Script Date: 2024-10-15 7:24:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [BCES].[StockCodedParts](
	[StockPartID] [int] IDENTITY(1,1) NOT NULL,
	[RebuiltPartID] [int] NULL,
	[MMSStockCode] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[Keyword] [nvarchar](100) NOT NULL,
	[Quantity] [int] NOT NULL,
	[CoreCost] [decimal](10, 2) NOT NULL,
	[CostCentreID] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[StockPartID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [BCES].[Suppliers]    Script Date: 2024-10-15 7:24:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [BCES].[Suppliers](
	[SupplierID] [int] IDENTITY(1,1) NOT NULL,
	[SupplierName] [nvarchar](100) NOT NULL,
	[SupplierCode] [nvarchar](50) NOT NULL,
	[SupplierContact] [nvarchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[SupplierID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[SupplierCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [BCES].[LabourTasks]  WITH CHECK ADD  CONSTRAINT [FK_LabourTasks_CostCentre] FOREIGN KEY([CostCentreID])
REFERENCES [BCES].[CostCentres] ([CostCentreID])
GO
ALTER TABLE [BCES].[LabourTasks] CHECK CONSTRAINT [FK_LabourTasks_CostCentre]
GO
ALTER TABLE [BCES].[LabourTasks]  WITH CHECK ADD  CONSTRAINT [FK_LabourTasks_RebuiltPart] FOREIGN KEY([RebuiltPartID])
REFERENCES [BCES].[RebuiltParts] ([RebuiltPartID])
GO
ALTER TABLE [BCES].[LabourTasks] CHECK CONSTRAINT [FK_LabourTasks_RebuiltPart]
GO
ALTER TABLE [BCES].[Materials]  WITH CHECK ADD  CONSTRAINT [FK_Materials_RebuiltPart] FOREIGN KEY([RebuiltPartID])
REFERENCES [BCES].[RebuiltParts] ([RebuiltPartID])
GO
ALTER TABLE [BCES].[Materials] CHECK CONSTRAINT [FK_Materials_RebuiltPart]
GO
ALTER TABLE [BCES].[NonStockCodedParts]  WITH CHECK ADD  CONSTRAINT [FK_NonStockCodedParts_RebuiltPart] FOREIGN KEY([RebuiltPartID])
REFERENCES [BCES].[RebuiltParts] ([RebuiltPartID])
GO
ALTER TABLE [BCES].[NonStockCodedParts] CHECK CONSTRAINT [FK_NonStockCodedParts_RebuiltPart]
GO
ALTER TABLE [BCES].[NonStockCodedParts]  WITH CHECK ADD  CONSTRAINT [FK_NonStockParts_CostCentre] FOREIGN KEY([CostCentreID])
REFERENCES [BCES].[CostCentres] ([CostCentreID])
GO
ALTER TABLE [BCES].[NonStockCodedParts] CHECK CONSTRAINT [FK_NonStockParts_CostCentre]
GO
ALTER TABLE [BCES].[NonStockCodedParts]  WITH CHECK ADD  CONSTRAINT [FK_NonStockParts_Supplier] FOREIGN KEY([SupplierID])
REFERENCES [BCES].[Suppliers] ([SupplierID])
GO
ALTER TABLE [BCES].[NonStockCodedParts] CHECK CONSTRAINT [FK_NonStockParts_Supplier]
GO
ALTER TABLE [BCES].[RebuiltParts]  WITH CHECK ADD  CONSTRAINT [FK_RebuiltParts_BusSeries] FOREIGN KEY([BusSeriesID])
REFERENCES [BCES].[BusSeries] ([BusSeriesID])
GO
ALTER TABLE [BCES].[RebuiltParts] CHECK CONSTRAINT [FK_RebuiltParts_BusSeries]
GO
ALTER TABLE [BCES].[RebuiltParts]  WITH CHECK ADD  CONSTRAINT [FK_RebuiltParts_PartTypes] FOREIGN KEY([PartTypeID])
REFERENCES [BCES].[PartTypes] ([PartTypeID])
GO
ALTER TABLE [BCES].[RebuiltParts] CHECK CONSTRAINT [FK_RebuiltParts_PartTypes]
GO
ALTER TABLE [BCES].[StockCodedParts]  WITH CHECK ADD  CONSTRAINT [FK_StockCodedParts_RebuiltPart] FOREIGN KEY([RebuiltPartID])
REFERENCES [BCES].[RebuiltParts] ([RebuiltPartID])
GO
ALTER TABLE [BCES].[StockCodedParts] CHECK CONSTRAINT [FK_StockCodedParts_RebuiltPart]
GO
ALTER TABLE [BCES].[StockCodedParts]  WITH CHECK ADD  CONSTRAINT [FK_StockParts_CostCentre] FOREIGN KEY([CostCentreID])
REFERENCES [BCES].[CostCentres] ([CostCentreID])
GO
ALTER TABLE [BCES].[StockCodedParts] CHECK CONSTRAINT [FK_StockParts_CostCentre]
GO
/****** Object:  StoredProcedure [BCES].[GetRebuildPartEstimate]    Script Date: 2024-10-15 7:24:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      [Your Name]
-- Create date: [Current Date]
-- Description: Stored procedure to get rebuild part estimate
-- =============================================
CREATE PROCEDURE [BCES].[GetRebuildPartEstimate]
    @JobNumber NVARCHAR(50),
    @RebuildNumber NVARCHAR(50),
    @MMSBuyCode NVARCHAR(50),
    @CoreCode NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    -- Declare variables for dynamic SQL
    DECLARE @ErrorNumber INT;
    DECLARE @ErrorMessage NVARCHAR(4000);

    BEGIN TRY
        -- Initialize the base query
        SELECT 
            LT.TaskDescription AS LabourTask,
            CC.CostCentreCode AS CC,
            LT.PercentUsage AS Usage,
            LT.HoursRequiredH + LT.HoursRequiredM / 60.0 AS Time,
            LT.HoursRequiredH * 60 + LT.HoursRequiredM AS WrenchTime,
            LT.LabourPosition AS LabourType
        FROM 
            [BCES].[RebuiltParts] RP
        INNER JOIN 
            [BCES].[LabourTasks] LT ON RP.RebuiltPartID = LT.RebuiltPartID
        INNER JOIN 
            [BCES].[CostCentres] CC ON LT.CostCentreID = CC.CostCentreID
        INNER JOIN 
            [BCES].[StockCodedParts] SCP ON RP.RebuiltPartID = SCP.RebuiltPartID
        WHERE 
            RP.JobNumber = @JobNumber
            AND RP.RebuiltStockCode = @RebuildNumber
            AND RP.CoreCode = @CoreCode
            AND SCP.MMSStockCode = @MMSBuyCode;
    END TRY
    BEGIN CATCH
        -- Capture error details
        SET @ErrorNumber = ERROR_NUMBER();
        SET @ErrorMessage = ERROR_MESSAGE();

        -- Log the error (you can replace this with your actual logging mechanism)
        RAISERROR('Error Number: %d, Error Message: %s', 16, 1, @ErrorNumber, @ErrorMessage);
    END CATCH
END
GO
/****** Object:  StoredProcedure [BCES].[SearchNonStockCodedParts]    Script Date: 2024-10-15 7:24:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      [Your Name]
-- Create date: [Current Date]
-- Description: Stored procedure to search NonStockCodedParts based on various criteria
-- =============================================
CREATE PROCEDURE [BCES].[SearchNonStockCodedParts]
    @StockCode NVARCHAR(50) = NULL,
    @Keyword NVARCHAR(100) = NULL,
    @Price DECIMAL(10, 2) = NULL,
    @Description NVARCHAR(255) = NULL,
    @ReturnCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Declare variables for dynamic SQL
    DECLARE @ErrorNumber INT;
    DECLARE @ErrorMessage NVARCHAR(4000);

    BEGIN TRY
        -- Initialize the base query
        SELECT 
            NSCP.NonStockPartID,
            NSCP.RebuiltPartID,
            NSCP.SupplierID,
            NSCP.PartDescription,
            NSCP.Keyword AS NonStockPartKeyword,
            NSCP.Quantity,
            NSCP.UnitCost,
            NSCP.CoreCost,
            NSCP.CostCentreID,
            RP.RebuiltStockCode,
            RP.Description AS RebuiltPartDescription,
            RP.Keyword AS RebuiltPartKeyword,
            RP.JobNumber,
            RP.CoreCode,
            RP.CoreCost AS RebuiltPartCoreCost,
            RP.SOPNumber,
            BS.BusSeriesCode,
            BS.BusSeriesDescription,
            PT.PartTypeName,
            CC.CostCentreCode,
            CC.CostCentreDescription,
            S.SupplierName,
            S.SupplierCode,
            S.SupplierContact
        INTO #TempResults
        FROM 
            [BCES].[NonStockCodedParts] NSCP
        LEFT JOIN 
            [BCES].[RebuiltParts] RP ON NSCP.RebuiltPartID = RP.RebuiltPartID
        LEFT JOIN 
            [BCES].[BusSeries] BS ON RP.BusSeriesID = BS.BusSeriesID
        LEFT JOIN 
            [BCES].[PartTypes] PT ON RP.PartTypeID = PT.PartTypeID
        LEFT JOIN 
            [BCES].[CostCentres] CC ON NSCP.CostCentreID = CC.CostCentreID
        LEFT JOIN 
            [BCES].[Suppliers] S ON NSCP.SupplierID = S.SupplierID
        WHERE 1=1

        -- Add conditions based on the provided parameters
        IF @StockCode IS NOT NULL
            DELETE FROM #TempResults WHERE RebuiltStockCode <> @StockCode;

        IF @Keyword IS NOT NULL
            DELETE FROM #TempResults WHERE Keyword <> @Keyword AND NonStockPartKeyword <> @Keyword;

        IF @Price IS NOT NULL
            DELETE FROM #TempResults WHERE UnitCost <> @Price;

        IF @Description IS NOT NULL
            DELETE FROM #TempResults WHERE RebuiltPartDescription <> @Description AND PartDescription <> @Description;

        -- Get the count of results
        SELECT @ReturnCount = COUNT(*) FROM #TempResults;

        -- Print the count of results for debugging
        PRINT 'Return Count:';
        PRINT @ReturnCount;

        -- Return the results
        SELECT * FROM #TempResults;
    END TRY
    BEGIN CATCH
        -- Capture error details
        SET @ErrorNumber = ERROR_NUMBER();
        SET @ErrorMessage = ERROR_MESSAGE();

        -- Log the error (you can replace this with your actual logging mechanism)
        RAISERROR('Error Number: %d, Error Message: %s', 16, 1, @ErrorNumber, @ErrorMessage);

        -- Set return count to -1 to indicate an error
        SET @ReturnCount = -1;
    END CATCH

    -- Clean up the temporary table
    DROP TABLE IF EXISTS #TempResults;
END
GO
/****** Object:  StoredProcedure [BCES].[SearchRebuiltParts]    Script Date: 2024-10-15 7:24:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      [Your Name]
-- Create date: [Current Date]
-- Description: Stored procedure to search RebuiltParts based on various criteria
-- =============================================
CREATE PROCEDURE [BCES].[SearchRebuiltParts]
    @RebuildNumber NVARCHAR(50) = NULL,
    @Keyword NVARCHAR(100) = NULL,
    @BusSeries NVARCHAR(20) = NULL,
    @Description NVARCHAR(255) = NULL,
    @ReturnCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Declare variables for dynamic SQL
    DECLARE @ErrorNumber INT;
    DECLARE @ErrorMessage NVARCHAR(4000);

    -- Initialize the base query
    SELECT 
        SCP.StockPartID,
        SCP.MMSStockCode,
        SCP.Description AS StockPartDescription,
        SCP.Keyword AS StockPartKeyword,
        SCP.Quantity,
        SCP.CoreCost,
        RP.RebuiltStockCode,
        RP.Description AS RebuiltPartDescription,
        RP.Keyword AS RebuiltPartKeyword,
        RP.JobNumber,
        RP.CoreCode,
        RP.CoreCost AS RebuiltPartCoreCost,
        RP.SOPNumber,
        BS.BusSeriesCode,
        BS.BusSeriesDescription,
        PT.PartTypeName,
        CC.CostCentreCode,
        CC.CostCentreDescription
    INTO #TempResults
    FROM 
        [BCES].[StockCodedParts] SCP
    LEFT JOIN 
        [BCES].[RebuiltParts] RP ON SCP.RebuiltPartID = RP.RebuiltPartID
    LEFT JOIN 
        [BCES].[BusSeries] BS ON RP.BusSeriesID = BS.BusSeriesID
    LEFT JOIN 
        [BCES].[PartTypes] PT ON RP.PartTypeID = PT.PartTypeID
    LEFT JOIN 
        [BCES].[CostCentres] CC ON SCP.CostCentreID = CC.CostCentreID
    WHERE 1=1

    -- Add conditions based on the provided parameters
    IF @RebuildNumber IS NOT NULL
        DELETE FROM #TempResults WHERE RebuiltStockCode <> @RebuildNumber;

    IF @Keyword IS NOT NULL
        DELETE FROM #TempResults WHERE Keyword <> @Keyword AND StockPartKeyword <> @Keyword;

    IF @BusSeries IS NOT NULL
        DELETE FROM #TempResults WHERE BusSeriesCode <> @BusSeries;

    IF @Description IS NOT NULL
        DELETE FROM #TempResults WHERE RebuiltPartDescription <> @Description AND StockPartDescription <> @Description;

    -- Get the count of results
    SELECT @ReturnCount = COUNT(*) FROM #TempResults;

    -- Print the count of results for debugging
    PRINT 'Return Count:';
    PRINT @ReturnCount;

    -- Return the results
    SELECT * FROM #TempResults;

    -- Clean up the temporary table
    DROP TABLE IF EXISTS #TempResults;
END
GO
/****** Object:  StoredProcedure [BCES].[SearchStockCodedParts]    Script Date: 2024-10-15 7:24:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:      [Your Name]
-- Create date: [Current Date]
-- Description: Stored procedure to search StockCodedParts based on various criteria
-- =============================================
CREATE PROCEDURE [BCES].[SearchStockCodedParts]
    @StockCode NVARCHAR(50) = NULL,
    @Keyword NVARCHAR(100) = NULL,
    @Price DECIMAL(10, 2) = NULL,
    @Description NVARCHAR(255) = NULL,
    @ReturnCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Declare variables for dynamic SQL
    DECLARE @ErrorNumber INT;
    DECLARE @ErrorMessage NVARCHAR(4000);

    BEGIN TRY
        -- Initialize the base query
        SELECT 
            SCP.StockPartID,
            SCP.MMSStockCode,
            SCP.Description AS StockPartDescription,
            SCP.Keyword AS StockPartKeyword,
            SCP.Quantity,
            SCP.CoreCost,
            RP.RebuiltStockCode,
            RP.Description AS RebuiltPartDescription,
            RP.Keyword AS RebuiltPartKeyword,
            RP.JobNumber,
            RP.CoreCode,
            RP.CoreCost AS RebuiltPartCoreCost,
            RP.SOPNumber,
            BS.BusSeriesCode,
            BS.BusSeriesDescription,
            PT.PartTypeName,
            CC.CostCentreCode,
            CC.CostCentreDescription
        INTO #TempResults
        FROM 
            [BCES].[StockCodedParts] SCP
        LEFT JOIN 
            [BCES].[RebuiltParts] RP ON SCP.RebuiltPartID = RP.RebuiltPartID
        LEFT JOIN 
            [BCES].[BusSeries] BS ON RP.BusSeriesID = BS.BusSeriesID
        LEFT JOIN 
            [BCES].[PartTypes] PT ON RP.PartTypeID = PT.PartTypeID
        LEFT JOIN 
            [BCES].[CostCentres] CC ON SCP.CostCentreID = CC.CostCentreID
        WHERE 1=1

        -- Add conditions based on the provided parameters
        IF @StockCode IS NOT NULL
            DELETE FROM #TempResults WHERE MMSStockCode <> @StockCode;

        IF @Keyword IS NOT NULL
            DELETE FROM #TempResults WHERE Keyword <> @Keyword AND StockPartKeyword <> @Keyword;

        IF @Price IS NOT NULL
            DELETE FROM #TempResults WHERE CoreCost <> @Price;

        IF @Description IS NOT NULL
            DELETE FROM #TempResults WHERE RebuiltPartDescription <> @Description AND StockPartDescription <> @Description;

        -- Get the count of results
        SELECT @ReturnCount = COUNT(*) FROM #TempResults;

        -- Print the count of results for debugging
        PRINT 'Return Count:';
        PRINT @ReturnCount;

        -- Return the results
        SELECT * FROM #TempResults;
    END TRY
    BEGIN CATCH
        -- Capture error details
        SET @ErrorNumber = ERROR_NUMBER();
        SET @ErrorMessage = ERROR_MESSAGE();

        -- Log the error (you can replace this with your actual logging mechanism)
        RAISERROR('Error Number: %d, Error Message: %s', 16, 1, @ErrorNumber, @ErrorMessage);

        -- Set return count to -1 to indicate an error
        SET @ReturnCount = -1;
    END CATCH

    -- Clean up the temporary table
    DROP TABLE IF EXISTS #TempResults;
END
GO
USE [master]
GO
ALTER DATABASE [BCES] SET  READ_WRITE 
GO
