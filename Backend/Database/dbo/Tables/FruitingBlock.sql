CREATE TABLE [dbo].[FruitingBlock]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[GrainSpawnId] INT,
	[RecipeId] INT NOT NULL,
	[CreateUserId] INT NOT NULL,
	[CreateDate] DATETIME NOT NULL,
	[InnoculateUserId] INT NULL,
	[InnoculateDate] DATETIME NULL,
	[IncubateShelfId] INT,
	[IncubateShelfPlaceUserId] INT,
	[IncubateShelfPlaceDate] DATETIME,
	[FruitingShelfId] INT,
	[FruitingShelfPlaceUserId] INT,
	[FruitingShelfPlaceDate] DATETIME,
	[Weight] DECIMAL(5, 3) NOT NULL,
	[Notes] NVARCHAR(MAX),
	FOREIGN KEY (CreateUserId) REFERENCES [User](Id),
	FOREIGN KEY (InnoculateUserId) REFERENCES [User](Id),
	FOREIGN KEY (IncubateShelfPlaceUserId) REFERENCES [User](Id),
	FOREIGN KEY (FruitingShelfPlaceUserId) REFERENCES [User](Id),
	FOREIGN KEY (GrainSpawnId) REFERENCES [GrainSpawn](Id),
	FOREIGN KEY (RecipeId) REFERENCES [Recipe](Id)
)