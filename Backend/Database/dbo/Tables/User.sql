﻿CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	UserId SMALLINT NOT NULL,
	Pin SMALLINT NOT NULL,
	[Name] NVARCHAR(100) NOT NULL
)