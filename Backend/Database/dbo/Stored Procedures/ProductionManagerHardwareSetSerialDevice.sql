CREATE PROCEDURE [dbo].[ProductionManagerHardwareSetSerialDevice]
(
	@machineName NVARCHAR(100),
	@deviceType NVARCHAR(50),
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

	DECLARE @id INT
	SELECT TOP 1 @id = Id FROM ProductionManager WHERE MachineName = @machineName
	
	SET XACT_ABORT ON
	BEGIN TRAN

	IF (@id IS NULL)
	BEGIN
		INSERT INTO ProductionManager (MachineName) VALUES(@machineName)
		SELECT @id = SCOPE_IDENTITY()
	END

	DECLARE @hardwareId INT
	SELECT TOP 1 @hardwareId = Id FROM ProductionManagerHardwareSerialDevice WHERE ProductionManagerId = @id AND DeviceType = @deviceType

	IF (@hardwareId IS NULL)
	BEGIN
		INSERT INTO ProductionManagerHardwareSerialDevice
				(ProductionManagerId, DeviceType, DriverName, SerialPort, BaudRate, Parity, DataBits, StopBits)
			VALUES
				(@id, @deviceType, @driverName, @serialPort, @baudRate, @parity, @dataBits, @stopBits)
		SELECT @hardwareId = SCOPE_IDENTITY()
		
		EXEC AuditAdd @table = 'ProductionManagerHardwareSerialDevice', @action = 'Add', @userId = @userId, @key = @hardwareId,  @column = 'ProductionManagerId',  @newValue = @id
		EXEC AuditAdd @table = 'ProductionManagerHardwareSerialDevice', @action = 'Add', @userId = @userId, @key = @hardwareId,  @column = 'DeviceType',  @newValue = @deviceType
		EXEC AuditAdd @table = 'ProductionManagerHardwareSerialDevice', @action = 'Add', @userId = @userId, @key = @hardwareId,  @column = 'DriverName',  @newValue = @driverName
		EXEC AuditAdd @table = 'ProductionManagerHardwareSerialDevice', @action = 'Add', @userId = @userId, @key = @hardwareId,  @column = 'SerialPort',  @newValue = @serialPort
		EXEC AuditAdd @table = 'ProductionManagerHardwareSerialDevice', @action = 'Add', @userId = @userId, @key = @hardwareId,  @column = 'BaudRate',  @newValue = @baudRate
		EXEC AuditAdd @table = 'ProductionManagerHardwareSerialDevice', @action = 'Add', @userId = @userId, @key = @hardwareId,  @column = 'Parity',  @newValue = @parity
		EXEC AuditAdd @table = 'ProductionManagerHardwareSerialDevice', @action = 'Add', @userId = @userId, @key = @hardwareId,  @column = 'DataBits',  @newValue = @dataBits
		EXEC AuditAdd @table = 'ProductionManagerHardwareSerialDevice', @action = 'Add', @userId = @userId, @key = @hardwareId,  @column = 'StopBits',  @newValue = @stopBits
	END
	ELSE
	BEGIN
		DECLARE @existingDriverName NVARCHAR(500)
		DECLARE @existingSerialPort NVARCHAR(5)
		DECLARE @existingBaudRate INT
		DECLARE @existingParity TINYINT
		DECLARE @existingDataBits INT
		DECLARE @existingStopBits TINYINT
		
		SELECT
				TOP 1
					@existingDriverName = DriverName,
					@existingSerialPort = SerialPort,
					@existingBaudRate = BaudRate,
					@existingParity = Parity,
					@existingDataBits = DataBits,
					@existingStopBits = StopBits
				FROM ProductionManagerHardwareSerialDevice
				WHERE Id = @hardwareId
		
		UPDATE ProductionManagerHardwareSerialDevice
			SET
				DriverName = @driverName,
				SerialPort = @serialPort,
				BaudRate = @baudRate,
				Parity = @parity,
				DataBits = @dataBits,
				StopBits = @stopBits
			WHERE Id = @hardwareId

		IF @existingDriverName != @driverName
			EXEC AuditAdd @table = 'ProductionManagerHardwareSerialDevice', @action = 'Update', @userId = @userId, @key = @hardwareId,  @column = 'DriverName',  @newValue = @driverName, @oldValue = @existingDriverName
		IF @existingSerialPort != @serialPort
			EXEC AuditAdd @table = 'ProductionManagerHardwareSerialDevice', @action = 'Update', @userId = @userId, @key = @hardwareId,  @column = 'SerialPort',  @newValue = @serialPort, @oldValue = @existingSerialPort
		IF @existingBaudRate != @baudRate
			EXEC AuditAdd @table = 'ProductionManagerHardwareSerialDevice', @action = 'Update', @userId = @userId, @key = @hardwareId,  @column = 'BaudRate',  @newValue = @baudRate, @oldValue = @existingBaudRate
		IF @existingParity != @parity
			EXEC AuditAdd @table = 'ProductionManagerHardwareSerialDevice', @action = 'Update', @userId = @userId, @key = @hardwareId,  @column = 'Parity',  @newValue = @parity, @oldValue = @existingParity
		IF @existingDataBits != @dataBits
			EXEC AuditAdd @table = 'ProductionManagerHardwareSerialDevice', @action = 'Update', @userId = @userId, @key = @hardwareId,  @column = 'DataBits',  @newValue = @dataBits, @oldValue = @existingDataBits
		IF @existingStopBits != @stopBits
			EXEC AuditAdd @table = 'ProductionManagerHardwareSerialDevice', @action = 'Update', @userId = @userId, @key = @hardwareId,  @column = 'StopBits',  @newValue = @stopBits, @oldValue = @existingStopBits
	END

	COMMIT TRAN

	RETURN 0
END
GO