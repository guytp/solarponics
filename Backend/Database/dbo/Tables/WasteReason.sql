﻿CREATE TABLE [dbo].[WasteReason]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[Reason] NVARCHAR(200) NOT NULL,
	[IsDeleted] BIT DEFAULT(0)
)