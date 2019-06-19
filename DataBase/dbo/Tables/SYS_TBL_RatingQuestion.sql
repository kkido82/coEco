CREATE TABLE [dbo].[SYS_TBL_RatingQuestion]
(
	[ID] INT PRIMARY KEY IDENTITY (1, 1) NOT NULL, 
	[QuestionText] NVARCHAR(100) NOT NULL,
	[CreatedOn] DATETIME2 NOT NULL, 
    [CreatedBy] NVARCHAR(50) NOT NULL, 
    [UpdatedOn] DATETIME2 NOT NULL, 
    [UpdatedBy] NVARCHAR(50) NOT NULL, 
    [Disable] BIT NOT NULL
)
