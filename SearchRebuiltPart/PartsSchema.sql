USE [BCES]
GO
/****** Object:  Table [BCES].[BusSeries]    Script Date: 2024-10-29 5:10:46 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [BCES].[CostCentres]    Script Date: 2024-10-29 5:10:46 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [BCES].[LabourTasks]    Script Date: 2024-10-29 5:10:46 PM ******/
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
/****** Object:  Table [BCES].[Materials]    Script Date: 2024-10-29 5:10:46 PM ******/
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
/****** Object:  Table [BCES].[NonStockCodedParts]    Script Date: 2024-10-29 5:10:46 PM ******/
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
/****** Object:  Table [BCES].[PartTypes]    Script Date: 2024-10-29 5:10:46 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [BCES].[RebuiltParts]    Script Date: 2024-10-29 5:10:46 PM ******/
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
/****** Object:  Table [BCES].[StockCodedParts]    Script Date: 2024-10-29 5:10:46 PM ******/
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
/****** Object:  Table [BCES].[Suppliers]    Script Date: 2024-10-29 5:10:46 PM ******/
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
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [BCES].[BusSeries] ON 
GO
INSERT [BCES].[BusSeries] ([BusSeriesID], [BusSeriesCode], [BusSeriesDescription]) VALUES (1, N'ALL CITY BUSES', N'ALL CITY BUSES')
GO
INSERT [BCES].[BusSeries] ([BusSeriesID], [BusSeriesCode], [BusSeriesDescription]) VALUES (2, N'ALRV', N'ALRV')
GO
INSERT [BCES].[BusSeries] ([BusSeriesID], [BusSeriesCode], [BusSeriesDescription]) VALUES (3, N'ARTIC TEST', N'ARTIC TEST')
GO
INSERT [BCES].[BusSeries] ([BusSeriesID], [BusSeriesCode], [BusSeriesDescription]) VALUES (4, N'CLRV', N'CLRV')
GO
INSERT [BCES].[BusSeries] ([BusSeriesID], [BusSeriesCode], [BusSeriesDescription]) VALUES (5, N'FACILITY', N'FACILITY')
GO
INSERT [BCES].[BusSeries] ([BusSeriesID], [BusSeriesCode], [BusSeriesDescription]) VALUES (6, N'FLYER D 9001', N'FLYER D 9001 (6000-6122)')
GO
INSERT [BCES].[BusSeries] ([BusSeriesID], [BusSeriesCode], [BusSeriesDescription]) VALUES (7, N'GMC T6H', N'GMC T6H (2000-2499)')
GO
SET IDENTITY_INSERT [BCES].[BusSeries] OFF
GO
SET IDENTITY_INSERT [BCES].[CostCentres] ON 
GO
INSERT [BCES].[CostCentres] ([CostCentreID], [CostCentreCode], [CostCentreDescription]) VALUES (1, N'05H7', N'Hydraulics Department')
GO
INSERT [BCES].[CostCentres] ([CostCentreID], [CostCentreCode], [CostCentreDescription]) VALUES (2, N'05H1', N'Overhaul Department')
GO
INSERT [BCES].[CostCentres] ([CostCentreID], [CostCentreCode], [CostCentreDescription]) VALUES (3, N'06E3', N'Electrical Systems')
GO
INSERT [BCES].[CostCentres] ([CostCentreID], [CostCentreCode], [CostCentreDescription]) VALUES (4, N'04M4', N'Mechanical Department')
GO
INSERT [BCES].[CostCentres] ([CostCentreID], [CostCentreCode], [CostCentreDescription]) VALUES (5, N'07B5', N'Body and Paint')
GO
SET IDENTITY_INSERT [BCES].[CostCentres] OFF
GO
SET IDENTITY_INSERT [BCES].[LabourTasks] ON 
GO
INSERT [BCES].[LabourTasks] ([LabourTaskID], [RebuiltPartID], [TaskDescription], [LabourPosition], [CostCentreID], [PercentUsage], [HoursRequiredH], [HoursRequiredM]) VALUES (1, 1, N'Engine Dismantling', N'Mechanic', 1, CAST(100.00 AS Decimal(5, 2)), 6, 30)
GO
INSERT [BCES].[LabourTasks] ([LabourTaskID], [RebuiltPartID], [TaskDescription], [LabourPosition], [CostCentreID], [PercentUsage], [HoursRequiredH], [HoursRequiredM]) VALUES (2, 1, N'Cooling System Installation', N'Technician', 1, CAST(100.00 AS Decimal(5, 2)), 4, 15)
GO
INSERT [BCES].[LabourTasks] ([LabourTaskID], [RebuiltPartID], [TaskDescription], [LabourPosition], [CostCentreID], [PercentUsage], [HoursRequiredH], [HoursRequiredM]) VALUES (3, 2, N'Transmission Disassembly', N'Mechanic', 2, CAST(100.00 AS Decimal(5, 2)), 5, 45)
GO
INSERT [BCES].[LabourTasks] ([LabourTaskID], [RebuiltPartID], [TaskDescription], [LabourPosition], [CostCentreID], [PercentUsage], [HoursRequiredH], [HoursRequiredM]) VALUES (4, 2, N'Fluid Refill', N'Technician', 2, CAST(100.00 AS Decimal(5, 2)), 2, 0)
GO
INSERT [BCES].[LabourTasks] ([LabourTaskID], [RebuiltPartID], [TaskDescription], [LabourPosition], [CostCentreID], [PercentUsage], [HoursRequiredH], [HoursRequiredM]) VALUES (5, 3, N'Brake Pad Replacement', N'Mechanic', 3, CAST(100.00 AS Decimal(5, 2)), 3, 30)
GO
INSERT [BCES].[LabourTasks] ([LabourTaskID], [RebuiltPartID], [TaskDescription], [LabourPosition], [CostCentreID], [PercentUsage], [HoursRequiredH], [HoursRequiredM]) VALUES (6, 4, N'Suspension Adjustment', N'Mechanic', 4, CAST(100.00 AS Decimal(5, 2)), 4, 0)
GO
INSERT [BCES].[LabourTasks] ([LabourTaskID], [RebuiltPartID], [TaskDescription], [LabourPosition], [CostCentreID], [PercentUsage], [HoursRequiredH], [HoursRequiredM]) VALUES (7, 5, N'Wiring Replacement', N'Electrician', 3, CAST(100.00 AS Decimal(5, 2)), 8, 0)
GO
INSERT [BCES].[LabourTasks] ([LabourTaskID], [RebuiltPartID], [TaskDescription], [LabourPosition], [CostCentreID], [PercentUsage], [HoursRequiredH], [HoursRequiredM]) VALUES (8, 5, N'Battery Installation', N'Electrician', 5, CAST(100.00 AS Decimal(5, 2)), 6, 30)
GO
INSERT [BCES].[LabourTasks] ([LabourTaskID], [RebuiltPartID], [TaskDescription], [LabourPosition], [CostCentreID], [PercentUsage], [HoursRequiredH], [HoursRequiredM]) VALUES (9, 1, N'OVERHAUL', N'UNIT BUILDER', 2, CAST(100.00 AS Decimal(5, 2)), 4, 0)
GO
INSERT [BCES].[LabourTasks] ([LabourTaskID], [RebuiltPartID], [TaskDescription], [LabourPosition], [CostCentreID], [PercentUsage], [HoursRequiredH], [HoursRequiredM]) VALUES (10, 1, N'SWIS WORK', N'COACH TECHNICIAN', 1, CAST(100.00 AS Decimal(5, 2)), 10, 22)
GO
INSERT [BCES].[LabourTasks] ([LabourTaskID], [RebuiltPartID], [TaskDescription], [LabourPosition], [CostCentreID], [PercentUsage], [HoursRequiredH], [HoursRequiredM]) VALUES (11, 2, N'OVERHAUL', N'UNIT BUILDER', 2, CAST(100.00 AS Decimal(5, 2)), 3, 0)
GO
INSERT [BCES].[LabourTasks] ([LabourTaskID], [RebuiltPartID], [TaskDescription], [LabourPosition], [CostCentreID], [PercentUsage], [HoursRequiredH], [HoursRequiredM]) VALUES (12, 3, N'SWIS WORK', N'COACH TECHNICIAN', 1, CAST(100.00 AS Decimal(5, 2)), 5, 30)
GO
INSERT [BCES].[LabourTasks] ([LabourTaskID], [RebuiltPartID], [TaskDescription], [LabourPosition], [CostCentreID], [PercentUsage], [HoursRequiredH], [HoursRequiredM]) VALUES (13, 4, N'INSTALLATION', N'MECHANIC', 2, CAST(100.00 AS Decimal(5, 2)), 2, 0)
GO
SET IDENTITY_INSERT [BCES].[LabourTasks] OFF
GO
SET IDENTITY_INSERT [BCES].[Materials] ON 
GO
INSERT [BCES].[Materials] ([MaterialID], [RebuiltPartID], [MaterialType], [PartID], [Quantity], [PercentUsage], [UnitCost]) VALUES (1, 1, N'Stock Coded Part', 1, 5, CAST(100.00 AS Decimal(5, 2)), CAST(100.00 AS Decimal(10, 2)))
GO
INSERT [BCES].[Materials] ([MaterialID], [RebuiltPartID], [MaterialType], [PartID], [Quantity], [PercentUsage], [UnitCost]) VALUES (2, 1, N'Non-Stock Coded Part', 1, 5, CAST(100.00 AS Decimal(5, 2)), CAST(50.00 AS Decimal(10, 2)))
GO
INSERT [BCES].[Materials] ([MaterialID], [RebuiltPartID], [MaterialType], [PartID], [Quantity], [PercentUsage], [UnitCost]) VALUES (3, 2, N'Stock Coded Part', 2, 10, CAST(100.00 AS Decimal(5, 2)), CAST(80.00 AS Decimal(10, 2)))
GO
INSERT [BCES].[Materials] ([MaterialID], [RebuiltPartID], [MaterialType], [PartID], [Quantity], [PercentUsage], [UnitCost]) VALUES (4, 2, N'Non-Stock Coded Part', 2, 10, CAST(100.00 AS Decimal(5, 2)), CAST(25.00 AS Decimal(10, 2)))
GO
INSERT [BCES].[Materials] ([MaterialID], [RebuiltPartID], [MaterialType], [PartID], [Quantity], [PercentUsage], [UnitCost]) VALUES (5, 3, N'Stock Coded Part', 3, 8, CAST(100.00 AS Decimal(5, 2)), CAST(150.00 AS Decimal(10, 2)))
GO
INSERT [BCES].[Materials] ([MaterialID], [RebuiltPartID], [MaterialType], [PartID], [Quantity], [PercentUsage], [UnitCost]) VALUES (6, 3, N'Non-Stock Coded Part', 3, 8, CAST(100.00 AS Decimal(5, 2)), CAST(75.00 AS Decimal(10, 2)))
GO
INSERT [BCES].[Materials] ([MaterialID], [RebuiltPartID], [MaterialType], [PartID], [Quantity], [PercentUsage], [UnitCost]) VALUES (7, 4, N'Stock Coded Part', 4, 10, CAST(100.00 AS Decimal(5, 2)), CAST(90.00 AS Decimal(10, 2)))
GO
INSERT [BCES].[Materials] ([MaterialID], [RebuiltPartID], [MaterialType], [PartID], [Quantity], [PercentUsage], [UnitCost]) VALUES (8, 4, N'Non-Stock Coded Part', 4, 10, CAST(100.00 AS Decimal(5, 2)), CAST(35.00 AS Decimal(10, 2)))
GO
INSERT [BCES].[Materials] ([MaterialID], [RebuiltPartID], [MaterialType], [PartID], [Quantity], [PercentUsage], [UnitCost]) VALUES (9, 5, N'Stock Coded Part', 5, 3, CAST(100.00 AS Decimal(5, 2)), CAST(200.00 AS Decimal(10, 2)))
GO
INSERT [BCES].[Materials] ([MaterialID], [RebuiltPartID], [MaterialType], [PartID], [Quantity], [PercentUsage], [UnitCost]) VALUES (10, 5, N'Non-Stock Coded Part', 5, 3, CAST(100.00 AS Decimal(5, 2)), CAST(500.00 AS Decimal(10, 2)))
GO
INSERT [BCES].[Materials] ([MaterialID], [RebuiltPartID], [MaterialType], [PartID], [Quantity], [PercentUsage], [UnitCost]) VALUES (11, 1, N'Stock Coded Part', 1, 100, CAST(100.00 AS Decimal(5, 2)), CAST(20.87 AS Decimal(10, 2)))
GO
INSERT [BCES].[Materials] ([MaterialID], [RebuiltPartID], [MaterialType], [PartID], [Quantity], [PercentUsage], [UnitCost]) VALUES (12, 1, N'Stock Coded Part', 2, 100, CAST(100.00 AS Decimal(5, 2)), CAST(146.89 AS Decimal(10, 2)))
GO
INSERT [BCES].[Materials] ([MaterialID], [RebuiltPartID], [MaterialType], [PartID], [Quantity], [PercentUsage], [UnitCost]) VALUES (13, 1, N'Stock Coded Part', 3, 2, CAST(100.00 AS Decimal(5, 2)), CAST(9.64 AS Decimal(10, 2)))
GO
INSERT [BCES].[Materials] ([MaterialID], [RebuiltPartID], [MaterialType], [PartID], [Quantity], [PercentUsage], [UnitCost]) VALUES (14, 2, N'Non-Stock Coded Part', 1, 1, CAST(100.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(10, 2)))
GO
INSERT [BCES].[Materials] ([MaterialID], [RebuiltPartID], [MaterialType], [PartID], [Quantity], [PercentUsage], [UnitCost]) VALUES (15, 2, N'Non-Stock Coded Part', 2, 1, CAST(100.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(10, 2)))
GO
INSERT [BCES].[Materials] ([MaterialID], [RebuiltPartID], [MaterialType], [PartID], [Quantity], [PercentUsage], [UnitCost]) VALUES (16, 3, N'Non-Stock Coded Part', 3, 1, CAST(100.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(10, 2)))
GO
INSERT [BCES].[Materials] ([MaterialID], [RebuiltPartID], [MaterialType], [PartID], [Quantity], [PercentUsage], [UnitCost]) VALUES (17, 3, N'Non-Stock Coded Part', 4, 1, CAST(100.00 AS Decimal(5, 2)), CAST(0.00 AS Decimal(10, 2)))
GO
SET IDENTITY_INSERT [BCES].[Materials] OFF
GO
SET IDENTITY_INSERT [BCES].[NonStockCodedParts] ON 
GO
INSERT [BCES].[NonStockCodedParts] ([NonStockPartID], [RebuiltPartID], [SupplierID], [PartDescription], [Keyword], [Quantity], [UnitCost], [CoreCost], [CostCentreID]) VALUES (1, 1, 1, N'Custom Engine Mount', N'ENGINE', 5, CAST(50.00 AS Decimal(10, 2)), CAST(10.00 AS Decimal(10, 2)), 1)
GO
INSERT [BCES].[NonStockCodedParts] ([NonStockPartID], [RebuiltPartID], [SupplierID], [PartDescription], [Keyword], [Quantity], [UnitCost], [CoreCost], [CostCentreID]) VALUES (2, 2, 2, N'Special Transmission Fluid', N'FLUID', 10, CAST(25.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), 2)
GO
INSERT [BCES].[NonStockCodedParts] ([NonStockPartID], [RebuiltPartID], [SupplierID], [PartDescription], [Keyword], [Quantity], [UnitCost], [CoreCost], [CostCentreID]) VALUES (3, 3, 3, N'Custom Brake Pads', N'BRAKE', 8, CAST(75.00 AS Decimal(10, 2)), CAST(5.00 AS Decimal(10, 2)), 3)
GO
INSERT [BCES].[NonStockCodedParts] ([NonStockPartID], [RebuiltPartID], [SupplierID], [PartDescription], [Keyword], [Quantity], [UnitCost], [CoreCost], [CostCentreID]) VALUES (4, 4, 4, N'High-Performance Suspension Bush', N'SUSPENSION', 10, CAST(35.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), 4)
GO
INSERT [BCES].[NonStockCodedParts] ([NonStockPartID], [RebuiltPartID], [SupplierID], [PartDescription], [Keyword], [Quantity], [UnitCost], [CoreCost], [CostCentreID]) VALUES (5, 5, 5, N'High-Voltage Battery Pack', N'BATTERY', 3, CAST(500.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), 5)
GO
INSERT [BCES].[NonStockCodedParts] ([NonStockPartID], [RebuiltPartID], [SupplierID], [PartDescription], [Keyword], [Quantity], [UnitCost], [CoreCost], [CostCentreID]) VALUES (6, 1, 1, N'SEAT INSERT', N'SUPPORT', 100, CAST(15.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), 1)
GO
INSERT [BCES].[NonStockCodedParts] ([NonStockPartID], [RebuiltPartID], [SupplierID], [PartDescription], [Keyword], [Quantity], [UnitCost], [CoreCost], [CostCentreID]) VALUES (7, 2, 2, N'SEAT BELT', N'BELT', 100, CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), 1)
GO
INSERT [BCES].[NonStockCodedParts] ([NonStockPartID], [RebuiltPartID], [SupplierID], [PartDescription], [Keyword], [Quantity], [UnitCost], [CoreCost], [CostCentreID]) VALUES (8, 3, 3, N'AIR DRILL 3/4 IN', N'DRILL', 100, CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), 1)
GO
INSERT [BCES].[NonStockCodedParts] ([NonStockPartID], [RebuiltPartID], [SupplierID], [PartDescription], [Keyword], [Quantity], [UnitCost], [CoreCost], [CostCentreID]) VALUES (9, 4, 1, N'PIN TAPER STEEL #0 X 1-1/2', N'PIN', 100, CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), 1)
GO
INSERT [BCES].[NonStockCodedParts] ([NonStockPartID], [RebuiltPartID], [SupplierID], [PartDescription], [Keyword], [Quantity], [UnitCost], [CoreCost], [CostCentreID]) VALUES (10, 5, 2, N'WASHER', N'WASHER', 100, CAST(0.00 AS Decimal(10, 2)), CAST(0.00 AS Decimal(10, 2)), 1)
GO
SET IDENTITY_INSERT [BCES].[NonStockCodedParts] OFF
GO
SET IDENTITY_INSERT [BCES].[PartTypes] ON 
GO
INSERT [BCES].[PartTypes] ([PartTypeID], [PartTypeName]) VALUES (5, N'Body Part')
GO
INSERT [BCES].[PartTypes] ([PartTypeID], [PartTypeName]) VALUES (6, N'Electrical Part')
GO
INSERT [BCES].[PartTypes] ([PartTypeID], [PartTypeName]) VALUES (4, N'Engine Part')
GO
INSERT [BCES].[PartTypes] ([PartTypeID], [PartTypeName]) VALUES (3, N'Non-Stock Coded Part')
GO
INSERT [BCES].[PartTypes] ([PartTypeID], [PartTypeName]) VALUES (1, N'Rebuilt Part')
GO
INSERT [BCES].[PartTypes] ([PartTypeID], [PartTypeName]) VALUES (2, N'Stock Coded Part')
GO
SET IDENTITY_INSERT [BCES].[PartTypes] OFF
GO
SET IDENTITY_INSERT [BCES].[RebuiltParts] ON 
GO
INSERT [BCES].[RebuiltParts] ([RebuiltPartID], [RebuiltStockCode], [Description], [Keyword], [JobNumber], [CoreCode], [CoreCost], [SOPNumber], [BusSeriesID], [PartTypeID]) VALUES (1, N'SW1001', N'Engine Overhaul', N'ENGINE', N'001', N'ENG1234', CAST(1200.00 AS Decimal(10, 2)), N'SOP1', 1, 1)
GO
INSERT [BCES].[RebuiltParts] ([RebuiltPartID], [RebuiltStockCode], [Description], [Keyword], [JobNumber], [CoreCode], [CoreCost], [SOPNumber], [BusSeriesID], [PartTypeID]) VALUES (2, N'SW1002', N'Transmission Rebuild', N'TRANSMISSION', N'002', N'TRN5678', CAST(800.00 AS Decimal(10, 2)), N'SOP2', 2, 1)
GO
INSERT [BCES].[RebuiltParts] ([RebuiltPartID], [RebuiltStockCode], [Description], [Keyword], [JobNumber], [CoreCode], [CoreCost], [SOPNumber], [BusSeriesID], [PartTypeID]) VALUES (3, N'SW1003', N'Brake System Upgrade', N'BRAKE', N'003', N'BRK1234', CAST(600.00 AS Decimal(10, 2)), N'SOP3', 3, 1)
GO
INSERT [BCES].[RebuiltParts] ([RebuiltPartID], [RebuiltStockCode], [Description], [Keyword], [JobNumber], [CoreCode], [CoreCost], [SOPNumber], [BusSeriesID], [PartTypeID]) VALUES (4, N'SW1004', N'Suspension Alignment', N'SUSPENSION', N'004', N'SUS5678', CAST(500.00 AS Decimal(10, 2)), N'SOP4', 4, 1)
GO
INSERT [BCES].[RebuiltParts] ([RebuiltPartID], [RebuiltStockCode], [Description], [Keyword], [JobNumber], [CoreCode], [CoreCost], [SOPNumber], [BusSeriesID], [PartTypeID]) VALUES (5, N'SW1005', N'Electrical System Overhaul', N'ELECTRICAL', N'005', N'ELE9876', CAST(700.00 AS Decimal(10, 2)), N'SOP5', 2, 1)
GO
INSERT [BCES].[RebuiltParts] ([RebuiltPartID], [RebuiltStockCode], [Description], [Keyword], [JobNumber], [CoreCode], [CoreCost], [SOPNumber], [BusSeriesID], [PartTypeID]) VALUES (6, N'SW1005', N'EGR AND VNT LINE UPGRADE', N'SWIS', N'92578', N'023415', CAST(0.00 AS Decimal(10, 2)), N'0', 1, 1)
GO
INSERT [BCES].[RebuiltParts] ([RebuiltPartID], [RebuiltStockCode], [Description], [Keyword], [JobNumber], [CoreCode], [CoreCost], [SOPNumber], [BusSeriesID], [PartTypeID]) VALUES (7, N'SW1016', N'HYDRAULIC HOSE AND CLAMP REPLACEMENT', N'SWIS', N'92579', N'023416', CAST(0.00 AS Decimal(10, 2)), N'0', 1, 1)
GO
INSERT [BCES].[RebuiltParts] ([RebuiltPartID], [RebuiltStockCode], [Description], [Keyword], [JobNumber], [CoreCode], [CoreCost], [SOPNumber], [BusSeriesID], [PartTypeID]) VALUES (8, N'SW1016A', N'HYDRAULIC LINES', N'SWIS', N'92580', N'023417', CAST(0.00 AS Decimal(10, 2)), N'0', 1, 1)
GO
INSERT [BCES].[RebuiltParts] ([RebuiltPartID], [RebuiltStockCode], [Description], [Keyword], [JobNumber], [CoreCode], [CoreCost], [SOPNumber], [BusSeriesID], [PartTypeID]) VALUES (9, N'SW0730', N'ANTI-SCRATCHITTI CLEAR WINDOW FILM', N'SWIS', N'92581', N'023418', CAST(0.00 AS Decimal(10, 2)), N'0', 1, 1)
GO
INSERT [BCES].[RebuiltParts] ([RebuiltPartID], [RebuiltStockCode], [Description], [Keyword], [JobNumber], [CoreCode], [CoreCost], [SOPNumber], [BusSeriesID], [PartTypeID]) VALUES (10, N'SW1102', N'FRONT SUNSHADE END BRACKET UPGRADE', N'SWIS', N'92582', N'023419', CAST(0.00 AS Decimal(10, 2)), N'0', 1, 1)
GO
INSERT [BCES].[RebuiltParts] ([RebuiltPartID], [RebuiltStockCode], [Description], [Keyword], [JobNumber], [CoreCode], [CoreCost], [SOPNumber], [BusSeriesID], [PartTypeID]) VALUES (11, N'SWE1046', N'STARTER MOTOR UPDATE', N'SWIS', N'00092575', N'012547', CAST(0.00 AS Decimal(10, 2)), N'0', 1, 1)
GO
SET IDENTITY_INSERT [BCES].[RebuiltParts] OFF
GO
SET IDENTITY_INSERT [BCES].[StockCodedParts] ON 
GO
INSERT [BCES].[StockCodedParts] ([StockPartID], [RebuiltPartID], [MMSStockCode], [Description], [Keyword], [Quantity], [CoreCost], [CostCentreID]) VALUES (1, 1, N'012300', N'Engine Cooling Fan', N'COOLING', 20, CAST(100.00 AS Decimal(10, 2)), 1)
GO
INSERT [BCES].[StockCodedParts] ([StockPartID], [RebuiltPartID], [MMSStockCode], [Description], [Keyword], [Quantity], [CoreCost], [CostCentreID]) VALUES (2, 2, N'054788', N'Transmission Belt', N'BELT', 15, CAST(80.00 AS Decimal(10, 2)), 2)
GO
INSERT [BCES].[StockCodedParts] ([StockPartID], [RebuiltPartID], [MMSStockCode], [Description], [Keyword], [Quantity], [CoreCost], [CostCentreID]) VALUES (3, 3, N'988321', N'Brake Caliper', N'BRAKE', 30, CAST(150.00 AS Decimal(10, 2)), 3)
GO
INSERT [BCES].[StockCodedParts] ([StockPartID], [RebuiltPartID], [MMSStockCode], [Description], [Keyword], [Quantity], [CoreCost], [CostCentreID]) VALUES (4, 4, N'125680', N'Suspension Spring', N'SPRING', 25, CAST(90.00 AS Decimal(10, 2)), 4)
GO
INSERT [BCES].[StockCodedParts] ([StockPartID], [RebuiltPartID], [MMSStockCode], [Description], [Keyword], [Quantity], [CoreCost], [CostCentreID]) VALUES (5, 5, N'012546', N'Alternator Assembly', N'ELECTRICAL', 10, CAST(200.00 AS Decimal(10, 2)), 3)
GO
INSERT [BCES].[StockCodedParts] ([StockPartID], [RebuiltPartID], [MMSStockCode], [Description], [Keyword], [Quantity], [CoreCost], [CostCentreID]) VALUES (6, 1, N'098765', N'Exhaust Manifold', N'EXHAUST', 12, CAST(300.00 AS Decimal(10, 2)), 1)
GO
INSERT [BCES].[StockCodedParts] ([StockPartID], [RebuiltPartID], [MMSStockCode], [Description], [Keyword], [Quantity], [CoreCost], [CostCentreID]) VALUES (7, 1, N'024894', N'EGR AND VNT LINE UPGRADE', N'SWIS', 100, CAST(0.00 AS Decimal(10, 2)), 1)
GO
INSERT [BCES].[StockCodedParts] ([StockPartID], [RebuiltPartID], [MMSStockCode], [Description], [Keyword], [Quantity], [CoreCost], [CostCentreID]) VALUES (8, 2, N'002255', N'HYDRAULIC HOSE AND CLAMP REPLACEMENT', N'SWIS', 100, CAST(0.00 AS Decimal(10, 2)), 1)
GO
INSERT [BCES].[StockCodedParts] ([StockPartID], [RebuiltPartID], [MMSStockCode], [Description], [Keyword], [Quantity], [CoreCost], [CostCentreID]) VALUES (9, 3, N'002233', N'HYDRAULIC LINES', N'SWIS', 100, CAST(0.00 AS Decimal(10, 2)), 1)
GO
INSERT [BCES].[StockCodedParts] ([StockPartID], [RebuiltPartID], [MMSStockCode], [Description], [Keyword], [Quantity], [CoreCost], [CostCentreID]) VALUES (10, 4, N'654321', N'ANTI-SCRATCHITTI CLEAR WINDOW FILM', N'SWIS', 100, CAST(0.00 AS Decimal(10, 2)), 1)
GO
INSERT [BCES].[StockCodedParts] ([StockPartID], [RebuiltPartID], [MMSStockCode], [Description], [Keyword], [Quantity], [CoreCost], [CostCentreID]) VALUES (11, 5, N'654324', N'FRONT SUNSHADE END BRACKET UPGRADE', N'SWIS', 100, CAST(0.00 AS Decimal(10, 2)), 1)
GO
INSERT [BCES].[StockCodedParts] ([StockPartID], [RebuiltPartID], [MMSStockCode], [Description], [Keyword], [Quantity], [CoreCost], [CostCentreID]) VALUES (12, 6, N'012546', N'STARTER MOTOR UPDATE', N'SWIS', 100, CAST(0.00 AS Decimal(10, 2)), 1)
GO
SET IDENTITY_INSERT [BCES].[StockCodedParts] OFF
GO
SET IDENTITY_INSERT [BCES].[Suppliers] ON 
GO
INSERT [BCES].[Suppliers] ([SupplierID], [SupplierName], [SupplierCode], [SupplierContact]) VALUES (1, N'TTC', N'TTC01', N'contact@ttc.com')
GO
INSERT [BCES].[Suppliers] ([SupplierID], [SupplierName], [SupplierCode], [SupplierContact]) VALUES (2, N'QSTRAINT', N'QS01', N'support@qstraint.com')
GO
INSERT [BCES].[Suppliers] ([SupplierID], [SupplierName], [SupplierCode], [SupplierContact]) VALUES (3, N'DISA', N'DS01', N'info@disa.com')
GO
INSERT [BCES].[Suppliers] ([SupplierID], [SupplierName], [SupplierCode], [SupplierContact]) VALUES (4, N'MERCEDES SUPPLY', N'MS01', N'parts@mercedes.com')
GO
INSERT [BCES].[Suppliers] ([SupplierID], [SupplierName], [SupplierCode], [SupplierContact]) VALUES (5, N'VOLVO PARTS', N'VP01', N'volvo@parts.com')
GO
SET IDENTITY_INSERT [BCES].[Suppliers] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__BusSerie__452174B38F74E1C1]    Script Date: 2024-10-29 5:10:46 PM ******/
ALTER TABLE [BCES].[BusSeries] ADD UNIQUE NONCLUSTERED 
(
	[BusSeriesCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__CostCent__29707BE769FA984C]    Script Date: 2024-10-29 5:10:46 PM ******/
ALTER TABLE [BCES].[CostCentres] ADD UNIQUE NONCLUSTERED 
(
	[CostCentreCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__PartType__E8493D2A1DF9B3BC]    Script Date: 2024-10-29 5:10:46 PM ******/
ALTER TABLE [BCES].[PartTypes] ADD UNIQUE NONCLUSTERED 
(
	[PartTypeName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__Supplier__44BE981BD65E226B]    Script Date: 2024-10-29 5:10:46 PM ******/
ALTER TABLE [BCES].[Suppliers] ADD UNIQUE NONCLUSTERED 
(
	[SupplierCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
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
