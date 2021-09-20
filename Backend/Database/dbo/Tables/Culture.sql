CREATE TABLE [dbo].[Culture]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[SupplierId] INT NOT NULL,
	[BookedInUserId] INT NOT NULL,
	[MediumType] INT NOT NULL,
	[OrderDate] DATETIME NOT NULL,
	[ReceivedDate] DATETIME NOT NULL,
	[Strain] NVARCHAR(500) NOT NULL,
	[Notes] NVARCHAR(MAX),
	FOREIGN KEY (SupplierId) REFERENCES Supplier(Id),
	FOREIGN KEY (BookedInUserId) REFERENCES [User](Id)
)