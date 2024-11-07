-- Engines Table
INSERT INTO SCES.Engines (Name) VALUES ('Engine1');
INSERT INTO SCES.Engines (Name) VALUES ('Engine2');
GO

-- Transmissions Table
INSERT INTO SCES.Transmissions (Name) VALUES ('Transmission1');
INSERT INTO SCES.Transmissions (Name) VALUES ('Transmission2');
GO

-- CostCenters Table
INSERT INTO SCES.CostCenters (CostCentre, CostCentreName, DateEntered) VALUES ('07894', 'Center1', '2023-01-01');
INSERT INTO SCES.CostCenters (CostCentre, CostCentreName, DateEntered) VALUES ('07895', 'Center2', '2023-02-01');
GO

-- EmplClass Table
INSERT INTO SCES.EmplClass (LabourType, LabourDefn, WageGroup, HrsPerWeek, DateEntered, OverheadType) VALUES (1, 'LabourDefn1', 101, 40.0, '2023-01-01', 1.5);
INSERT INTO SCES.EmplClass (LabourType, LabourDefn, WageGroup, HrsPerWeek, DateEntered, OverheadType) VALUES (2, 'LabourDefn2', 102, 35.0, '2023-02-01', 2.0);
GO

-- VehicleList Table
INSERT INTO SCES.VehicleList (VehicleListId, VehSeriesCode, NumOfVehicles, ProjDesc, DateEntered, EnteredBy, ModifiedLastBy, ModifiedLastDate, Make, Model, Year, Engine, Transmission, Differential, SopNumber) 
VALUES (1, 'Series1', '10', 'Project1', '2023-01-01', 'User1', 'User1', '2023-01-01', 'Make1', 'Model1', '2023', 'Engine1', 'Transmission1', 'Diff1', 'SOP1');
INSERT INTO SCES.VehicleList (VehicleListId, VehSeriesCode, NumOfVehicles, ProjDesc, DateEntered, EnteredBy, ModifiedLastBy, ModifiedLastDate, Make, Model, Year, Engine, Transmission, Differential, SopNumber) 
VALUES (2, 'Series2', '5', 'Project2', '2023-02-01', 'User2', 'User2', '2023-02-01', 'Make2', 'Model2', '2023', 'Engine2', 'Transmission2', 'Diff2', 'SOP2');
GO

-- RbMasterlist Table
INSERT INTO SCES.RbMasterlist (MmsStockCode, ItemRefNumber, DetailedDesc, Keyword, CorePartNum, RebuiltStockNum, CoreCharge, JobNumber, DateModified, LastModifiedBy, EstimatedCost, SopNumber, BuyNewCost, RemanCost, ExternalCost, DateConverted, Active) 
VALUES ('MMS1', 'Ref1', 'Description1', 'Keyword1', 'Core1', 'Rebuilt1', '100', 'Job1', '2023-01-01', 'User1', 500.00, 'SOP1', '200', '150', '100', '2023-01-01', 1);
INSERT INTO SCES.RbMasterlist (MmsStockCode, ItemRefNumber, DetailedDesc, Keyword, CorePartNum, RebuiltStockNum, CoreCharge, JobNumber, DateModified, LastModifiedBy, EstimatedCost, SopNumber, BuyNewCost, RemanCost, ExternalCost, DateConverted, Active) 
VALUES ('MMS2', 'Ref2', 'Description2', 'Keyword2', 'Core2', 'Rebuilt2', '150', 'Job2', '2023-02-01', 'User2', 600.00, 'SOP2', '250', '200', '150', '2023-02-01', 1);
GO

-- RbListOfBuses Table
INSERT INTO SCES.RbListOfBuses (RebuiltStockNum, VehicleListId) VALUES ('Rebuilt1', 1);
INSERT INTO SCES.RbListOfBuses (RebuiltStockNum, VehicleListId) VALUES ('Rebuilt2', 2);
GO

-- ScPartsUsed Table
INSERT INTO SCES.ScPartsUsed (MmsStockCode, DateEntered, RebPartCost, UserEntered, MmsCost, OemCost, LastModifiedBy, LastModifiedDate, MmsRebuiltCode, CostCentre, QtyReqd, PercentUsage, LinkCode, RebuiltPart, LinkType, CoreCost, OrigSuppNum, OrigSupplierName) 
VALUES ('MMS1', '2023-01-01', 50.00, 'User1', 100.00, 150.00, 'User1', '2023-01-01', 'MMS1', '07894', '10', '50', '1', 'Y', 'T1', 50.00, 'Supp1', 'Supplier1');
INSERT INTO SCES.ScPartsUsed (MmsStockCode, DateEntered, RebPartCost, UserEntered, MmsCost, OemCost, LastModifiedBy, LastModifiedDate, MmsRebuiltCode, CostCentre, QtyReqd, PercentUsage, LinkCode, RebuiltPart, LinkType, CoreCost, OrigSuppNum, OrigSupplierName) 
VALUES ('MMS2', '2023-02-01', 60.00, 'User2', 120.00, 180.00, 'User2', '2023-02-01', 'MMS2', '07895', '5', '60', '2', 'N', 'T2', 60.00, 'Supp2', 'Supplier2');
GO

-- StockCodedParts Table
INSERT INTO SCES.StockCodedParts (PartNo, CorePartNumber, PartType, JobNumber, DetailedDesc, DateEntered, ItemRefNumber, OverheadType, CoreCharge, PartCost, LastModifiedDate, LastModifiedBy, AddedBy, MmsNewCost, MmsSyncDate, OrigSupplierNum, OrigSupplierName) 
VALUES ('030248', 'Core1', 'Type1', 'Job1', 'Description1', '1', 'Ref1', 1.5, 100, 500, '2023-01-01', 'User1', 'User1', 1000.00, '2023-01-01', 'Supp1', 'Supplier1');
INSERT INTO SCES.StockCodedParts (PartNo, CorePartNumber, PartType, JobNumber, DetailedDesc, DateEntered, ItemRefNumber, OverheadType, CoreCharge, PartCost, LastModifiedDate, LastModifiedBy, AddedBy, MmsNewCost, MmsSyncDate, OrigSupplierNum, OrigSupplierName) 
VALUES ('030249', 'Core2', 'Type2', 'Job2', 'Description2', '', 'Ref2', 2.0, 150, 600, '2023-02-01', 'User2', 'User2', 1200.00, '2023-02-01', 'Supp2', 'Supplier2');
GO

-- ScOemkitsTextData Table
INSERT INTO SCES.ScOemkitsTextData (PartNo, TextData) VALUES ('030248', 'TextData1');
INSERT INTO SCES.ScOemkitsTextData (PartNo, TextData) VALUES ('030249', 'TextData2');
GO

-- NscPartsUsed Table
INSERT INTO SCES.NscPartsUsed (OrigSuppNum, OrigSupplierName, CostCentre, QtyReqd, PercentUsage, DateEntered, EnteredBy, LastModifiedBy, LastModifiedDate, LinkCode, Cost, LinkType, CoreCost) 
VALUES ('Supp1', 'Supplier1', '07894', '10', '50', '2023-01-01', 'User1', 'User1', '2023-01-01', 'Link1', 50.00, 'T1', 50.00);
INSERT INTO SCES.NscPartsUsed (OrigSuppNum, OrigSupplierName, CostCentre, QtyReqd, PercentUsage, DateEntered, EnteredBy, LastModifiedBy, LastModifiedDate, LinkCode, Cost, LinkType, CoreCost) 
VALUES ('Supp2', 'Supplier2', '07895', '5', '60', '2023-02-01', 'User2', 'User2', '2023-02-01', 'Link2', 60.00, 'T2', 60.00);
GO

-- KitsUsed Table
INSERT INTO SCES.KitsUsed (PartNo, DateEntered, UserEntered, LastModifiedBy, LastModifiedDate, CostCentre, LinkCode) 
VALUES ('03024', '2023-01-01', 'User1', 'User1', '2023-01-01', '07894', 'Link1');
INSERT INTO SCES.KitsUsed (PartNo, DateEntered, UserEntered, LastModifiedBy, LastModifiedDate, CostCentre, LinkCode) 
VALUES ('03024', '2023-02-01', 'User2', 'User2', '2023-02-01', '07895', 'Link2');
GO

-- KitsMasterlist Table
INSERT INTO SCES.KitsMasterlist (PartNo, EstimatedCost, Keyword, DetailedDesc, ModifiedLastBy, ModifiedLastDate, DateEntered) 
VALUES ('030248', 500.00, 'Keyword1', 'Description1', 'User1', '2023-01-01', '2023-01-01');
INSERT INTO SCES.KitsMasterlist (PartNo, EstimatedCost, Keyword, DetailedDesc, ModifiedLastBy, ModifiedLastDate, DateEntered) 
VALUES ('030249', 600.00, 'Keyword2', 'Description2', 'User2', '2023-02-01', '2023-02-01');
GO

-- EmployeeLabour Table
INSERT INTO SCES.EmployeeLabour (LabourDefn, DateEntered, LinkNumber, TypeId, CostCentre, Task, LabourType, Usage, HrsReqd, AdjHrs, DateRevised, TimeAddition, RebuiltPartNum, LastModifiedBy) 
VALUES ('LabourDefn1', '2023-01-01', 'Link1', 'Type1', '07894', 'Task1', 'Type1', 'Usage1', '40', '40', '2023-01-01', 10.00, 'Part1', 'User1');
INSERT INTO SCES.EmployeeLabour (LabourDefn, DateEntered, LinkNumber, TypeId, CostCentre, Task, LabourType, Usage, HrsReqd, AdjHrs, DateRevised, TimeAddition, RebuiltPartNum, LastModifiedBy) 
VALUES ('LabourDefn2', '2023-02-01', 'Link2', 'Type2', '07895', 'Task2', 'Type2', 'Usage2', '35', '35', '2023-02-01', 15.00, 'Part2', 'User2');
GO

-- Linking Tables

-- RbMasterlistVehicleList Linking Table
INSERT INTO SCES.RbMasterlistVehicleList (MmsStockCode, VehicleListId) VALUES ('MMS1', 1);
INSERT INTO SCES.RbMasterlistVehicleList (MmsStockCode, VehicleListId) VALUES ('MMS2', 2);
GO

-- RbMasterlistEngines Linking Table
INSERT INTO SCES.RbMasterlistEngines (MmsStockCode, EngineName) VALUES ('MMS1', 'Engine1');
INSERT INTO SCES.RbMasterlistEngines (MmsStockCode, EngineName) VALUES ('MMS2', 'Engine2');
GO

-- RbMasterlistTransmissions Linking Table
INSERT INTO SCES.RbMasterlistTransmissions (MmsStockCode, TransmissionName) VALUES ('MMS1', 'Transmission1');
INSERT INTO SCES.RbMasterlistTransmissions (MmsStockCode, TransmissionName) VALUES ('MMS2', 'Transmission2');
GO

-- RbMasterlistCostCenters Linking Table
INSERT INTO SCES.RbMasterlistCostCenters (MmsStockCode, CostCentre) VALUES ('MMS1', '07894');
INSERT INTO SCES.RbMasterlistCostCenters (MmsStockCode, CostCentre) VALUES ('MMS2', '07895');
GO

-- EmployeeLabourEmplClass Linking Table
INSERT INTO SCES.EmployeeLabourEmplClass (EmployeeLabourId, EmplClassId) VALUES (1, 1);
INSERT INTO SCES.EmployeeLabourEmplClass (EmployeeLabourId, EmplClassId) VALUES (2, 2);
GO

-- MbListOfBuses Table
INSERT INTO SCES.MbListOfBuses (MbNumber, VehicleListId) VALUES ('Mb1', 1);
INSERT INTO SCES.MbListOfBuses (MbNumber, VehicleListId) VALUES ('Mb2', 2);
GO

-- RbListOfBuses Table
INSERT INTO SCES.RbListOfBuses (RebuiltStockNum, VehicleListId) VALUES ('Rebuilt1', 1);
INSERT INTO SCES.RbListOfBuses (RebuiltStockNum, VehicleListId) VALUES ('Rebuilt2', 2);
GO

-- Linking Tables

-- MbListOfBusesListOfBuses Linking Table
INSERT INTO SCES.MbListOfBusesListOfBuses (MbListOfBusesId, VehicleListId) VALUES (1, 1);
INSERT INTO SCES.MbListOfBusesListOfBuses (MbListOfBusesId, VehicleListId) VALUES (2, 2);
GO

-- RbListOfBusesListOfBuses Linking Table
INSERT INTO SCES.RbListOfBusesListOfBuses (RbListOfBusesId, VehicleListId) VALUES (1, 1);
INSERT INTO SCES.RbListOfBusesListOfBuses (RbListOfBusesId, VehicleListId) VALUES (2, 2);
GO