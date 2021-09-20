CREATE TABLE [dbo].[ProductionManagerHardwareDevice]
(
	[Id] INT NOT NULL PRIMARY KEY,
	[ProductionManagerId] INT NOT NULL,
	[DeviceType] TINYINT NOT NULL,
	[DriverName] NVARCHAR(500) NOT NULL,
	[SerialPort] NVARCHAR(5) NOT NULL,
	FOREIGN KEY ([ProductionManagerId]) REFERENCES [ProductionManager](Id)
)