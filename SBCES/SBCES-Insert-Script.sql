-- Delete all data from the tables
DELETE FROM SBCES.EmployeeLabourEmplClass;
DELETE FROM SBCES.RbMasterlistCostCenters;
DELETE FROM SBCES.RbMasterlistTransmissions;
DELETE FROM SBCES.RbMasterlistEngines;
DELETE FROM SBCES.RbMasterlistVehicleList;
DELETE FROM SBCES.EmployeeLabour;
DELETE FROM SBCES.LabourTaskDescriptions;
DELETE FROM SBCES.KitsUsed;
DELETE FROM SBCES.KitsMasterlist;
DELETE FROM SBCES.NscPartsUsed;
DELETE FROM SBCES.ScOemkitsTextData;
DELETE FROM SBCES.StockCodedParts;
DELETE FROM SBCES.ScPartsUsed;
DELETE FROM SBCES.RbListOfBusesListOfBuses;
DELETE FROM SBCES.MbListOfBusesListOfBuses;
DELETE FROM SBCES.MbListOfBuses;
DELETE FROM SBCES.RbListOfBuses;
DELETE FROM SBCES.RbMasterlist;
DELETE FROM SBCES.VehicleList;
DELETE FROM SBCES.EmplClass;
DELETE FROM SBCES.CostCenter;
DELETE FROM SBCES.Transmission;
DELETE FROM SBCES.Engine;

-- Reseed identity columns
DBCC CHECKIDENT ('SBCES.RbListOfBuses', RESEED, 1);
DBCC CHECKIDENT ('SBCES.NscPartsUsed', RESEED, 1);
DBCC CHECKIDENT ('SBCES.EmployeeLabour', RESEED, 1);
DBCC CHECKIDENT ('SBCES.RbMasterlistVehicleList', RESEED, 1);
DBCC CHECKIDENT ('SBCES.RbMasterlistEngines', RESEED, 1);
DBCC CHECKIDENT ('SBCES.RbMasterlistTransmissions', RESEED, 1);
DBCC CHECKIDENT ('SBCES.RbMasterlistCostCenters', RESEED, 1);
DBCC CHECKIDENT ('SBCES.EmployeeLabourEmplClass', RESEED, 1);
DBCC CHECKIDENT ('SBCES.MbListOfBuses', RESEED, 1);
DBCC CHECKIDENT ('SBCES.MbListOfBusesListOfBuses', RESEED, 1);
DBCC CHECKIDENT ('SBCES.RbListOfBusesListOfBuses', RESEED, 1);

INSERT INTO SBCES.Engine (Name) VALUES ('S50DDEC III');
INSERT INTO SBCES.Engine (Name) VALUES ('M11');
INSERT INTO SBCES.Engine (Name) VALUES ('S50 EGR');
INSERT INTO SBCES.Engine (Name) VALUES ('S50DDEC III');

INSERT INTO SBCES.Transmission (Name) VALUES ('B400R(M)');
INSERT INTO SBCES.Transmission (Name) VALUES ('B500');
INSERT INTO SBCES.Transmission (Name) VALUES ('BAE');
INSERT INTO SBCES.Transmission (Name) VALUES ('BAE 800');

INSERT INTO SBCES.CostCenter (CostCentre, CostCentreName, DateEntered) VALUES ('05H7', 'COST CENTER 1', GETDATE());
INSERT INTO SBCES.CostCenter (CostCentre, CostCentreName, DateEntered) VALUES ('05H3', 'COST CENTER 2', GETDATE());
INSERT INTO SBCES.CostCenter (CostCentre, CostCentreName, DateEntered) VALUES ('05H4', 'COST CENTER 3', GETDATE());
INSERT INTO SBCES.CostCenter (CostCentre, CostCentreName, DateEntered) VALUES ('05H8', 'COST CENTER 1', GETDATE());
INSERT INTO SBCES.CostCenter (CostCentre, CostCentreName, DateEntered) VALUES ('05H9', 'COST CENTER 2', GETDATE());

INSERT INTO SBCES.EmplClass (LabourType, LabourDefn, WageGroup, HrsPerWeek, DateEntered, OverheadType) VALUES (1, 'COACH TECHNICIAN', 1, 40, GETDATE(), 1.00);
INSERT INTO SBCES.EmplClass (LabourType, LabourDefn, WageGroup, HrsPerWeek, DateEntered, OverheadType) VALUES (2, 'BODY REPAIR PERSON', 2, 40, GETDATE(), 1.00);
INSERT INTO SBCES.EmplClass (LabourType, LabourDefn, WageGroup, HrsPerWeek, DateEntered, OverheadType) VALUES (3, 'MECHANICAL SERVICE', 3, 40, GETDATE(), 1.00);
INSERT INTO SBCES.EmplClass (LabourType, LabourDefn, WageGroup, HrsPerWeek, DateEntered, OverheadType) VALUES (4, 'COACH TECHNICIAN', 1, 40, GETDATE(), 1.00);
INSERT INTO SBCES.EmplClass (LabourType, LabourDefn, WageGroup, HrsPerWeek, DateEntered, OverheadType) VALUES (5, 'BODY REPAIR PERSON', 2, 40, GETDATE(), 1.00);

INSERT INTO SBCES.VehicleList (VehicleListId, VehSeriesCode, NumOfVehicles, ProjDesc, DateEntered, EnteredBy, ModifiedLastBy, ModifiedLastDate, Make, Model, Year, Engine, Transmission, Differential, SopNumber)
VALUES (1, 'ORIONS DSL LE (7000-1734)', '10', 'MIDLIFE REBUILD', GETDATE(), 'ADMIN', 'ADMIN', GETDATE(), 'NOVA', '5', '2006', 'S50DDEC III', 'B400R(M)', '5.63.1', 'SOP123');

INSERT INTO SBCES.VehicleList (VehicleListId, VehSeriesCode, NumOfVehicles, ProjDesc, DateEntered, EnteredBy, ModifiedLastBy, ModifiedLastDate, Make, Model, Year, Engine, Transmission, Differential, SopNumber)
VALUES (2, 'SRT', '5', 'BULKHEAD REPAIR', GETDATE(), 'ADMIN', 'ADMIN', GETDATE(), 'ORION', '6', '2008', 'M11', 'B500', '5.13.1', 'SOP456');

INSERT INTO SBCES.VehicleList (VehicleListId, VehSeriesCode, NumOfVehicles, ProjDesc, DateEntered, EnteredBy, ModifiedLastBy, ModifiedLastDate, Make, Model, Year, Engine, Transmission, Differential, SopNumber)
VALUES (3, 'STREET CAR ALVR', '8', '4W BREAK RELINE NEW FLYER', GETDATE(), 'ADMIN', 'ADMIN', GETDATE(), 'NEW FLYER', '7', '2009', 'S50 EGR', 'BAe', '5.38.1', 'SOP789');

INSERT INTO SBCES.VehicleList (VehicleListId, VehSeriesCode, NumOfVehicles, ProjDesc, DateEntered, EnteredBy, ModifiedLastBy, ModifiedLastDate, Make, Model, Year, Engine, Transmission, Differential, SopNumber)
VALUES (4, 'SUBWAY 11', '12', 'MIDLIFE REBUILD', GETDATE(), 'ADMIN', 'ADMIN', GETDATE(), 'NOVA', '5', '2006', 'S50DDEC III', 'B400R(M)', '5.63.1', 'SOP123');

INSERT INTO SBCES.VehicleList (VehicleListId, VehSeriesCode, NumOfVehicles, ProjDesc, DateEntered, EnteredBy, ModifiedLastBy, ModifiedLastDate, Make, Model, Year, Engine, Transmission, Differential, SopNumber)
VALUES (5, 'ORIONS DSL LE (7000-1734)', '10', 'BULKHEAD REPAIR', GETDATE(), 'ADMIN', 'ADMIN', GETDATE(), 'ORION', '6', '2008', 'M11', 'B500', '5.13.1', 'SOP456');

--
INSERT INTO SBCES.RbMasterlist (MmsStockCode, ItemRefNumber, DetailedDesc, Keyword, CorePartNum, RebuiltStockNum, CoreCharge, JobNumber, DateModified, LastModifiedBy, EstimatedCost, SopNumber, BuyNewCost, RemanCost, ExternalCost, DateConverted, Active)
VALUES ('035433', 'REF123', 'Fule tak 1308 NG Hybrid', 'ENGINE', '012366', 'SW1001', '100', 'JOB123', GETDATE(), 'ADMIN', 500.00, 'SOP123', '200', '300', '400', GETDATE(), 1);

INSERT INTO SBCES.RbMasterlist (MmsStockCode, ItemRefNumber, DetailedDesc, Keyword, CorePartNum, RebuiltStockNum, CoreCharge, JobNumber, DateModified, LastModifiedBy, EstimatedCost, SopNumber, BuyNewCost, RemanCost, ExternalCost, DateConverted, Active)
VALUES ('01233', 'REF456', 'Stub axle coe', 'SWS', '012388', 'SW1003', '150', 'JOB456', GETDATE(), 'ADMIN', 600.00, 'SOP456', '250', '350', '450', GETDATE(), 1);

INSERT INTO SBCES.RbMasterlist (MmsStockCode, ItemRefNumber, DetailedDesc, Keyword, CorePartNum, RebuiltStockNum, CoreCharge, JobNumber, DateModified, LastModifiedBy, EstimatedCost, SopNumber, BuyNewCost, RemanCost, ExternalCost, DateConverted, Active)
VALUES ('054786', 'REF789', 'Egr and vnt LINE upgrade', 'MIRROR', '056432', 'SW1004', '200', 'JOB789', GETDATE(), 'ADMIN', 700.00, 'SOP789', '300', '400', '500', GETDATE(), 1);

--
INSERT INTO SBCES.RbListOfBuses (RebuiltStockNum, VehicleListId) VALUES ('SW1001', 1);
INSERT INTO SBCES.RbListOfBuses (RebuiltStockNum, VehicleListId) VALUES ('SW1003', 2);
INSERT INTO SBCES.RbListOfBuses (RebuiltStockNum, VehicleListId) VALUES ('SW1004', 3);
INSERT INTO SBCES.RbListOfBuses (RebuiltStockNum, VehicleListId) VALUES ('SW1001', 4);
INSERT INTO SBCES.RbListOfBuses (RebuiltStockNum, VehicleListId) VALUES ('SW1003', 5);
------------------------------
INSERT INTO SBCES.ScPartsUsed (MmsStockCode, DateEntered, RebPartCost, UserEntered, MmsCost, OemCost, LastModifiedBy, LastModifiedDate, MmsRebuiltCode, CostCentre, QtyReqd, PercentUsage, LinkCode, RebuiltPart, LinkType, CoreCost, OrigSuppNum, OrigSupplierName)
VALUES ('035433', GETDATE(), 100.00, 'ADMIN', 50.00, 75.00, 'ADMIN', GETDATE(), '035433', '05H7', '10', '100', '035433', 'Y', 'L1', 25.00, '012366', 'SUPPLIER 1');

INSERT INTO SBCES.ScPartsUsed (MmsStockCode, DateEntered, RebPartCost, UserEntered, MmsCost, OemCost, LastModifiedBy, LastModifiedDate, MmsRebuiltCode, CostCentre, QtyReqd, PercentUsage, LinkCode, RebuiltPart, LinkType, CoreCost, OrigSuppNum, OrigSupplierName)
VALUES ('01233', GETDATE(), 150.00, 'ADMIN', 75.00, 100.00, 'ADMIN', GETDATE(), '01233', '05H3', '5', '50', '01233', 'N', 'L2', 30.00, '012388', 'SUPPLIER 2');

INSERT INTO SBCES.ScPartsUsed (MmsStockCode, DateEntered, RebPartCost, UserEntered, MmsCost, OemCost, LastModifiedBy, LastModifiedDate, MmsRebuiltCode, CostCentre, QtyReqd, PercentUsage, LinkCode, RebuiltPart, LinkType, CoreCost, OrigSuppNum, OrigSupplierName)
VALUES ('054786', GETDATE(), 200.00, 'ADMIN', 100.00, 125.00, 'ADMIN', GETDATE(), '054786', '05H4', '8', '75', '054786', 'Y', 'L3', 35.00, '056432', 'SUPPLIER 3');

INSERT INTO SBCES.ScPartsUsed (MmsStockCode, DateEntered, RebPartCost, UserEntered, MmsCost, OemCost, LastModifiedBy, LastModifiedDate, MmsRebuiltCode, CostCentre, QtyReqd, PercentUsage, LinkCode, RebuiltPart, LinkType, CoreCost, OrigSuppNum, OrigSupplierName)
VALUES ('035433', GETDATE(), 100.00, 'ADMIN', 50.00, 75.00, 'ADMIN', GETDATE(), '035433', '05H7', '10', '100', '035433', 'Y', 'L1', 25.00, '012366', 'SUPPLIER 1');

INSERT INTO SBCES.ScPartsUsed (MmsStockCode, DateEntered, RebPartCost, UserEntered, MmsCost, OemCost, LastModifiedBy, LastModifiedDate, MmsRebuiltCode, CostCentre, QtyReqd, PercentUsage, LinkCode, RebuiltPart, LinkType, CoreCost, OrigSuppNum, OrigSupplierName)
VALUES ('01233', GETDATE(), 150.00, 'ADMIN', 75.00, 100.00, 'ADMIN', GETDATE(), '01233', '05H3', '5', '50', '01233', 'N', 'L2', 30.00, '012388', 'SUPPLIER 2');


-------------------------------------------
INSERT INTO SBCES.ScPartsUsed (MmsStockCode, DateEntered, RebPartCost, UserEntered, MmsCost, OemCost, LastModifiedBy, LastModifiedDate, MmsRebuiltCode, CostCentre, QtyReqd, PercentUsage, LinkCode, RebuiltPart, LinkType, CoreCost, OrigSuppNum, OrigSupplierName)
VALUES ('035433', GETDATE(), 100.00, 'ADMIN', 50.00, 75.00, 'ADMIN', GETDATE(), '035433', '05H7', '10', '100', '035433', 'Y', 'L1', 25.00, '035433', 'SUPPLIER 1');

INSERT INTO SBCES.ScPartsUsed (MmsStockCode, DateEntered, RebPartCost, UserEntered, MmsCost, OemCost, LastModifiedBy, LastModifiedDate, MmsRebuiltCode, CostCentre, QtyReqd, PercentUsage, LinkCode, RebuiltPart, LinkType, CoreCost, OrigSuppNum, OrigSupplierName)
VALUES ('01233', GETDATE(), 150.00, 'ADMIN', 75.00, 100.00, 'ADMIN', GETDATE(), '01233', '05H3', '5', '50', '01233', 'N', 'L2', 30.00, '01233', 'SUPPLIER 2');

INSERT INTO SBCES.ScPartsUsed (MmsStockCode, DateEntered, RebPartCost, UserEntered, MmsCost, OemCost, LastModifiedBy, LastModifiedDate, MmsRebuiltCode, CostCentre, QtyReqd, PercentUsage, LinkCode, RebuiltPart, LinkType, CoreCost, OrigSuppNum, OrigSupplierName)
VALUES ('054786', GETDATE(), 200.00, 'ADMIN', 100.00, 125.00, 'ADMIN', GETDATE(), '054786', '05H4', '8', '75', '054786', 'Y', 'L3', 35.00, '054786', 'SUPPLIER 3');

----

INSERT INTO SBCES.StockCodedParts (PartNo, CorePartNumber, PartType, JobNumber, DetailedDesc, DateEntered, ItemRefNumber, OverheadType, CoreCharge, PartCost, LastModifiedDate, LastModifiedBy, AddedBy, MmsNewCost, MmsSyncDate, OrigSupplierNum, OrigSupplierName)
VALUES ('PART001', 'CORE001', 'ENGINE', 'JOB123', 'Fule tak 1308 NG Hybrid', GETDATE(), 'REF123', 1, 100, 500, GETDATE(), 'ADMIN', 'ADMIN', 200.00, GETDATE(), 'SUPP001', 'SUPPLIER 1');

INSERT INTO SBCES.StockCodedParts (PartNo, CorePartNumber, PartType, JobNumber, DetailedDesc, DateEntered, ItemRefNumber, OverheadType, CoreCharge, PartCost, LastModifiedDate, LastModifiedBy, AddedBy, MmsNewCost, MmsSyncDate, OrigSupplierNum, OrigSupplierName)
VALUES ('PART002', 'CORE002', 'SWS', 'JOB456', 'Stub axle coe', GETDATE(), 'REF456', 2, 150, 600, GETDATE(), 'ADMIN', 'ADMIN', 250.00, GETDATE(), 'SUPP002', 'SUPPLIER 2');

INSERT INTO SBCES.StockCodedParts (PartNo, CorePartNumber, PartType, JobNumber, DetailedDesc, DateEntered, ItemRefNumber, OverheadType, CoreCharge, PartCost, LastModifiedDate, LastModifiedBy, AddedBy, MmsNewCost, MmsSyncDate, OrigSupplierNum, OrigSupplierName)
VALUES ('PART003', 'CORE003', 'MIRROR', 'JOB789', 'Egr and vnt LINE upgrade', GETDATE(), 'REF789', 3, 200, 700, GETDATE(), 'ADMIN', 'ADMIN', 300.00, GETDATE(), 'SUPP003', 'SUPPLIER 3');

INSERT INTO SBCES.StockCodedParts (PartNo, CorePartNumber, PartType, JobNumber, DetailedDesc, DateEntered, ItemRefNumber, OverheadType, CoreCharge, PartCost, LastModifiedDate, LastModifiedBy, AddedBy, MmsNewCost, MmsSyncDate, OrigSupplierNum, OrigSupplierName)
VALUES ('PART001', 'CORE001', 'ENGINE', 'JOB123', 'Fule tak 1308 NG Hybrid', GETDATE(), 'REF123', 1, 100, 500, GETDATE(), 'ADMIN', 'ADMIN', 200.00, GETDATE(), 'SUPP001', 'SUPPLIER 1');

INSERT INTO SBCES.StockCodedParts (PartNo, CorePartNumber, PartType, JobNumber, DetailedDesc, DateEntered, ItemRefNumber, OverheadType, CoreCharge, PartCost, LastModifiedDate, LastModifiedBy, AddedBy, MmsNewCost, MmsSyncDate, OrigSupplierNum, OrigSupplierName)
VALUES ('PART002', 'CORE002', 'SWS', 'JOB456', 'Stub axle coe', GETDATE(), 'REF456', 2, 150, 600, GETDATE(), 'ADMIN', 'ADMIN', 250.00, GETDATE(), 'SUPP002', 'SUPPLIER 2');
---------------
-----
INSERT INTO SBCES.ScOemkitsTextData (PartNo, TextData) VALUES ('PART001', 'TEXT DATA 1');
INSERT INTO SBCES.ScOemkitsTextData (PartNo, TextData) VALUES ('PART002', 'TEXT DATA 2');
INSERT INTO SBCES.ScOemkitsTextData (PartNo, TextData) VALUES ('PART003', 'TEXT DATA 3');
INSERT INTO SBCES.ScOemkitsTextData (PartNo, TextData) VALUES ('PART001', 'TEXT DATA 1');
INSERT INTO SBCES.ScOemkitsTextData (PartNo, TextData) VALUES ('PART002', 'TEXT DATA 2');
---
INSERT INTO SBCES.NscPartsUsed (OrigSuppNum, OrigSupplierName, CostCentre, QtyReqd, PercentUsage, DateEntered, EnteredBy, LastModifiedBy, LastModifiedDate, LinkCode, Cost, LinkType, CoreCost)
VALUES ('SUPP001', 'SUPPLIER 1', '05H7', '10', '100', GETDATE(), 'ADMIN', 'ADMIN', GETDATE(), 'LINK001', 100.00, 'L1', 25.00);

INSERT INTO SBCES.NscPartsUsed (OrigSuppNum, OrigSupplierName, CostCentre, QtyReqd, PercentUsage, DateEntered, EnteredBy, LastModifiedBy, LastModifiedDate, LinkCode, Cost, LinkType, CoreCost)
VALUES ('SUPP002', 'SUPPLIER 2', '05H3', '5', '50', GETDATE(), 'ADMIN', 'ADMIN', GETDATE(), 'LINK002', 150.00, 'L2', 30.00);

INSERT INTO SBCES.NscPartsUsed (OrigSuppNum, OrigSupplierName, CostCentre, QtyReqd, PercentUsage, DateEntered, EnteredBy, LastModifiedBy, LastModifiedDate, LinkCode, Cost, LinkType, CoreCost)
VALUES ('SUPP003', 'SUPPLIER 3', '05H4', '8', '75', GETDATE(), 'ADMIN', 'ADMIN', GETDATE(), 'LINK003', 200.00, 'L3', 35.00);

INSERT INTO SBCES.NscPartsUsed (OrigSuppNum, OrigSupplierName, CostCentre, QtyReqd, PercentUsage, DateEntered, EnteredBy, LastModifiedBy, LastModifiedDate, LinkCode, Cost, LinkType, CoreCost)
VALUES ('SUPP001', 'SUPPLIER 1', '05H7', '10', '100', GETDATE(), 'ADMIN', 'ADMIN', GETDATE(), 'LINK001', 100.00, 'L1', 25.00);

INSERT INTO SBCES.NscPartsUsed (OrigSuppNum, OrigSupplierName, CostCentre, QtyReqd, PercentUsage, DateEntered, EnteredBy, LastModifiedBy, LastModifiedDate, LinkCode, Cost, LinkType, CoreCost)
VALUES ('SUPP002', 'SUPPLIER 2', '05H3', '5', '50', GETDATE(), 'ADMIN', 'ADMIN', GETDATE(), 'LINK002', 150.00, 'L2', 30.00);
-------------------========================================================

--------
INSERT INTO SBCES.KitsMasterlist (PartNo, EstimatedCost, Keyword, DetailedDesc, ModifiedLastBy, ModifiedLastDate, DateEntered)
VALUES ('KIT001', 500.00, 'ENGINE', 'Fule tak 1308 NG Hybrid', 'ADMIN', GETDATE(), GETDATE());

INSERT INTO SBCES.KitsMasterlist (PartNo, EstimatedCost, Keyword, DetailedDesc, ModifiedLastBy, ModifiedLastDate, DateEntered)
VALUES ('KIT002', 600.00, 'SWS', 'Stub axle coe', 'ADMIN', GETDATE(), GETDATE());

INSERT INTO SBCES.KitsMasterlist (PartNo, EstimatedCost, Keyword, DetailedDesc, ModifiedLastBy, ModifiedLastDate, DateEntered)
VALUES ('KIT003', 700.00, 'MIRROR', 'Egr and vnt LINE upgrade', 'ADMIN', GETDATE(), GETDATE());

INSERT INTO SBCES.KitsMasterlist (PartNo, EstimatedCost, Keyword, DetailedDesc, ModifiedLastBy, ModifiedLastDate, DateEntered)
VALUES ('KIT001', 500.00, 'ENGINE', 'Fule tak 1308 NG Hybrid', 'ADMIN', GETDATE(), GETDATE());

INSERT INTO SBCES.KitsMasterlist (PartNo, EstimatedCost, Keyword, DetailedDesc, ModifiedLastBy, ModifiedLastDate, DateEntered)
VALUES ('KIT002', 600.00, 'SWS', 'Stub axle coe', 'ADMIN', GETDATE(), GETDATE());
---------
INSERT INTO SBCES.KitsUsed (PartNo, DateEntered, UserEntered, LastModifiedBy, LastModifiedDate, CostCentre, LinkCode)
VALUES ('KIT001', GETDATE(), 'ADMIN', 'ADMIN', GETDATE(), '05H7', 'LINK001');

INSERT INTO SBCES.KitsUsed (PartNo, DateEntered, UserEntered, LastModifiedBy, LastModifiedDate, CostCentre, LinkCode)
VALUES ('KIT002', GETDATE(), 'ADMIN', 'ADMIN', GETDATE(), '05H3', 'LINK002');

INSERT INTO SBCES.KitsUsed (PartNo, DateEntered, UserEntered, LastModifiedBy, LastModifiedDate, CostCentre, LinkCode)
VALUES ('KIT003', GETDATE(), 'ADMIN', 'ADMIN', GETDATE(), '05H4', 'LINK003');

INSERT INTO SBCES.KitsUsed (PartNo, DateEntered, UserEntered, LastModifiedBy, LastModifiedDate, CostCentre, LinkCode)
VALUES ('KIT001', GETDATE(), 'ADMIN', 'ADMIN', GETDATE(), '05H7', 'LINK001');

INSERT INTO SBCES.KitsUsed (PartNo, DateEntered, UserEntered, LastModifiedBy, LastModifiedDate, CostCentre, LinkCode)
VALUES ('KIT002', GETDATE(), 'ADMIN', 'ADMIN', GETDATE(), '05H3', 'LINK002');
-----------
INSERT INTO SBCES.LabourTaskDescriptions (TaskDescription) VALUES ('SWS WORK');
INSERT INTO SBCES.LabourTaskDescriptions (TaskDescription) VALUES ('MECHANICAL SERVICE');
INSERT INTO SBCES.LabourTaskDescriptions (TaskDescription) VALUES ('COACH TECHNICIAN');
INSERT INTO SBCES.LabourTaskDescriptions (TaskDescription) VALUES ('SWS WORK');
INSERT INTO SBCES.LabourTaskDescriptions (TaskDescription) VALUES ('MECHANICAL SERVICE');
----------
INSERT INTO SBCES.EmployeeLabour (LabourDefn, DateEntered, LinkNumber, TypeId, CostCentre, Task, LabourType, Usage, HrsReqd, AdjHrs, DateRevised, TimeAddition, RebuiltPartNum, LastModifiedBy)
VALUES ('COACH TECHNICIAN', GETDATE(), 'LINK001', 'TYPE1', '05H7', 'SWS WORK', 'LT1', 'USAGE1', '0.38', '0.38', GETDATE(), 10.00, 'SW1001', 'ADMIN');

INSERT INTO SBCES.EmployeeLabour (LabourDefn, DateEntered, LinkNumber, TypeId, CostCentre, Task, LabourType, Usage, HrsReqd, AdjHrs, DateRevised, TimeAddition, RebuiltPartNum, LastModifiedBy)
VALUES ('BODY REPAIR PERSON', GETDATE(), 'LINK002', 'TYPE2', '05H3', 'MECHANICAL SERVICE', 'LT2', 'USAGE2', '0.78', '0.78', GETDATE(), 20.00, 'SW1003', 'ADMIN');

INSERT INTO SBCES.EmployeeLabour (LabourDefn, DateEntered, LinkNumber, TypeId, CostCentre, Task, LabourType, Usage, HrsReqd, AdjHrs, DateRevised, TimeAddition, RebuiltPartNum, LastModifiedBy)
VALUES ('MECHANICAL SERVICE', GETDATE(), 'LINK003', 'TYPE3', '05H4', 'COACH TECHNICIAN', 'LT3', 'USAGE3', '100', '100', GETDATE(), 30.00, 'SW1004', 'ADMIN');

INSERT INTO SBCES.EmployeeLabour (LabourDefn, DateEntered, LinkNumber, TypeId, CostCentre, Task, LabourType, Usage, HrsReqd, AdjHrs, DateRevised, TimeAddition, RebuiltPartNum, LastModifiedBy)
VALUES ('COACH TECHNICIAN', GETDATE(), 'LINK001', 'TYPE1', '05H7', 'SWS WORK', 'LT1', 'USAGE1', '0.38', '0.38', GETDATE(), 10.00, 'SW1001', 'ADMIN');

INSERT INTO SBCES.EmployeeLabour (LabourDefn, DateEntered, LinkNumber, TypeId, CostCentre, Task, LabourType, Usage, HrsReqd, AdjHrs, DateRevised, TimeAddition, RebuiltPartNum, LastModifiedBy)
VALUES ('BODY REPAIR PERSON', GETDATE(), 'LINK002', 'TYPE2', '05H3', 'MECHANICAL SERVICE', 'LT2', 'USAGE2', '0.78', '0.78', GETDATE(), 20.00, 'SW1003', 'ADMIN');
----------
INSERT INTO SBCES.RbMasterlistVehicleList (MmsStockCode, VehicleListId) VALUES ('035433', 1);
INSERT INTO SBCES.RbMasterlistVehicleList (MmsStockCode, VehicleListId) VALUES ('01233', 2);
INSERT INTO SBCES.RbMasterlistVehicleList (MmsStockCode, VehicleListId) VALUES ('054786', 3);
INSERT INTO SBCES.RbMasterlistVehicleList (MmsStockCode, VehicleListId) VALUES ('035433', 4);
INSERT INTO SBCES.RbMasterlistVehicleList (MmsStockCode, VehicleListId) VALUES ('01233', 5);
---------
INSERT INTO SBCES.RbMasterlistEngines (MmsStockCode, EngineName) VALUES ('035433', 'S50DDEC III');
INSERT INTO SBCES.RbMasterlistEngines (MmsStockCode, EngineName) VALUES ('01233', 'M11');
INSERT INTO SBCES.RbMasterlistEngines (MmsStockCode, EngineName) VALUES ('054786', 'S50 EGR');
INSERT INTO SBCES.RbMasterlistEngines (MmsStockCode, EngineName) VALUES ('035433', 'S50DDEC III');
INSERT INTO SBCES.RbMasterlistEngines (MmsStockCode, EngineName) VALUES ('01233', 'M11');
------
INSERT INTO SBCES.RbMasterlistTransmissions (MmsStockCode, TransmissionName) VALUES ('035433', 'B400R(M)');
INSERT INTO SBCES.RbMasterlistTransmissions (MmsStockCode, TransmissionName) VALUES ('01233', 'B500');
INSERT INTO SBCES.RbMasterlistTransmissions (MmsStockCode, TransmissionName) VALUES ('054786', 'BAE');
INSERT INTO SBCES.RbMasterlistTransmissions (MmsStockCode, TransmissionName) VALUES ('035433', 'B400R(M)');
INSERT INTO SBCES.RbMasterlistTransmissions (MmsStockCode, TransmissionName) VALUES ('01233', 'B500');
-----
INSERT INTO SBCES.RbMasterlistCostCenters (MmsStockCode, CostCentre) VALUES ('035433', '05H7');
INSERT INTO SBCES.RbMasterlistCostCenters (MmsStockCode, CostCentre) VALUES ('01233', '05H3');
INSERT INTO SBCES.RbMasterlistCostCenters (MmsStockCode, CostCentre) VALUES ('054786', '05H4');

