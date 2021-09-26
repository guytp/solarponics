CREATE PROCEDURE [dbo].[ProductionManagerHardwareSetScale]
(
	@machineName NVARCHAR(100),
	@driverName NVARCHAR(500),
	@serialPort NVARCHAR(5),
	@baudRate INT,
	@parity TINYINT,
	@dataBits INT,
	@stopBits TINYINT,
	@userId INT
)
AS
BEGIN
	SET NOCOUNT ON

	EXEC [dbo].[ProductionManagerHardwareSetSerialDevice]
		@machineName = @machineName,
		@driverName = @driverName,
		@deviceType = 'Scale',
		@serialPort = @serialPort,
		@baudRate = @baudRate,
		@parity = @parity,
		@dataBits = @dataBits,
		@stopBits = @stopBits,
		@userId = @userId

	RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[ProductionManagerHardwareSetScale] TO [WebApi]