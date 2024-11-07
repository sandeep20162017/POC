CREATE SCHEMA SCES;
GO

-- Static and Non-Dependent Tables

-- Engines Table
CREATE TABLE SCES.Engines (
    Name VARCHAR(25) NOT NULL,
    CONSTRAINT PK_Engines PRIMARY KEY CLUSTERED (Name ASC)
) ON [PRIMARY];
GO

-- Transmissions Table
CREATE TABLE SCES.Transmissions (
    Name VARCHAR(25) NOT NULL,
    CONSTRAINT PK_Transmissions PRIMARY KEY CLUSTERED (Name ASC)
) ON [PRIMARY];
GO

-- CostCenters Table
CREATE TABLE SCES.CostCenters (
    CostCentre VARCHAR(5) NOT NULL,
    CostCentreName VARCHAR(50) NULL,
    DateEntered DATETIME NULL,
    CONSTRAINT PK_CostCenters PRIMARY KEY CLUSTERED (CostCentre ASC)
) ON [PRIMARY];
GO

-- EmplClass Table
CREATE TABLE SCES.EmplClass (
    LabourType NUMERIC(10, 0) NOT NULL,
    LabourDefn VARCHAR(50) NULL,
    WageGroup NUMERIC(10, 0) NULL,
    HrsPerWeek NUMERIC(18, 4) NULL,
    DateEntered DATETIME NULL,
    OverheadType NUMERIC(5, 2) NOT NULL,
    CONSTRAINT PK_EmplClass PRIMARY KEY CLUSTERED (LabourType ASC)
) ON [PRIMARY];
GO

-- VehicleList Table
CREATE TABLE SCES.VehicleList (
    VehicleListId NUMERIC(38, 0) NOT NULL,
    VehSeriesCode VARCHAR(500) NOT NULL,
    NumOfVehicles VARCHAR(10) NOT NULL,
    ProjDesc VARCHAR(200) NOT NULL,
    DateEntered DATETIME NOT NULL,
    EnteredBy VARCHAR(25) NOT NULL,
    ModifiedLastBy VARCHAR(25) NOT NULL,
    ModifiedLastDate DATETIME NOT NULL,
    Make VARCHAR(25) NULL,
    Model VARCHAR(25) NULL,
    Year VARCHAR(4) NULL,
    Engine VARCHAR(25) NULL,
    Transmission VARCHAR(25) NULL,
    Differential VARCHAR(25) NULL,
    SopNumber VARCHAR(30) NULL,
    CONSTRAINT PK_VehicleList PRIMARY KEY CLUSTERED (VehicleListId ASC),
    CONSTRAINT FK_VehicleList_Engines FOREIGN KEY (Engine) REFERENCES SCES.Engines (Name),
    CONSTRAINT FK_VehicleList_Transmissions FOREIGN KEY (Transmission) REFERENCES SCES.Transmissions (Name)
) ON [PRIMARY];
GO

-- Dependent Tables

-- RbMasterlist Table
CREATE TABLE SCES.RbMasterlist (
    MmsStockCode VARCHAR(30) NOT NULL,
    ItemRefNumber VARCHAR(16) NULL,
    DetailedDesc VARCHAR(250) NULL,
    Keyword VARCHAR(250) NULL,
    CorePartNum VARCHAR(30) NULL,
    RebuiltStockNum VARCHAR(30) NOT NULL,
    CoreCharge VARCHAR(25) NULL,
    JobNumber VARCHAR(25) NULL,
    DateModified DATETIME NULL,
    LastModifiedBy VARCHAR(50) NOT NULL,
    EstimatedCost NUMERIC(20, 2) NULL,
    SopNumber VARCHAR(25) NULL,
    BuyNewCost VARCHAR(25) NULL,
    RemanCost VARCHAR(25) NULL,
    ExternalCost VARCHAR(25) NULL,
    DateConverted DATETIME NULL,
    Active NUMERIC(4, 0) NULL,
    CONSTRAINT PK_RbMasterlist PRIMARY KEY CLUSTERED (MmsStockCode ASC),
    CONSTRAINT UK_RebuiltStockNum UNIQUE NONCLUSTERED (RebuiltStockNum ASC)
) ON [PRIMARY];
GO

-- RbListOfBuses Table
CREATE TABLE SCES.RbListOfBuses (
    RbListOfBusesId INT IDENTITY(1,1) NOT NULL,
    RebuiltStockNum VARCHAR(30) NOT NULL,
    VehicleListId NUMERIC(38, 0) NOT NULL,
    CONSTRAINT PK_RbListOfBuses PRIMARY KEY CLUSTERED (RbListOfBusesId ASC),
    CONSTRAINT FK_RbListOfBuses_VehicleList FOREIGN KEY (VehicleListId) REFERENCES SCES.VehicleList (VehicleListId)
) ON [PRIMARY];
GO

-- ScPartsUsed Table
CREATE TABLE SCES.ScPartsUsed (
    MmsStockCode VARCHAR(30) NOT NULL,
    DateEntered DATETIME NULL,
    RebPartCost NUMERIC(18, 2) NULL,
    UserEntered VARCHAR(50) NULL,
    MmsCost NUMERIC(18, 2) NULL,
    OemCost NUMERIC(18, 2) NULL,
    LastModifiedBy VARCHAR(25) NULL,
    LastModifiedDate DATETIME NULL,
    MmsRebuiltCode VARCHAR(30) NULL,
    CostCentre VARCHAR(5) NULL,
    QtyReqd VARCHAR(10) NULL,
    PercentUsage VARCHAR(10) NULL,
    LinkCode VARCHAR(30) NULL,
    RebuiltPart VARCHAR(1) NULL,
    LinkType VARCHAR(3) NULL,
    CoreCost NUMERIC(18, 2) NULL,
    OrigSuppNum VARCHAR(50) NULL,
    OrigSupplierName VARCHAR(50) NULL,
    CONSTRAINT PK_ScPartsUsed PRIMARY KEY CLUSTERED (MmsStockCode ASC),
    CONSTRAINT FK_ScPartsUsed_CostCenters FOREIGN KEY (CostCentre) REFERENCES SCES.CostCenters (CostCentre)
) ON [PRIMARY];
GO

-- StockCodedParts Table
CREATE TABLE SCES.StockCodedParts (
    PartNo VARCHAR(30) NOT NULL,
    CorePartNumber VARCHAR(30) NULL,
    PartType VARCHAR(50) NULL,
    JobNumber VARCHAR(30) NULL,
    DetailedDesc VARCHAR(500) NULL,
    DateEntered VARCHAR(8) NULL,
    ItemRefNumber VARCHAR(16) NULL,
    OverheadType NUMERIC(9, 0) NULL,
    CoreCharge NUMERIC(9, 0) NULL,
    PartCost NUMERIC(9, 0) NULL,
    LastModifiedDate DATETIME NULL,
    LastModifiedBy VARCHAR(30) NULL,
    AddedBy VARCHAR(30) NULL,
    MmsNewCost NUMERIC(15, 5) NULL,
    MmsSyncDate DATETIME NULL,
    OrigSupplierNum VARCHAR(25) NULL,
    OrigSupplierName VARCHAR(40) NULL,
    CONSTRAINT PK_StockCodedParts PRIMARY KEY CLUSTERED (PartNo ASC)
) ON [PRIMARY];
GO

-- ScOemkitsTextData Table
CREATE TABLE SCES.ScOemkitsTextData (
    PartNo VARCHAR(30) NOT NULL,
    TextData VARCHAR(132) NULL,
    CONSTRAINT PK_ScOemkitsTextData PRIMARY KEY CLUSTERED (PartNo ASC)
) ON [PRIMARY];
GO

-- NscPartsUsed Table
CREATE TABLE SCES.NscPartsUsed (
    NscPartsUsedId INT IDENTITY(1,1) NOT NULL,
    OrigSuppNum VARCHAR(50) NULL,
    OrigSupplierName VARCHAR(50) NULL,
    CostCentre VARCHAR(5) NULL,
    QtyReqd VARCHAR(10) NULL,
    PercentUsage VARCHAR(10) NULL,
    DateEntered DATETIME NULL,
    EnteredBy VARCHAR(25) NULL,
    LastModifiedBy VARCHAR(25) NULL,
    LastModifiedDate DATETIME NULL,
    LinkCode VARCHAR(30) NULL,
    Cost NUMERIC(18, 2) NULL,
    LinkType VARCHAR(3) NULL,
    CoreCost NUMERIC(18, 2) NULL,
    Id NUMERIC(10, 0) NULL,
    CONSTRAINT PK_NscPartsUsed PRIMARY KEY CLUSTERED (NscPartsUsedId ASC),
    CONSTRAINT FK_NscPartsUsed_CostCenters FOREIGN KEY (CostCentre) REFERENCES SCES.CostCenters (CostCentre)
) ON [PRIMARY];
GO


-- KitsMasterlist Table
CREATE TABLE SCES.KitsMasterlist (
    PartNo VARCHAR(30) NOT NULL,
    EstimatedCost NUMERIC(18, 2) NULL,
    Keyword VARCHAR(50) NULL,
    DetailedDesc VARCHAR(250) NULL,
    ModifiedLastBy VARCHAR(25) NULL,
    ModifiedLastDate DATE NULL,
    DateEntered DATETIME NULL,
    CONSTRAINT PK_KitsMasterlist PRIMARY KEY CLUSTERED (PartNo ASC)
) ON [PRIMARY];
GO

-- KitsUsed Table
CREATE TABLE SCES.KitsUsed (
    PartNo VARCHAR(30) NOT NULL,
    DateEntered DATETIME NULL,
    UserEntered VARCHAR(30) NULL,
    LastModifiedBy VARCHAR(30) NULL,
    LastModifiedDate DATETIME NULL,
    CostCentre VARCHAR(5) NULL,
    LinkCode VARCHAR(30) NULL,
    CONSTRAINT PK_KitsUsed PRIMARY KEY CLUSTERED (PartNo ASC),
    CONSTRAINT FK_KitsUsed_CostCenters FOREIGN KEY (CostCentre) REFERENCES SCES.CostCenters (CostCentre),
    CONSTRAINT FK_KitsUsed_KitsMasterlist FOREIGN KEY (PartNo) REFERENCES SCES.KitsMasterlist (PartNo)
) ON [PRIMARY];
GO

-- LabourTaskDescriptions Table
CREATE TABLE SCES.LabourTaskDescriptions (
    TaskDescription VARCHAR(50) NOT NULL,
    CONSTRAINT PK_LabourTaskDescriptions PRIMARY KEY CLUSTERED (TaskDescription ASC)
) ON [PRIMARY];
GO

-- EmployeeLabour Table
CREATE TABLE SCES.EmployeeLabour (
    Id INT IDENTITY(1,1) NOT NULL,
    LabourDefn VARCHAR(50) NULL,
    DateEntered DATETIME NULL,
    LinkNumber VARCHAR(25) NULL,
    TypeId VARCHAR(5) NULL,
    CostCentre VARCHAR(5) NULL,
    Task VARCHAR(50) NULL,
    LabourType VARCHAR(5) NULL,
    Usage VARCHAR(25) NULL,
    HrsReqd VARCHAR(25) NULL,
    AdjHrs VARCHAR(25) NULL,
    DateRevised DATE NULL,
    TimeAddition NUMERIC(18, 2) NULL,
    RebuiltPartNum VARCHAR(30) NULL,
    LastModifiedBy VARCHAR(25) NULL,
    CONSTRAINT PK_EmployeeLabour PRIMARY KEY CLUSTERED (Id ASC),
    CONSTRAINT FK_EmployeeLabour_CostCenters FOREIGN KEY (CostCentre) REFERENCES SCES.CostCenters (CostCentre),
    CONSTRAINT FK_EmployeeLabour_LabourTaskDescriptions FOREIGN KEY (Task) REFERENCES SCES.LabourTaskDescriptions (TaskDescription)
) ON [PRIMARY];
GO

-- Linking Tables

-- RbMasterlistVehicleList Linking Table
CREATE TABLE SCES.RbMasterlistVehicleList (
    RbMasterlistVehicleListId INT IDENTITY(1,1) NOT NULL,
    MmsStockCode VARCHAR(30) NOT NULL,
    VehicleListId NUMERIC(38, 0) NOT NULL,
    CONSTRAINT PK_RbMasterlistVehicleList PRIMARY KEY CLUSTERED (RbMasterlistVehicleListId ASC),
    CONSTRAINT FK_RbMasterlistVehicleList_RbMasterlist FOREIGN KEY (MmsStockCode) REFERENCES SCES.RbMasterlist (MmsStockCode),
    CONSTRAINT FK_RbMasterlistVehicleList_VehicleList FOREIGN KEY (VehicleListId) REFERENCES SCES.VehicleList (VehicleListId)
) ON [PRIMARY];
GO

-- RbMasterlistEngines Linking Table
CREATE TABLE SCES.RbMasterlistEngines (
    RbMasterlistEnginesId INT IDENTITY(1,1) NOT NULL,
    MmsStockCode VARCHAR(30) NOT NULL,
    EngineName VARCHAR(25) NOT NULL,
    CONSTRAINT PK_RbMasterlistEngines PRIMARY KEY CLUSTERED (RbMasterlistEnginesId ASC),
    CONSTRAINT FK_RbMasterlistEngines_RbMasterlist FOREIGN KEY (MmsStockCode) REFERENCES SCES.RbMasterlist (MmsStockCode),
    CONSTRAINT FK_RbMasterlistEngines_Engines FOREIGN KEY (EngineName) REFERENCES SCES.Engines (Name)
) ON [PRIMARY];
GO

-- RbMasterlistTransmissions Linking Table
CREATE TABLE SCES.RbMasterlistTransmissions (
    RbMasterlistTransmissionsId INT IDENTITY(1,1) NOT NULL,
    MmsStockCode VARCHAR(30) NOT NULL,
    TransmissionName VARCHAR(25) NOT NULL,
    CONSTRAINT PK_RbMasterlistTransmissions PRIMARY KEY CLUSTERED (RbMasterlistTransmissionsId ASC),
    CONSTRAINT FK_RbMasterlistTransmissions_RbMasterlist FOREIGN KEY (MmsStockCode) REFERENCES SCES.RbMasterlist (MmsStockCode),
    CONSTRAINT FK_RbMasterlistTransmissions_Transmissions FOREIGN KEY (TransmissionName) REFERENCES SCES.Transmissions (Name)
) ON [PRIMARY];
GO

-- RbMasterlistCostCenters Linking Table
CREATE TABLE SCES.RbMasterlistCostCenters (
    RbMasterlistCostCentersId INT IDENTITY(1,1) NOT NULL,
    MmsStockCode VARCHAR(30) NOT NULL,
    CostCentre VARCHAR(5) NOT NULL,
    CONSTRAINT PK_RbMasterlistCostCenters PRIMARY KEY CLUSTERED (RbMasterlistCostCentersId ASC),
    CONSTRAINT FK_RbMasterlistCostCenters_RbMasterlist FOREIGN KEY (MmsStockCode) REFERENCES SCES.RbMasterlist (MmsStockCode),
    CONSTRAINT FK_RbMasterlistCostCenters_CostCenters FOREIGN KEY (CostCentre) REFERENCES SCES.CostCenters (CostCentre)
) ON [PRIMARY];
GO

-- EmployeeLabourEmplClass Linking Table
CREATE TABLE SCES.EmployeeLabourEmplClass (
    EmployeeLabourEmplClassId INT IDENTITY(1,1) NOT NULL,
    EmployeeLabourId INT NOT NULL,
    EmplClassId NUMERIC(10, 0) NOT NULL,
    CONSTRAINT PK_EmployeeLabourEmplClass PRIMARY KEY CLUSTERED (EmployeeLabourEmplClassId ASC),
    CONSTRAINT FK_EmployeeLabourEmplClass_EmployeeLabour FOREIGN KEY (EmployeeLabourId) REFERENCES SCES.EmployeeLabour (Id),
    CONSTRAINT FK_EmployeeLabourEmplClass_EmplClass FOREIGN KEY (EmplClassId) REFERENCES SCES.EmplClass (LabourType)
) ON [PRIMARY];
GO

-- MbListOfBuses Table
CREATE TABLE SCES.MbListOfBuses (
    MbListOfBusesId INT IDENTITY(1,1) NOT NULL,
    MbNumber VARCHAR(25) NOT NULL,
    VehicleListId NUMERIC(38, 0) NOT NULL,
    CONSTRAINT PK_MbListOfBuses PRIMARY KEY CLUSTERED (MbListOfBusesId ASC),
    CONSTRAINT FK_MbListOfBuses_VehicleList FOREIGN KEY (VehicleListId) REFERENCES SCES.VehicleList (VehicleListId)
) ON [PRIMARY];
GO

---- RbListOfBuses Table
--CREATE TABLE SCES.RbListOfBuses (
--    RbListOfBusesId INT IDENTITY(1,1) NOT NULL,
--    RebuiltStockNum VARCHAR(30) NOT NULL,
--    VehicleListId NUMERIC(38, 0) NOT NULL,
--    CONSTRAINT PK_RbListOfBuses PRIMARY KEY CLUSTERED (RbListOfBusesId ASC),
--    CONSTRAINT FK_RbListOfBuses_VehicleList FOREIGN KEY (VehicleListId) REFERENCES SCES.VehicleList (VehicleListId)
--) ON [PRIMARY];
--GO

-- Linking Tables

-- MbListOfBusesListOfBuses Linking Table
CREATE TABLE SCES.MbListOfBusesListOfBuses (
    MbListOfBusesListOfBusesId INT IDENTITY(1,1) NOT NULL,
    MbListOfBusesId INT NOT NULL,
    VehicleListId NUMERIC(38, 0) NOT NULL,
    CONSTRAINT PK_MbListOfBusesListOfBuses PRIMARY KEY CLUSTERED (MbListOfBusesListOfBusesId ASC),
    CONSTRAINT FK_MbListOfBusesListOfBuses_MbListOfBuses FOREIGN KEY (MbListOfBusesId) REFERENCES SCES.MbListOfBuses (MbListOfBusesId),
    CONSTRAINT FK_MbListOfBusesListOfBuses_VehicleList FOREIGN KEY (VehicleListId) REFERENCES SCES.VehicleList (VehicleListId)
) ON [PRIMARY];
GO

-- RbListOfBusesListOfBuses Linking Table
CREATE TABLE SCES.RbListOfBusesListOfBuses (
    RbListOfBusesListOfBusesId INT IDENTITY(1,1) NOT NULL,
    RbListOfBusesId INT NOT NULL,
    VehicleListId NUMERIC(38, 0) NOT NULL,
    CONSTRAINT PK_RbListOfBusesListOfBuses PRIMARY KEY CLUSTERED (RbListOfBusesListOfBusesId ASC),
    CONSTRAINT FK_RbListOfBusesListOfBuses_RbListOfBuses FOREIGN KEY (RbListOfBusesId) REFERENCES SCES.RbListOfBuses (RbListOfBusesId),
    CONSTRAINT FK_RbListOfBusesListOfBuses_VehicleList FOREIGN KEY (VehicleListId) REFERENCES SCES.VehicleList (VehicleListId)
) ON [PRIMARY];
GO