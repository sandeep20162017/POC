USE [SBCES]
GO
INSERT [SCES].[Engines] ([Name]) VALUES (N'M11')
GO
INSERT [SCES].[Engines] ([Name]) VALUES (N'S50 EGR')
GO
INSERT [SCES].[Engines] ([Name]) VALUES (N'S50DDEC III')
GO
INSERT [SCES].[Transmissions] ([Name]) VALUES (N'B400R(M)')
GO
INSERT [SCES].[Transmissions] ([Name]) VALUES (N'B500')
GO
INSERT [SCES].[Transmissions] ([Name]) VALUES (N'BAe')
GO
INSERT [SCES].[VehicleList] ([VehicleListId], [VehSeriesCode], [NumOfVehicles], [ProjDesc], [DateEntered], [EnteredBy], [ModifiedLastBy], [ModifiedLastDate], [Make], [Model], [Year], [Engine], [Transmission], [Differential], [SopNumber]) VALUES (CAST(1 AS Numeric(38, 0)), N'ORIONS DSL LE (7000-1734)', N'10', N'MIDLIFE REBUILD', CAST(N'2024-11-11T11:23:33.190' AS DateTime), N'ADMIN', N'ADMIN', CAST(N'2024-11-11T11:23:33.190' AS DateTime), N'NOVA', N'5', N'2006', N'S50DDEC III', N'B400R(M)', N'5.63.1', NULL)
GO
INSERT [SCES].[VehicleList] ([VehicleListId], [VehSeriesCode], [NumOfVehicles], [ProjDesc], [DateEntered], [EnteredBy], [ModifiedLastBy], [ModifiedLastDate], [Make], [Model], [Year], [Engine], [Transmission], [Differential], [SopNumber]) VALUES (CAST(2 AS Numeric(38, 0)), N'SRT', N'5', N'BULKHEAD REPAIR', CAST(N'2024-11-11T11:23:33.193' AS DateTime), N'ADMIN', N'ADMIN', CAST(N'2024-11-11T11:23:33.193' AS DateTime), N'ORION', N'6', N'2008', N'M11', N'B500', N'5.13.1', NULL)
GO
INSERT [SCES].[VehicleList] ([VehicleListId], [VehSeriesCode], [NumOfVehicles], [ProjDesc], [DateEntered], [EnteredBy], [ModifiedLastBy], [ModifiedLastDate], [Make], [Model], [Year], [Engine], [Transmission], [Differential], [SopNumber]) VALUES (CAST(3 AS Numeric(38, 0)), N'STREET CAR ALVR', N'15', N'4W BREAK RELINE NEW FLYER', CAST(N'2024-11-11T13:20:42.493' AS DateTime), N'ADMIN', N'ADMIN', CAST(N'2024-11-11T13:20:42.493' AS DateTime), N'NEW FLYER', N'7', N'2009', N'S50 EGR', N'BAe', N'5.38.1', NULL)
GO
INSERT [SCES].[VehicleList] ([VehicleListId], [VehSeriesCode], [NumOfVehicles], [ProjDesc], [DateEntered], [EnteredBy], [ModifiedLastBy], [ModifiedLastDate], [Make], [Model], [Year], [Engine], [Transmission], [Differential], [SopNumber]) VALUES (CAST(4 AS Numeric(38, 0)), N'SUBWAY 11', N'20', N'MIDLIFE REBUILD', CAST(N'2024-11-11T13:20:42.493' AS DateTime), N'ADMIN', N'ADMIN', CAST(N'2024-11-11T13:20:42.493' AS DateTime), N'ORION', N'6', N'2008', N'M11', N'B500', N'5.13.1', NULL)
GO
INSERT [SCES].[VehicleList] ([VehicleListId], [VehSeriesCode], [NumOfVehicles], [ProjDesc], [DateEntered], [EnteredBy], [ModifiedLastBy], [ModifiedLastDate], [Make], [Model], [Year], [Engine], [Transmission], [Differential], [SopNumber]) VALUES (CAST(5 AS Numeric(38, 0)), N'ORIONS DSL LE (7000-1734)', N'25', N'BULKHEAD REPAIR', CAST(N'2024-11-11T13:20:42.493' AS DateTime), N'ADMIN', N'ADMIN', CAST(N'2024-11-11T13:20:42.493' AS DateTime), N'NOVA', N'5', N'2006', N'S50DDEC III', N'B400R(M)', N'5.63.1', NULL)
GO
INSERT [SCES].[VehicleList] ([VehicleListId], [VehSeriesCode], [NumOfVehicles], [ProjDesc], [DateEntered], [EnteredBy], [ModifiedLastBy], [ModifiedLastDate], [Make], [Model], [Year], [Engine], [Transmission], [Differential], [SopNumber]) VALUES (CAST(6 AS Numeric(38, 0)), N'SRT', N'30', N'MIDLIFE REBUILD', CAST(N'2024-11-11T13:20:42.493' AS DateTime), N'ADMIN', N'ADMIN', CAST(N'2024-11-11T13:20:42.493' AS DateTime), N'NEW FLYER', N'7', N'2009', N'S50 EGR', N'BAe', N'5.38.1', NULL)
GO
INSERT [SCES].[VehicleList] ([VehicleListId], [VehSeriesCode], [NumOfVehicles], [ProjDesc], [DateEntered], [EnteredBy], [ModifiedLastBy], [ModifiedLastDate], [Make], [Model], [Year], [Engine], [Transmission], [Differential], [SopNumber]) VALUES (CAST(7 AS Numeric(38, 0)), N'STREET CAR ALVR', N'35', N'4W BREAK RELINE NEW FLYER', CAST(N'2024-11-11T13:20:42.493' AS DateTime), N'ADMIN', N'ADMIN', CAST(N'2024-11-11T13:20:42.493' AS DateTime), N'ORION', N'6', N'2008', N'M11', N'B500', N'5.13.1', NULL)
GO
INSERT [SCES].[RbMasterlist] ([MmsStockCode], [ItemRefNumber], [DetailedDesc], [Keyword], [CorePartNum], [RebuiltStockNum], [CoreCharge], [JobNumber], [DateModified], [LastModifiedBy], [EstimatedCost], [SopNumber], [BuyNewCost], [RemanCost], [ExternalCost], [DateConverted], [Active]) VALUES (N'01233', N'SW1003', N'STUB AXLE COE', N'SWS', N'012388', N'SW1003', N'150', N'JOB2', CAST(N'2024-11-11T11:23:33.197' AS DateTime), N'ADMIN', CAST(600.00 AS Numeric(20, 2)), N'SOP2', N'600', N'500', N'700', CAST(N'2024-11-11T11:23:33.197' AS DateTime), CAST(1 AS Numeric(4, 0)))
GO
INSERT [SCES].[RbMasterlist] ([MmsStockCode], [ItemRefNumber], [DetailedDesc], [Keyword], [CorePartNum], [RebuiltStockNum], [CoreCharge], [JobNumber], [DateModified], [LastModifiedBy], [EstimatedCost], [SopNumber], [BuyNewCost], [RemanCost], [ExternalCost], [DateConverted], [Active]) VALUES (N'035433', N'SW1001', N'FULE TAK 1308 NG HYBRID', N'ENGINE', N'012366', N'SW1001', N'100', N'JOB1', CAST(N'2024-11-11T11:23:33.193' AS DateTime), N'ADMIN', CAST(500.00 AS Numeric(20, 2)), N'SOP1', N'500', N'400', N'600', CAST(N'2024-11-11T11:23:33.193' AS DateTime), CAST(1 AS Numeric(4, 0)))
GO
SET IDENTITY_INSERT [SCES].[RbMasterlistEngines] ON 
GO
INSERT [SCES].[RbMasterlistEngines] ([RbMasterlistEnginesId], [MmsStockCode], [EngineName]) VALUES (1, N'035433', N'S50DDEC III')
GO
INSERT [SCES].[RbMasterlistEngines] ([RbMasterlistEnginesId], [MmsStockCode], [EngineName]) VALUES (2, N'01233', N'M11')
GO
SET IDENTITY_INSERT [SCES].[RbMasterlistEngines] OFF
GO
SET IDENTITY_INSERT [SCES].[RbMasterlistTransmissions] ON 
GO
INSERT [SCES].[RbMasterlistTransmissions] ([RbMasterlistTransmissionsId], [MmsStockCode], [TransmissionName]) VALUES (1, N'035433', N'B400R(M)')
GO
INSERT [SCES].[RbMasterlistTransmissions] ([RbMasterlistTransmissionsId], [MmsStockCode], [TransmissionName]) VALUES (2, N'01233', N'B500')
GO
SET IDENTITY_INSERT [SCES].[RbMasterlistTransmissions] OFF
GO
INSERT [SCES].[CostCenters] ([CostCentre], [CostCentreName], [DateEntered]) VALUES (N'05H3', N'COST CENTER 2', CAST(N'2024-11-11T11:23:33.190' AS DateTime))
GO
INSERT [SCES].[CostCenters] ([CostCentre], [CostCentreName], [DateEntered]) VALUES (N'05H7', N'COST CENTER 1', CAST(N'2024-11-11T11:23:33.190' AS DateTime))
GO
INSERT [SCES].[ScPartsUsed] ([MmsStockCode], [DateEntered], [RebPartCost], [UserEntered], [MmsCost], [OemCost], [LastModifiedBy], [LastModifiedDate], [MmsRebuiltCode], [CostCentre], [QtyReqd], [PercentUsage], [LinkCode], [RebuiltPart], [LinkType], [CoreCost], [OrigSuppNum], [OrigSupplierName]) VALUES (N'01233', CAST(N'2024-11-11T11:23:33.200' AS DateTime), CAST(600.00 AS Numeric(18, 2)), N'ADMIN', CAST(600.00 AS Numeric(18, 2)), CAST(700.00 AS Numeric(18, 2)), N'ADMIN', CAST(N'2024-11-11T11:23:33.200' AS DateTime), N'01235', N'05H3', N'5', N'100', N'LINK2', N'Y', N'L2', CAST(150.00 AS Numeric(18, 2)), N'SUPP2', N'SUPPLIER 2')
GO
INSERT [SCES].[ScPartsUsed] ([MmsStockCode], [DateEntered], [RebPartCost], [UserEntered], [MmsCost], [OemCost], [LastModifiedBy], [LastModifiedDate], [MmsRebuiltCode], [CostCentre], [QtyReqd], [PercentUsage], [LinkCode], [RebuiltPart], [LinkType], [CoreCost], [OrigSuppNum], [OrigSupplierName]) VALUES (N'035433', CAST(N'2024-11-11T11:23:33.200' AS DateTime), CAST(500.00 AS Numeric(18, 2)), N'ADMIN', CAST(500.00 AS Numeric(18, 2)), CAST(600.00 AS Numeric(18, 2)), N'ADMIN', CAST(N'2024-11-11T11:23:33.200' AS DateTime), N'01236', N'05H7', N'10', N'100', N'LINK1', N'Y', N'L1', CAST(100.00 AS Numeric(18, 2)), N'SUPP1', N'SUPPLIER 1')
GO
SET IDENTITY_INSERT [SCES].[NscPartsUsed] ON 
GO
INSERT [SCES].[NscPartsUsed] ([NscPartsUsedId], [OrigSuppNum], [OrigSupplierName], [CostCentre], [QtyReqd], [PercentUsage], [DateEntered], [EnteredBy], [LastModifiedBy], [LastModifiedDate], [LinkCode], [Cost], [LinkType], [CoreCost], [Id]) VALUES (1, N'SUPP1', N'SUPPLIER 1', N'05H7', N'10', N'100', CAST(N'2024-11-11T11:23:33.203' AS DateTime), N'ADMIN', N'ADMIN', CAST(N'2024-11-11T11:23:33.203' AS DateTime), N'LINK1', CAST(500.00 AS Numeric(18, 2)), N'L1', CAST(100.00 AS Numeric(18, 2)), CAST(1 AS Numeric(10, 0)))
GO
INSERT [SCES].[NscPartsUsed] ([NscPartsUsedId], [OrigSuppNum], [OrigSupplierName], [CostCentre], [QtyReqd], [PercentUsage], [DateEntered], [EnteredBy], [LastModifiedBy], [LastModifiedDate], [LinkCode], [Cost], [LinkType], [CoreCost], [Id]) VALUES (2, N'SUPP2', N'SUPPLIER 2', N'05H3', N'5', N'100', CAST(N'2024-11-11T11:23:33.203' AS DateTime), N'ADMIN', N'ADMIN', CAST(N'2024-11-11T11:23:33.203' AS DateTime), N'LINK2', CAST(600.00 AS Numeric(18, 2)), N'L2', CAST(150.00 AS Numeric(18, 2)), CAST(2 AS Numeric(10, 0)))
GO
SET IDENTITY_INSERT [SCES].[NscPartsUsed] OFF
GO
INSERT [SCES].[KitsMasterlist] ([PartNo], [EstimatedCost], [Keyword], [DetailedDesc], [ModifiedLastBy], [ModifiedLastDate], [DateEntered]) VALUES (N'01233', CAST(600.00 AS Numeric(18, 2)), N'SWS', N'STUB AXLE COE', N'ADMIN', CAST(N'2024-11-11' AS Date), CAST(N'2024-11-11T11:23:33.207' AS DateTime))
GO
INSERT [SCES].[KitsMasterlist] ([PartNo], [EstimatedCost], [Keyword], [DetailedDesc], [ModifiedLastBy], [ModifiedLastDate], [DateEntered]) VALUES (N'035433', CAST(500.00 AS Numeric(18, 2)), N'ENGINE', N'FULE TAK 1308 NG HYBRID', N'ADMIN', CAST(N'2024-11-11' AS Date), CAST(N'2024-11-11T11:23:33.207' AS DateTime))
GO
INSERT [SCES].[KitsUsed] ([PartNo], [DateEntered], [UserEntered], [LastModifiedBy], [LastModifiedDate], [CostCentre], [LinkCode]) VALUES (N'01233', CAST(N'2024-11-11T11:23:33.210' AS DateTime), N'ADMIN', N'ADMIN', CAST(N'2024-11-11T11:23:33.210' AS DateTime), N'05H3', N'LINK2')
GO
INSERT [SCES].[KitsUsed] ([PartNo], [DateEntered], [UserEntered], [LastModifiedBy], [LastModifiedDate], [CostCentre], [LinkCode]) VALUES (N'035433', CAST(N'2024-11-11T11:23:33.207' AS DateTime), N'ADMIN', N'ADMIN', CAST(N'2024-11-11T11:23:33.207' AS DateTime), N'05H7', N'LINK1')
GO
INSERT [SCES].[LabourTaskDescriptions] ([TaskDescription]) VALUES (N'MECHANICAL SERVICE')
GO
INSERT [SCES].[LabourTaskDescriptions] ([TaskDescription]) VALUES (N'SWS WORK')
GO
SET IDENTITY_INSERT [SCES].[EmployeeLabour] ON 
GO
INSERT [SCES].[EmployeeLabour] ([Id], [LabourDefn], [DateEntered], [LinkNumber], [TypeId], [CostCentre], [Task], [LabourType], [Usage], [HrsReqd], [AdjHrs], [DateRevised], [TimeAddition], [RebuiltPartNum], [LastModifiedBy]) VALUES (1, N'COACH TECHNICIAN', CAST(N'2024-11-11T11:23:33.210' AS DateTime), N'LINK1', N'TYPE1', N'05H7', N'SWS WORK', N'LT1', N'USAGE1', N'0.38', N'0.38', CAST(N'2024-11-11' AS Date), CAST(10.00 AS Numeric(18, 2)), N'SW1001', N'ADMIN')
GO
INSERT [SCES].[EmployeeLabour] ([Id], [LabourDefn], [DateEntered], [LinkNumber], [TypeId], [CostCentre], [Task], [LabourType], [Usage], [HrsReqd], [AdjHrs], [DateRevised], [TimeAddition], [RebuiltPartNum], [LastModifiedBy]) VALUES (2, N'BODY REPAIR PERSON', CAST(N'2024-11-11T11:23:33.213' AS DateTime), N'LINK2', N'TYPE2', N'05H3', N'MECHANICAL SERVICE', N'LT2', N'USAGE2', N'0.78', N'0.78', CAST(N'2024-11-11' AS Date), CAST(20.00 AS Numeric(18, 2)), N'SW1003', N'ADMIN')
GO
SET IDENTITY_INSERT [SCES].[EmployeeLabour] OFF
GO
SET IDENTITY_INSERT [SCES].[RbMasterlistCostCenters] ON 
GO
INSERT [SCES].[RbMasterlistCostCenters] ([RbMasterlistCostCentersId], [MmsStockCode], [CostCentre]) VALUES (1, N'035433', N'05H7')
GO
INSERT [SCES].[RbMasterlistCostCenters] ([RbMasterlistCostCentersId], [MmsStockCode], [CostCentre]) VALUES (2, N'01233', N'05H3')
GO
SET IDENTITY_INSERT [SCES].[RbMasterlistCostCenters] OFF
GO
INSERT [SCES].[EmplClass] ([LabourType], [LabourDefn], [WageGroup], [HrsPerWeek], [DateEntered], [OverheadType]) VALUES (CAST(1 AS Numeric(10, 0)), N'COACH TECHNICIAN', CAST(1 AS Numeric(10, 0)), CAST(40.0000 AS Numeric(18, 4)), CAST(N'2024-11-11T11:23:33.190' AS DateTime), CAST(1.00 AS Numeric(5, 2)))
GO
INSERT [SCES].[EmplClass] ([LabourType], [LabourDefn], [WageGroup], [HrsPerWeek], [DateEntered], [OverheadType]) VALUES (CAST(2 AS Numeric(10, 0)), N'BODY REPAIR PERSON', CAST(2 AS Numeric(10, 0)), CAST(40.0000 AS Numeric(18, 4)), CAST(N'2024-11-11T11:23:33.190' AS DateTime), CAST(1.00 AS Numeric(5, 2)))
GO
INSERT [SCES].[EmplClass] ([LabourType], [LabourDefn], [WageGroup], [HrsPerWeek], [DateEntered], [OverheadType]) VALUES (CAST(3 AS Numeric(10, 0)), N'MECHANIC', CAST(3 AS Numeric(10, 0)), CAST(40.0000 AS Numeric(18, 4)), CAST(N'2024-11-11T13:20:42.490' AS DateTime), CAST(1.00 AS Numeric(5, 2)))
GO
INSERT [SCES].[EmplClass] ([LabourType], [LabourDefn], [WageGroup], [HrsPerWeek], [DateEntered], [OverheadType]) VALUES (CAST(4 AS Numeric(10, 0)), N'ELECTRICIAN', CAST(4 AS Numeric(10, 0)), CAST(40.0000 AS Numeric(18, 4)), CAST(N'2024-11-11T13:20:42.490' AS DateTime), CAST(1.00 AS Numeric(5, 2)))
GO
INSERT [SCES].[EmplClass] ([LabourType], [LabourDefn], [WageGroup], [HrsPerWeek], [DateEntered], [OverheadType]) VALUES (CAST(5 AS Numeric(10, 0)), N'PAINTER', CAST(5 AS Numeric(10, 0)), CAST(40.0000 AS Numeric(18, 4)), CAST(N'2024-11-11T13:20:42.490' AS DateTime), CAST(1.00 AS Numeric(5, 2)))
GO
INSERT [SCES].[EmplClass] ([LabourType], [LabourDefn], [WageGroup], [HrsPerWeek], [DateEntered], [OverheadType]) VALUES (CAST(6 AS Numeric(10, 0)), N'WELDER', CAST(6 AS Numeric(10, 0)), CAST(40.0000 AS Numeric(18, 4)), CAST(N'2024-11-11T13:20:42.490' AS DateTime), CAST(1.00 AS Numeric(5, 2)))
GO
INSERT [SCES].[EmplClass] ([LabourType], [LabourDefn], [WageGroup], [HrsPerWeek], [DateEntered], [OverheadType]) VALUES (CAST(7 AS Numeric(10, 0)), N'INSPECTOR', CAST(7 AS Numeric(10, 0)), CAST(40.0000 AS Numeric(18, 4)), CAST(N'2024-11-11T13:20:42.490' AS DateTime), CAST(1.00 AS Numeric(5, 2)))
GO
SET IDENTITY_INSERT [SCES].[EmployeeLabourEmplClass] ON 
GO
INSERT [SCES].[EmployeeLabourEmplClass] ([EmployeeLabourEmplClassId], [EmployeeLabourId], [EmplClassId]) VALUES (1, 1, CAST(1 AS Numeric(10, 0)))
GO
INSERT [SCES].[EmployeeLabourEmplClass] ([EmployeeLabourEmplClassId], [EmployeeLabourId], [EmplClassId]) VALUES (2, 2, CAST(2 AS Numeric(10, 0)))
GO
SET IDENTITY_INSERT [SCES].[EmployeeLabourEmplClass] OFF
GO
SET IDENTITY_INSERT [SCES].[RbListOfBuses] ON 
GO
INSERT [SCES].[RbListOfBuses] ([RbListOfBusesId], [RebuiltStockNum], [VehicleListId]) VALUES (1, N'SW1001', CAST(1 AS Numeric(38, 0)))
GO
INSERT [SCES].[RbListOfBuses] ([RbListOfBusesId], [RebuiltStockNum], [VehicleListId]) VALUES (2, N'SW1003', CAST(2 AS Numeric(38, 0)))
GO
INSERT [SCES].[RbListOfBuses] ([RbListOfBusesId], [RebuiltStockNum], [VehicleListId]) VALUES (3, N'SW1004', CAST(3 AS Numeric(38, 0)))
GO
INSERT [SCES].[RbListOfBuses] ([RbListOfBusesId], [RebuiltStockNum], [VehicleListId]) VALUES (4, N'SW1005', CAST(4 AS Numeric(38, 0)))
GO
INSERT [SCES].[RbListOfBuses] ([RbListOfBusesId], [RebuiltStockNum], [VehicleListId]) VALUES (5, N'SW1006', CAST(5 AS Numeric(38, 0)))
GO
INSERT [SCES].[RbListOfBuses] ([RbListOfBusesId], [RebuiltStockNum], [VehicleListId]) VALUES (6, N'SW1007', CAST(6 AS Numeric(38, 0)))
GO
INSERT [SCES].[RbListOfBuses] ([RbListOfBusesId], [RebuiltStockNum], [VehicleListId]) VALUES (7, N'SW1008', CAST(7 AS Numeric(38, 0)))
GO
INSERT [SCES].[RbListOfBuses] ([RbListOfBusesId], [RebuiltStockNum], [VehicleListId]) VALUES (8, N'SW1004', CAST(3 AS Numeric(38, 0)))
GO
INSERT [SCES].[RbListOfBuses] ([RbListOfBusesId], [RebuiltStockNum], [VehicleListId]) VALUES (9, N'SW1005', CAST(4 AS Numeric(38, 0)))
GO
INSERT [SCES].[RbListOfBuses] ([RbListOfBusesId], [RebuiltStockNum], [VehicleListId]) VALUES (10, N'SW1006', CAST(5 AS Numeric(38, 0)))
GO
INSERT [SCES].[RbListOfBuses] ([RbListOfBusesId], [RebuiltStockNum], [VehicleListId]) VALUES (11, N'SW1007', CAST(6 AS Numeric(38, 0)))
GO
INSERT [SCES].[RbListOfBuses] ([RbListOfBusesId], [RebuiltStockNum], [VehicleListId]) VALUES (12, N'SW1008', CAST(7 AS Numeric(38, 0)))
GO
SET IDENTITY_INSERT [SCES].[RbListOfBuses] OFF
GO
SET IDENTITY_INSERT [SCES].[RbMasterlistVehicleList] ON 
GO
INSERT [SCES].[RbMasterlistVehicleList] ([RbMasterlistVehicleListId], [MmsStockCode], [VehicleListId]) VALUES (1, N'035433', CAST(1 AS Numeric(38, 0)))
GO
INSERT [SCES].[RbMasterlistVehicleList] ([RbMasterlistVehicleListId], [MmsStockCode], [VehicleListId]) VALUES (2, N'01233', CAST(2 AS Numeric(38, 0)))
GO
SET IDENTITY_INSERT [SCES].[RbMasterlistVehicleList] OFF
GO
SET IDENTITY_INSERT [SCES].[MbListOfBuses] ON 
GO
INSERT [SCES].[MbListOfBuses] ([MbListOfBusesId], [MbNumber], [VehicleListId]) VALUES (1, N'MB1', CAST(1 AS Numeric(38, 0)))
GO
INSERT [SCES].[MbListOfBuses] ([MbListOfBusesId], [MbNumber], [VehicleListId]) VALUES (2, N'MB2', CAST(2 AS Numeric(38, 0)))
GO
INSERT [SCES].[MbListOfBuses] ([MbListOfBusesId], [MbNumber], [VehicleListId]) VALUES (3, N'MB3', CAST(3 AS Numeric(38, 0)))
GO
INSERT [SCES].[MbListOfBuses] ([MbListOfBusesId], [MbNumber], [VehicleListId]) VALUES (4, N'MB4', CAST(4 AS Numeric(38, 0)))
GO
INSERT [SCES].[MbListOfBuses] ([MbListOfBusesId], [MbNumber], [VehicleListId]) VALUES (5, N'MB5', CAST(5 AS Numeric(38, 0)))
GO
INSERT [SCES].[MbListOfBuses] ([MbListOfBusesId], [MbNumber], [VehicleListId]) VALUES (6, N'MB6', CAST(6 AS Numeric(38, 0)))
GO
INSERT [SCES].[MbListOfBuses] ([MbListOfBusesId], [MbNumber], [VehicleListId]) VALUES (7, N'MB7', CAST(7 AS Numeric(38, 0)))
GO
INSERT [SCES].[MbListOfBuses] ([MbListOfBusesId], [MbNumber], [VehicleListId]) VALUES (8, N'MB3', CAST(3 AS Numeric(38, 0)))
GO
INSERT [SCES].[MbListOfBuses] ([MbListOfBusesId], [MbNumber], [VehicleListId]) VALUES (9, N'MB4', CAST(4 AS Numeric(38, 0)))
GO
INSERT [SCES].[MbListOfBuses] ([MbListOfBusesId], [MbNumber], [VehicleListId]) VALUES (10, N'MB5', CAST(5 AS Numeric(38, 0)))
GO
INSERT [SCES].[MbListOfBuses] ([MbListOfBusesId], [MbNumber], [VehicleListId]) VALUES (11, N'MB6', CAST(6 AS Numeric(38, 0)))
GO
INSERT [SCES].[MbListOfBuses] ([MbListOfBusesId], [MbNumber], [VehicleListId]) VALUES (12, N'MB7', CAST(7 AS Numeric(38, 0)))
GO
SET IDENTITY_INSERT [SCES].[MbListOfBuses] OFF
GO
SET IDENTITY_INSERT [SCES].[MbListOfBusesListOfBuses] ON 
GO
INSERT [SCES].[MbListOfBusesListOfBuses] ([MbListOfBusesListOfBusesId], [MbListOfBusesId], [VehicleListId]) VALUES (1, 1, CAST(1 AS Numeric(38, 0)))
GO
INSERT [SCES].[MbListOfBusesListOfBuses] ([MbListOfBusesListOfBusesId], [MbListOfBusesId], [VehicleListId]) VALUES (2, 2, CAST(2 AS Numeric(38, 0)))
GO
INSERT [SCES].[MbListOfBusesListOfBuses] ([MbListOfBusesListOfBusesId], [MbListOfBusesId], [VehicleListId]) VALUES (3, 3, CAST(3 AS Numeric(38, 0)))
GO
INSERT [SCES].[MbListOfBusesListOfBuses] ([MbListOfBusesListOfBusesId], [MbListOfBusesId], [VehicleListId]) VALUES (4, 4, CAST(4 AS Numeric(38, 0)))
GO
INSERT [SCES].[MbListOfBusesListOfBuses] ([MbListOfBusesListOfBusesId], [MbListOfBusesId], [VehicleListId]) VALUES (5, 5, CAST(5 AS Numeric(38, 0)))
GO
INSERT [SCES].[MbListOfBusesListOfBuses] ([MbListOfBusesListOfBusesId], [MbListOfBusesId], [VehicleListId]) VALUES (6, 6, CAST(6 AS Numeric(38, 0)))
GO
INSERT [SCES].[MbListOfBusesListOfBuses] ([MbListOfBusesListOfBusesId], [MbListOfBusesId], [VehicleListId]) VALUES (7, 7, CAST(7 AS Numeric(38, 0)))
GO
INSERT [SCES].[MbListOfBusesListOfBuses] ([MbListOfBusesListOfBusesId], [MbListOfBusesId], [VehicleListId]) VALUES (8, 3, CAST(3 AS Numeric(38, 0)))
GO
INSERT [SCES].[MbListOfBusesListOfBuses] ([MbListOfBusesListOfBusesId], [MbListOfBusesId], [VehicleListId]) VALUES (9, 4, CAST(4 AS Numeric(38, 0)))
GO
INSERT [SCES].[MbListOfBusesListOfBuses] ([MbListOfBusesListOfBusesId], [MbListOfBusesId], [VehicleListId]) VALUES (10, 5, CAST(5 AS Numeric(38, 0)))
GO
INSERT [SCES].[MbListOfBusesListOfBuses] ([MbListOfBusesListOfBusesId], [MbListOfBusesId], [VehicleListId]) VALUES (11, 6, CAST(6 AS Numeric(38, 0)))
GO
INSERT [SCES].[MbListOfBusesListOfBuses] ([MbListOfBusesListOfBusesId], [MbListOfBusesId], [VehicleListId]) VALUES (12, 7, CAST(7 AS Numeric(38, 0)))
GO
SET IDENTITY_INSERT [SCES].[MbListOfBusesListOfBuses] OFF
GO
SET IDENTITY_INSERT [SCES].[RbListOfBusesListOfBuses] ON 
GO
INSERT [SCES].[RbListOfBusesListOfBuses] ([RbListOfBusesListOfBusesId], [RbListOfBusesId], [VehicleListId]) VALUES (1, 1, CAST(1 AS Numeric(38, 0)))
GO
INSERT [SCES].[RbListOfBusesListOfBuses] ([RbListOfBusesListOfBusesId], [RbListOfBusesId], [VehicleListId]) VALUES (2, 2, CAST(2 AS Numeric(38, 0)))
GO
INSERT [SCES].[RbListOfBusesListOfBuses] ([RbListOfBusesListOfBusesId], [RbListOfBusesId], [VehicleListId]) VALUES (3, 3, CAST(3 AS Numeric(38, 0)))
GO
INSERT [SCES].[RbListOfBusesListOfBuses] ([RbListOfBusesListOfBusesId], [RbListOfBusesId], [VehicleListId]) VALUES (4, 4, CAST(4 AS Numeric(38, 0)))
GO
INSERT [SCES].[RbListOfBusesListOfBuses] ([RbListOfBusesListOfBusesId], [RbListOfBusesId], [VehicleListId]) VALUES (5, 5, CAST(5 AS Numeric(38, 0)))
GO
INSERT [SCES].[RbListOfBusesListOfBuses] ([RbListOfBusesListOfBusesId], [RbListOfBusesId], [VehicleListId]) VALUES (6, 6, CAST(6 AS Numeric(38, 0)))
GO
INSERT [SCES].[RbListOfBusesListOfBuses] ([RbListOfBusesListOfBusesId], [RbListOfBusesId], [VehicleListId]) VALUES (7, 7, CAST(7 AS Numeric(38, 0)))
GO
INSERT [SCES].[RbListOfBusesListOfBuses] ([RbListOfBusesListOfBusesId], [RbListOfBusesId], [VehicleListId]) VALUES (8, 3, CAST(3 AS Numeric(38, 0)))
GO
INSERT [SCES].[RbListOfBusesListOfBuses] ([RbListOfBusesListOfBusesId], [RbListOfBusesId], [VehicleListId]) VALUES (9, 4, CAST(4 AS Numeric(38, 0)))
GO
INSERT [SCES].[RbListOfBusesListOfBuses] ([RbListOfBusesListOfBusesId], [RbListOfBusesId], [VehicleListId]) VALUES (10, 5, CAST(5 AS Numeric(38, 0)))
GO
INSERT [SCES].[RbListOfBusesListOfBuses] ([RbListOfBusesListOfBusesId], [RbListOfBusesId], [VehicleListId]) VALUES (11, 6, CAST(6 AS Numeric(38, 0)))
GO
INSERT [SCES].[RbListOfBusesListOfBuses] ([RbListOfBusesListOfBusesId], [RbListOfBusesId], [VehicleListId]) VALUES (12, 7, CAST(7 AS Numeric(38, 0)))
GO
SET IDENTITY_INSERT [SCES].[RbListOfBusesListOfBuses] OFF
GO
INSERT [SCES].[ScOemkitsTextData] ([PartNo], [TextData]) VALUES (N'01233', N'TEXT DATA 2')
GO
INSERT [SCES].[ScOemkitsTextData] ([PartNo], [TextData]) VALUES (N'035433', N'TEXT DATA 1')
GO
INSERT [SCES].[StockCodedParts] ([PartNo], [CorePartNumber], [PartType], [JobNumber], [DetailedDesc], [DateEntered], [ItemRefNumber], [OverheadType], [CoreCharge], [PartCost], [LastModifiedDate], [LastModifiedBy], [AddedBy], [MmsNewCost], [MmsSyncDate], [OrigSupplierNum], [OrigSupplierName]) VALUES (N'01233', N'012388', N'SWS', N'JOB2', N'STUB AXLE COE', N'20230101', N'SW1003', CAST(2 AS Numeric(9, 0)), CAST(150 AS Numeric(9, 0)), CAST(600 AS Numeric(9, 0)), CAST(N'2024-11-11T11:23:33.200' AS DateTime), N'ADMIN', N'ADMIN', CAST(600.00000 AS Numeric(15, 5)), CAST(N'2024-11-11T11:23:33.200' AS DateTime), N'SUPP2', N'SUPPLIER 2')
GO
INSERT [SCES].[StockCodedParts] ([PartNo], [CorePartNumber], [PartType], [JobNumber], [DetailedDesc], [DateEntered], [ItemRefNumber], [OverheadType], [CoreCharge], [PartCost], [LastModifiedDate], [LastModifiedBy], [AddedBy], [MmsNewCost], [MmsSyncDate], [OrigSupplierNum], [OrigSupplierName]) VALUES (N'035433', N'012366', N'ENGINE', N'JOB1', N'FULE TAK 1308 NG HYBRID', N'20230101', N'SW1001', CAST(1 AS Numeric(9, 0)), CAST(100 AS Numeric(9, 0)), CAST(500 AS Numeric(9, 0)), CAST(N'2024-11-11T11:23:33.200' AS DateTime), N'ADMIN', N'ADMIN', CAST(500.00000 AS Numeric(15, 5)), CAST(N'2024-11-11T11:23:33.200' AS DateTime), N'SUPP1', N'SUPPLIER 1')
GO
