CREATE TABLE [dbo].[ProductionManagerHardwareSerialDevice]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY(1, 1),
	[ProductionManagerId] INT NOT NULL,
	[DeviceType] NVARCHAR(50) NOT NULL,
	[DriverName] NVARCHAR(500) NOT NULL,
	[SerialPort] NVARCHAR(5) NOT NULL,
	[BaudRate] INT NOT NULL,
	[Parity] TINYINT NOT NULL,
	[DataBits] INT NOT NULL,
	[StopBits] TINYINT NOT NULL,
	FOREIGN KEY ([ProductionManagerId]) REFERENCES [ProductionManager](Id)
)