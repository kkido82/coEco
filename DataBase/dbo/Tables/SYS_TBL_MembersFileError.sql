CREATE TABLE [dbo].[SYS_TBL_MembersFileError]
(
	[ID] INT PRIMARY KEY IDENTITY (1, 1) NOT NULL, 
	[TZ] NVARCHAR(50) NOT NULL,
	[FilesUploadName] NVARCHAR(max) NULL,
	[ErrorMsg] NVARCHAR(max) NULL,
	[CreatedOn] DATETIME2 NOT NULL, 
    [CreatedBy] NVARCHAR(50) NOT NULL, 
    [UpdatedOn] DATETIME2 NOT NULL, 
    [UpdatedBy] NVARCHAR(50) NOT NULL, 
    [Disable] BIT NOT NULL
)
