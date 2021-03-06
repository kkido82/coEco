﻿CREATE TABLE [dbo].[SYS_TBL_ItemToUnit]
(
	[ID] INT PRIMARY KEY IDENTITY (1, 1) NOT NULL, 
	[ItemID] INT NOT NULL,
	[UnitID] INT NOT NULL,
	[Quantity] INT NOT NULL,
	[Description] NVARCHAR(500) NULL,
	[CreatedBy] NVARCHAR(50) NOT NULL, 
    [UpdatedOn] DATETIME2 NOT NULL, 
    [UpdatedBy] NVARCHAR(50) NOT NULL, 
    [Disable] BIT NOT NULL
	CONSTRAINT [FK_SYS_TBL_ItemToUnit_SYS_TBL_Item] FOREIGN KEY ([ItemID]) REFERENCES [dbo].[SYS_TBL_Item] ([ID]),
	[CreatedOn] DATETIME2 NOT NULL, 
    CONSTRAINT [FK_SYS_TBL_ItemToUnit_SYS_TBL_Unit] FOREIGN KEY ([UnitID]) REFERENCES [dbo].[SYS_TBL_Unit] ([ID]),
)
