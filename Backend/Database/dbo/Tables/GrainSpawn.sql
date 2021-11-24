CREATE TABLE [dbo].[GrainSpawn]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[CultureId] INT,
	[RecipeId] INT NOT NULL,
	[CreateUserId] INT NOT NULL,
	[CreateDate] DATETIME NOT NULL,
	[InnoculateUserId] INT NULL,
	[InnoculateDate] DATETIME NULL,
	[ShelfId] INT,
	[ShelfPlaceUserId] INT,
	[ShelfPlaceDate] DATETIME,
	[Weight] DECIMAL(5, 3) NOT NULL,
	[Notes] NVARCHAR(MAX),
	FOREIGN KEY (CreateUserId) REFERENCES [User](Id),
	FOREIGN KEY (InnoculateUserId) REFERENCES [User](Id),
	FOREIGN KEY (ShelfPlaceUserId) REFERENCES [User](Id),
	FOREIGN KEY (CultureId) REFERENCES [Culture](Id),
	FOREIGN KEY (RecipeId) REFERENCES [Recipe](Id)
)