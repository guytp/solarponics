CREATE TABLE [dbo].[Culture]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[SupplierId] INT,
	[ParentCultureId] INT,
	[RecipeId] INT,
	[UserId] INT NOT NULL,
	[MediumType] INT NOT NULL,
	[Generation] INT NULL,
	[OrderDate] DATETIME,
	[CreateDate] DATETIME NOT NULL,
	[Strain] NVARCHAR(500),
	[Notes] NVARCHAR(MAX),
	FOREIGN KEY (SupplierId) REFERENCES Supplier(Id),
	FOREIGN KEY (UserId) REFERENCES [User](Id),
	FOREIGN KEY (ParentCultureId) REFERENCES [Culture](Id),
	FOREIGN KEY (RecipeId) REFERENCES [Recipe](Id)
)