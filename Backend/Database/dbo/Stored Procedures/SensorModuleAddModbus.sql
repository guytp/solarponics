CREATE PROCEDURE [dbo].[SensorModuleAddModbus]
(
    @roomId INT,
    @serialNumber NVARCHAR(50),
    @driver NVARCHAR(50),
    @name NVARCHAR(50),
    @ipAddress NVARCHAR(39),
    @port SMALLINT,
    @userId INT,
    @temperatureSensorNumber INT,
    @temperatureWarningLowBelow DECIMAL(12, 4),
    @temperatureCriticalLowBelow DECIMAL(12, 4),
    @temperatureWarningHighAbove DECIMAL(12, 4),
    @temperatureCriticalHighAbove DECIMAL(12, 4),
    @humiditySensorNumber INT,
    @humidityWarningLowBelow DECIMAL(12, 4),
    @humidityCriticalLowBelow DECIMAL(12, 4),
    @humidityWarningHighAbove DECIMAL(12, 4),
    @humidityCriticalHighAbove DECIMAL(12, 4),
    @carbonDioxideSensorNumber INT,
    @carbonDioxideWarningLowBelow DECIMAL(12, 4),
    @carbonDioxideCriticalLowBelow DECIMAL(12, 4),
    @carbonDioxideWarningHighAbove DECIMAL(12, 4),
    @carbonDioxideCriticalHighAbove DECIMAL(12, 4)
)
AS
BEGIN
    SET NOCOUNT ON
    DECLARE @id INT
    SET XACT_ABORT ON
    BEGIN TRAN

    INSERT INTO SensorModule (RoomId, SerialNumber, Driver, [Name]) VALUES(@roomId, @serialNumber, @driver, @name)
    SELECT @id = SCOPE_IDENTITY()
    INSERT INTO SensorModuleModbusTcp (Id, IpAddress, [Port]) VALUES(@id, @ipAddress, @port)
    
	EXEC AuditAdd @table = 'SensorModule', @column = 'RoomId', @action = 'Add', @userId = @userId, @key = @id, @newValue = @roomId
    EXEC AuditAdd @table = 'SensorModule', @column = 'SerialNumber', @action = 'Add', @userId = @userId, @key = @id, @newValue = @serialNumber
    EXEC AuditAdd @table = 'SensorModule', @column = 'Driver', @action = 'Add', @userId = @userId, @key = @id, @newValue = @driver
    EXEC AuditAdd @table = 'SensorModule', @column = 'Name', @action = 'Add', @userId = @userId, @key = @id, @newValue = @name
    EXEC AuditAdd @table = 'SensorModuleModbusTcp', @column = 'IpAddress', @action = 'Add', @userId = @userId, @key = @id, @newValue = @ipAddress
    EXEC AuditAdd @table = 'SensorModuleModbusTcp', @column = 'Port', @action = 'Add', @userId = @userId, @key = @id, @newValue = @port

    DECLARE @sensorId INT
    IF (@temperatureSensorNumber IS NOT NULL)
    BEGIN
        INSERT INTO Sensor (SensorModuleId, [Type], [Number], WarningLowBelow, CriticalLowBelow, WarningHighAbove, CriticalHighAbove) VALUES(@id, 'Temperature', @temperatureSensorNumber, @temperatureWarningLowBelow, @temperatureCriticalLowBelow, @temperatureWarningHighAbove, @temperatureCriticalHighAbove)
        SELECT @sensorId = SCOPE_IDENTITY()
    	EXEC AuditAdd @table = 'Sensor', @column = 'SensorModuleId', @action = 'Add', @userId = @userId, @key = @sensorId, @newValue = @id
    	EXEC AuditAdd @table = 'Sensor', @column = 'Type', @action = 'Add', @userId = @userId, @key = @sensorId, @newValue = 'Temperature'
    	EXEC AuditAdd @table = 'Sensor', @column = 'Number', @action = 'Add', @userId = @userId, @key = @sensorId, @newValue = @temperatureSensorNumber
    	EXEC AuditAdd @table = 'Sensor', @column = 'WarningLowBelow', @action = 'Add', @userId = @userId, @key = @sensorId, @newValue = @temperatureWarningLowBelow
    	EXEC AuditAdd @table = 'Sensor', @column = 'CriticalLowBelow', @action = 'Add', @userId = @userId, @key = @sensorId, @newValue = @temperatureCriticalLowBelow
    	EXEC AuditAdd @table = 'Sensor', @column = 'WarningHighAbove', @action = 'Add', @userId = @userId, @key = @sensorId, @newValue = @temperatureWarningHighAbove
    	EXEC AuditAdd @table = 'Sensor', @column = 'CriticalHighAbove', @action = 'Add', @userId = @userId, @key = @sensorId, @newValue = @temperatureCriticalHighAbove
    END
    
    IF (@humiditySensorNumber IS NOT NULL)
    BEGIN
        INSERT INTO Sensor (SensorModuleId, [Type], [Number], WarningLowBelow, CriticalLowBelow, WarningHighAbove, CriticalHighAbove) VALUES(@id, 'Humidity', @humiditySensorNumber, @humidityWarningLowBelow, @humidityCriticalLowBelow, @humidityWarningHighAbove, @humidityCriticalHighAbove)
        SELECT @sensorId = SCOPE_IDENTITY()
    	EXEC AuditAdd @table = 'Sensor', @column = 'SensorModuleId', @action = 'Add', @userId = @userId, @key = @sensorId, @newValue = @id
    	EXEC AuditAdd @table = 'Sensor', @column = 'Type', @action = 'Add', @userId = @userId, @key = @sensorId, @newValue = 'Humidity'
    	EXEC AuditAdd @table = 'Sensor', @column = 'Number', @action = 'Add', @userId = @userId, @key = @sensorId, @newValue = @humiditySensorNumber
    	EXEC AuditAdd @table = 'Sensor', @column = 'WarningLowBelow', @action = 'Add', @userId = @userId, @key = @sensorId, @newValue = @humidityWarningLowBelow
    	EXEC AuditAdd @table = 'Sensor', @column = 'CriticalLowBelow', @action = 'Add', @userId = @userId, @key = @sensorId, @newValue = @humidityCriticalLowBelow
    	EXEC AuditAdd @table = 'Sensor', @column = 'WarningHighAbove', @action = 'Add', @userId = @userId, @key = @sensorId, @newValue = @humidityWarningHighAbove
    	EXEC AuditAdd @table = 'Sensor', @column = 'CriticalHighAbove', @action = 'Add', @userId = @userId, @key = @sensorId, @newValue = @humidityCriticalHighAbove
    END

    IF (@carbonDioxideSensorNumber IS NOT NULL)
    BEGIN
        INSERT INTO Sensor (SensorModuleId, [Type], [Number], WarningLowBelow, CriticalLowBelow, WarningHighAbove, CriticalHighAbove) VALUES(@id, 'CarbonDioxide', @carbonDioxideSensorNumber, @carbonDioxideWarningLowBelow, @carbonDioxideCriticalLowBelow, @carbonDioxideWarningHighAbove, @carbonDioxideWarningHighAbove)
        SELECT @sensorId = SCOPE_IDENTITY()
    	EXEC AuditAdd @table = 'Sensor', @column = 'SensorModuleId', @action = 'Add', @userId = @userId, @key = @sensorId, @newValue = @id
    	EXEC AuditAdd @table = 'Sensor', @column = 'Type', @action = 'Add', @userId = @userId, @key = @sensorId, @newValue = 'CarbonDioxide'
    	EXEC AuditAdd @table = 'Sensor', @column = 'Number', @action = 'Add', @userId = @userId, @key = @sensorId, @newValue = @carbonDioxideSensorNumber
    	EXEC AuditAdd @table = 'Sensor', @column = 'WarningLowBelow', @action = 'Add', @userId = @userId, @key = @sensorId, @newValue = @carbonDioxideWarningLowBelow
    	EXEC AuditAdd @table = 'Sensor', @column = 'CriticalLowBelow', @action = 'Add', @userId = @userId, @key = @sensorId, @newValue = @carbonDioxideCriticalLowBelow
    	EXEC AuditAdd @table = 'Sensor', @column = 'WarningHighAbove', @action = 'Add', @userId = @userId, @key = @sensorId, @newValue = @carbonDioxideWarningHighAbove
    	EXEC AuditAdd @table = 'Sensor', @column = 'CriticalHighAbove', @action = 'Add', @userId = @userId, @key = @sensorId, @newValue = @carbonDioxideCriticalHighAbove
    END

    SELECT @id [Id]
    COMMIT TRAN
    RETURN 0
END
GO

GRANT EXECUTE ON [dbo].[SensorModuleAddModbus] TO [WebApi]