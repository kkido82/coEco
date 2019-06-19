CREATE TABLE [dbo].[SYS_TBL_Members]
(
	[ID] INT PRIMARY KEY IDENTITY (1, 1) NOT NULL, 
	[TZ] NVARCHAR (9) NOT NULL,
	[FirstName] NVARCHAR(50) NOT NULL, 
	[LastName] NVARCHAR(50) NOT NULL, 
	[Email] NVARCHAR(50) NULL,
	[PhoneNumber] NVARCHAR(50) NOT NULL, 
	[Role] NVARCHAR(1000) NULL,
	[UnitID] INT NOT NULL,
	[PermissionsProfileID] INT NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL, 
    [CreatedBy] NVARCHAR(50) NOT NULL, 
    [UpdatedOn] DATETIME2 NOT NULL, 
    [UpdatedBy] NVARCHAR(50) NOT NULL, 
    [Disable] BIT NOT NULL 

	CONSTRAINT [FK_SYS_TBL_Members_SYS_TBL_Unit] FOREIGN KEY ([UnitID]) REFERENCES [dbo].[SYS_TBL_Unit] ([ID]),
	CONSTRAINT [FK_SYS_TBL_Members_SYS_TBL_PermissionsProfileID] FOREIGN KEY ([PermissionsProfileID]) REFERENCES [dbo].[SYS_TBL_PermissionsProfile] ([ID]),

)
