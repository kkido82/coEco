CREATE TABLE [dbo].[SYS_TBL_PermissionsProfile]
(
	[ID] INT PRIMARY KEY IDENTITY (1, 1) NOT NULL, 
	[Name] NVARCHAR(500) NOT NULL,
	[OpenAnOrder] BIT NOT NULL,
	[UpdateInventory] BIT NOT NULL,
	[OrderConfirmation] BIT NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL, 
    [CreatedBy] NVARCHAR(50) NOT NULL, 
    [UpdatedOn] DATETIME2 NOT NULL, 
    [UpdatedBy] NVARCHAR(50) NOT NULL, 
    [Disable] BIT NOT NULL
)
