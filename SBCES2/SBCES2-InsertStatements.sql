-- Insert statements for static tables

-- Insert into SBCES.Engine
INSERT INTO SBCES.Engine (Name) VALUES ('S50DDEC III');
INSERT INTO SBCES.Engine (Name) VALUES ('M11');
INSERT INTO SBCES.Engine (Name) VALUES ('S50 EGR');

-- Insert into SBCES.Transmission
INSERT INTO SBCES.Transmission (Name) VALUES ('B400R(M)');
INSERT INTO SBCES.Transmission (Name) VALUES ('B500');
INSERT INTO SBCES.Transmission (Name) VALUES ('BAe');

-- Insert into SBCES.CostCenter
INSERT INTO SBCES.CostCenter (CostCentre) VALUES ('05h7');
INSERT INTO SBCES.CostCenter (CostCentre) VALUES ('05h3');
INSERT INTO SBCES.CostCenter (CostCentre) VALUES ('05h4');

-- Insert into SBCES.EmplClass
INSERT INTO SBCES.EmplClass (LabourType, LabourDefn, OverheadType) VALUES (1, 'Coach technician', 1.00);
INSERT INTO SBCES.EmplClass (LabourType, LabourDefn, OverheadType) VALUES (2, 'Body repair person', 1.00);

-- Insert into SBCES.LabourTaskDescriptions
INSERT INTO SBCES.LabourTaskDescriptions (TaskDescription) VALUES ('SWS work');
INSERT INTO SBCES.LabourTaskDescriptions (TaskDescription) VALUES ('Mechanical service');
INSERT INTO SBCES.LabourTaskDescriptions (TaskDescription) VALUES ('coach technician');
INSERT INTO SBCES.LabourTaskDescriptions (TaskDescription) VALUES ('break drum replacement');

-- Insert into SBCES.VehicleList
INSERT INTO SBCES.VehicleList (VehicleListId, VehSeriesCode, NumOfVehicles, ProjDesc, Make, Model, Year, Engine, Transmission, Differential)
VALUES (1, 'ORIONS DSL LE (7000-1734)', '10', 'MIDLIFE REBUILD', 'NOVA', '5', '2006', 'S50DDEC III', 'B400R(M)', '5.63.1');
INSERT INTO SBCES.VehicleList (VehicleListId, VehSeriesCode, NumOfVehicles, ProjDesc, Make, Model, Year, Engine, Transmission, Differential)
VALUES (2, 'SRT', '5', 'BULKHEAD REPAIR', 'ORION', '6', '2008', 'M11', 'B500', '5.13.1');
INSERT INTO SBCES.VehicleList (VehicleListId, VehSeriesCode, NumOfVehicles, ProjDesc, Make, Model, Year, Engine, Transmission, Differential)
VALUES (3, 'STREET CAR ALVR', '3', '4W BREAK RELINE NEW FLYER', 'NEW FLYER', '7', '2009', 'S50 EGR', 'BAe', '5.38.1');

-- Insert into SBCES.RbMasterlist
INSERT INTO SBCES.RbMasterlist ( MmsStockCode, DetailedDesc, Keyword, CorePartNum, RebuiltStockNum)
VALUES ('035433', 'Fule tak 1308 NG Hybrid', 'Engine', '012366', 'SW1001');
INSERT INTO SBCES.RbMasterlist ( MmsStockCode, DetailedDesc, Keyword, CorePartNum, RebuiltStockNum)
VALUES ('01233', 'Stub axle coe', 'SWS', '012388', 'SW 1003');
INSERT INTO SBCES.RbMasterlist ( MmsStockCode, DetailedDesc, Keyword, CorePartNum, RebuiltStockNum)
VALUES ('054786', 'Egr and vnt LINE upgrade', 'Mirror', '056432', 'SW1004');

-- Insert into SBCES.RbListOfBuses
INSERT INTO SBCES.RbListOfBuses (RebuiltStockNum, VehicleListId) VALUES ('SW1001', 1);
INSERT INTO SBCES.RbListOfBuses (RebuiltStockNum, VehicleListId) VALUES ('SW 1003', 2);
INSERT INTO SBCES.RbListOfBuses (RebuiltStockNum, VehicleListId) VALUES ('SW1004', 3);

-- Insert into SBCES.ScPartsUsed
INSERT INTO SBCES.ScPartsUsed (RbMasterlistId, CostCentre, OrigSuppNum, OrigSupplierName) VALUES (1, '05h7', '112002', 'PANLTE');
INSERT INTO SBCES.ScPartsUsed (RbMasterlistId, CostCentre, OrigSuppNum, OrigSupplierName) VALUES (2, '05h3', '20405', 'BARA');
INSERT INTO SBCES.ScPartsUsed (RbMasterlistId, CostCentre, OrigSuppNum, OrigSupplierName) VALUES (3, '05h4', '112002', 'CANNON');

-- Insert into SBCES.StockCodedParts
INSERT INTO SBCES.StockCodedParts (PartNo, CorePartNumber, PartType, DetailedDesc, OrigSupplierNum, OrigSupplierName)
VALUES ('035433', '012366', 'Engine', 'Fule tak 1308 NG Hybrid', '01236', 'PANLTE');
INSERT INTO SBCES.StockCodedParts (PartNo, CorePartNumber, PartType, DetailedDesc, OrigSupplierNum, OrigSupplierName)
VALUES ('01233', '012388', 'SWS', 'Stub axle coe', '01235', 'BARA');
INSERT INTO SBCES.StockCodedParts (PartNo, CorePartNumber, PartType, DetailedDesc, OrigSupplierNum, OrigSupplierName)
VALUES ('054786', '056432', 'Mirror', 'Egr and vnt LINE upgrade', '09873', 'CANNON');

-- Insert into SBCES.ScOemkitsTextData
INSERT INTO SBCES.ScOemkitsTextData (PartNo, TextData) VALUES ('035433', 'Text data for 035433');
INSERT INTO SBCES.ScOemkitsTextData (PartNo, TextData) VALUES ('01233', 'Text data for 01233');
INSERT INTO SBCES.ScOemkitsTextData (PartNo, TextData) VALUES ('054786', 'Text data for 054786');

-- Insert into SBCES.NscPartsUsed
INSERT INTO SBCES.NscPartsUsed (CostCentre, OrigSuppNum, OrigSupplierName) VALUES ('05h7', '112002', 'PANLTE');
INSERT INTO SBCES.NscPartsUsed (CostCentre, OrigSuppNum, OrigSupplierName) VALUES ('05h3', '20405', 'BARA');
INSERT INTO SBCES.NscPartsUsed (CostCentre, OrigSuppNum, OrigSupplierName) VALUES ('05h4', '112002', 'CANNON');

-- Insert into SBCES.KitsMasterlist
INSERT INTO SBCES.KitsMasterlist (PartNo, Keyword, DetailedDesc) VALUES ('035433', 'Engine', 'Fule tak 1308 NG Hybrid');
INSERT INTO SBCES.KitsMasterlist (PartNo, Keyword, DetailedDesc) VALUES ('01233', 'SWS', 'Stub axle coe');
INSERT INTO SBCES.KitsMasterlist (PartNo, Keyword, DetailedDesc) VALUES ('054786', 'Mirror', 'Egr and vnt LINE upgrade');

-- Insert into SBCES.KitsUsed
INSERT INTO SBCES.KitsUsed (PartNo, CostCentre) VALUES ('035433', '05h7');
INSERT INTO SBCES.KitsUsed (PartNo, CostCentre) VALUES ('01233', '05h3');
INSERT INTO SBCES.KitsUsed (PartNo, CostCentre) VALUES ('054786', '05h4');

-- Insert into SBCES.EmployeeLabour
INSERT INTO SBCES.EmployeeLabour (LabourDefn, CostCentre, Task, LabourType, Usage, HrsReqd)
VALUES ('Coach technician', '05h7', 'SWS work', '1', '0.38', '100');
INSERT INTO SBCES.EmployeeLabour (LabourDefn, CostCentre, Task, LabourType, Usage, HrsReqd)
VALUES ('Body repair person', '05h3', 'Mechanical service', '2', '0.78', '100');

-- Insert into SBCES.RbMasterlistVehicleList
INSERT INTO SBCES.RbMasterlistVehicleList (RbMasterlistId, VehicleListId) VALUES (1, 1);
INSERT INTO SBCES.RbMasterlistVehicleList (RbMasterlistId, VehicleListId) VALUES (2, 2);
INSERT INTO SBCES.RbMasterlistVehicleList (RbMasterlistId, VehicleListId) VALUES (3, 3);

-- Insert into SBCES.RbMasterlistEngines
INSERT INTO SBCES.RbMasterlistEngines (RbMasterlistId, EngineName) VALUES (1, 'S50DDEC III');
INSERT INTO SBCES.RbMasterlistEngines (RbMasterlistId, EngineName) VALUES (2, 'M11');
INSERT INTO SBCES.RbMasterlistEngines (RbMasterlistId, EngineName) VALUES (3, 'S50 EGR');

-- Insert into SBCES.RbMasterlistTransmissions
INSERT INTO SBCES.RbMasterlistTransmissions (RbMasterlistId, TransmissionName) VALUES (1, 'B400R(M)');
INSERT INTO SBCES.RbMasterlistTransmissions (RbMasterlistId, TransmissionName) VALUES (2, 'B500');
INSERT INTO SBCES.RbMasterlistTransmissions (RbMasterlistId, TransmissionName) VALUES (3, 'BAE');

-- Insert into SBCES.RbMasterlistCostCenters
INSERT INTO SBCES.RbMasterlistCostCenters (RbMasterlistId, CostCentre) VALUES (1, '05h7');
INSERT INTO SBCES.RbMasterlistCostCenters (RbMasterlistId, CostCentre) VALUES (2, '05h3');
INSERT INTO SBCES.RbMasterlistCostCenters (RbMasterlistId, CostCentre) VALUES (3, '05h4');

-- Insert into SBCES.EmployeeLabourEmplClass
INSERT INTO SBCES.EmployeeLabourEmplClass (EmployeeLabourId, EmplClassId) VALUES (1, 1);
INSERT INTO SBCES.EmployeeLabourEmplClass (EmployeeLabourId, EmplClassId) VALUES (2, 2);

-- Insert into SBCES.MbListOfBuses
INSERT INTO SBCES.MbListOfBuses (MbNumber, VehicleListId) VALUES ('MB1', 1);
INSERT INTO SBCES.MbListOfBuses (MbNumber, VehicleListId) VALUES ('MB2', 2);
INSERT INTO SBCES.MbListOfBuses (MbNumber, VehicleListId) VALUES ('MB3', 3);

-- Insert into SBCES.MbListOfBusesListOfBuses
INSERT INTO SBCES.MbListOfBusesListOfBuses (MbListOfBusesId, VehicleListId) VALUES (1, 1);
INSERT INTO SBCES.MbListOfBusesListOfBuses (MbListOfBusesId, VehicleListId) VALUES (2, 2);
INSERT INTO SBCES.MbListOfBusesListOfBuses (MbListOfBusesId, VehicleListId) VALUES (3, 3);

-- Insert into SBCES.RbListOfBusesListOfBuses
INSERT INTO SBCES.RbListOfBusesListOfBuses (RbListOfBusesId, VehicleListId) VALUES (1, 1);
INSERT INTO SBCES.RbListOfBusesListOfBuses (RbListOfBusesId, VehicleListId) VALUES (2, 2);
INSERT INTO SBCES.RbListOfBusesListOfBuses (RbListOfBusesId, VehicleListId) VALUES (3, 3);