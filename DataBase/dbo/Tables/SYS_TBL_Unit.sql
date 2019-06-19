CREATE TABLE [dbo].[SYS_TBL_Unit]
(
	[ID] INT PRIMARY KEY IDENTITY (1, 1) NOT NULL, 
	[UnitName] NVARCHAR(500) NOT NULL,
	[OriginalWheelsQuantity] INT NOT NULL,
	[CurrentWheelQuantity] INT NOT NULL,
	[Rating] FLOAT NULL,
	[CreatedOn] DATETIME2 NOT NULL, 
    [CreatedBy] NVARCHAR(50) NOT NULL, 
    [UpdatedOn] DATETIME2 NOT NULL, 
    [UpdatedBy] NVARCHAR(50) NOT NULL, 
    [Disable] BIT NOT NULL

)
