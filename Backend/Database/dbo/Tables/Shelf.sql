﻿CREATE TABLE [dbo].[Shelf]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[RoomId] INT NOT NULL,
	[Name] NVARCHAR(100) NOT NULL,
	FOREIGN KEY (RoomId) REFERENCES Room(Id)
)
