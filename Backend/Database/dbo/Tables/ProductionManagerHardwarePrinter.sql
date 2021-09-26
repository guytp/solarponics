CREATE TABLE [dbo].[ProductionManagerHardwarePrinter]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[ProductionManagerId] INT NOT NULL,
	[PrinterType] NVARCHAR(50) NOT NULL,
	[DriverName] NVARCHAR(500) NOT NULL,
	[QueueName] NVARCHAR(200) NOT NULL,
	FOREIGN KEY ([ProductionManagerId]) REFERENCES [ProductionManager](Id)
)