CREATE TABLE [dbo].[ProductionManagerPrinter]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[ProductionManagerId] INT NOT NULL,
	[PrinterType] TINYINT NOT NULL,
	[DriverName] NVARCHAR(500) NOT NULL,
	[QueueName] NVARCHAR(200) NOT NULL,
	FOREIGN KEY ([ProductionManagerId]) REFERENCES [ProductionManager](Id)
)