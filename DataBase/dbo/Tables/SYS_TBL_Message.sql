﻿CREATE TABLE [dbo].[SYS_TBL_Message]
(
	[ID] INT PRIMARY KEY IDENTITY (1, 1) NOT NULL, 
	[SentDate] DATETIME NULL,
	[Content] NVARCHAR(200) NOT NULL,
	[OrderID] INT NOT NULL,
	[MemberID] INT NOT NULL,
	[OpeningDate] DATETIME2 NULL,
	[CreatedOn] DATETIME2 NOT NULL, 
    [CreatedBy] NVARCHAR(50) NOT NULL, 
    [UpdatedOn] DATETIME2 NOT NULL, 
    [UpdatedBy] NVARCHAR(50) NOT NULL, 
    [Disable] BIT NOT NULL
	CONSTRAINT [FK_SYS_TBL_Message_SYS_TBL_LendingItem] FOREIGN KEY ([OrderID]) REFERENCES [dbo].[SYS_TBL_LendingItem] ([ID]),
	CONSTRAINT [FK_SYS_TBL_Message_SYS_TBL_Members] FOREIGN KEY ([MemberID]) REFERENCES [dbo].[SYS_TBL_Members] ([ID]),
)
