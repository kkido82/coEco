﻿CREATE TABLE [dbo].[SYS_TBL_DistanceUnit]
(
	[ID] INT PRIMARY KEY IDENTITY (1, 1) NOT NULL, 
	[FirstUnitID] INT NOT NULL,
	[SecondUnitID] INT NOT NULL,
	[Distance] FLOAT NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL, 
    [CreatedBy] NVARCHAR(50) NOT NULL, 
    [UpdatedOn] DATETIME2 NOT NULL, 
    [UpdatedBy] NVARCHAR(50) NOT NULL, 
    [Disable] BIT NOT NULL
	CONSTRAINT [FK_SYS_TBL_Item_SYS_TBL_VAL_FirstUnit] FOREIGN KEY ([FirstUnitID]) REFERENCES [dbo].[SYS_TBL_Unit] ([ID]),
	CONSTRAINT [FK_SYS_TBL_Item_SYS_TBL_VAL_SecondUnit] FOREIGN KEY ([FirstUnitID]) REFERENCES [dbo].[SYS_TBL_Unit] ([ID]),
)
