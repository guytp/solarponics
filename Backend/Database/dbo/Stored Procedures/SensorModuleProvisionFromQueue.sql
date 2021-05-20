

CREATE PROCEDURE SensorModuleProvisionFromQueue
(
	@serialNumber NVARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @room NVARCHAR(50)
	DECLARE @location NVARCHAR(50)
	DECLARE @numberTemperatureSensors TINYINT
	DECLARE @numberHumiditySensors TINYINT
	DECLARE @numberCarbonDioxideSensors TINYINT
	DECLARE @name NVARCHAR(50)

	SELECT
		TOP 1
			@room = [Room],
			@location = [Location],
			@numberTemperatureSensors = NumberTemperatureSensors,
			@numberHumiditySensors = NumberHumiditySensors,
			@numberCarbonDioxideSensors = NumberCarbonDioxideSensors,
			@name = [Name]
		FROM SensorModuleProvisioningQueue
		WHERE
			SerialNumber = @serialNumber

	IF @room IS NULL
		THROW 50000, N'Serial number not found in provisioning queue', 1
		
	SET XACT_ABORT ON
	BEGIN TRANSACTION SensorProvision

	DECLARE @locationId INT
	SELECT TOP 1 @locationId = Id FROM [Location] WHERE [Name] = @location
	IF @locationId IS NULL
	BEGIN
		INSERT INTO [location] ([Name]) VALUES(@location)
		SELECT @locationId = SCOPE_IDENTITY()
	END

	DECLARE @roomId INT
	SELECT TOP 1 @roomId = Id FROM [Room] WHERE [Name] = @room
	IF @roomId IS NULL
	BEGIN
		INSERT INTO [Room] (LocationId, [Name]) VALUES(@locationId, @room)
		SELECT @roomId = SCOPE_IDENTITY()
	END

	DECLARE @sensorModuleId INT
	INSERT INTO [SensorModule] (RoomId, SerialNumber, [Name]) VALUES(@roomId, @serialNumber, @name)
	SELECT @sensorModuleId = SCOPE_IDENTITY()

	DECLARE @i INT
	SELECT @i = 1
	WHILE @i <= @numberTemperatureSensors
	BEGIN
		INSERT INTO [Sensor] (SensorModuleId, [Type], [Number]) VALUES(@sensorModuleId, 'Temperature', @i)
		SELECT @i = @i + 1
	END

	SELECT @i = 1
	WHILE @i <= @numberHumiditySensors
	BEGIN
		INSERT INTO [Sensor] (SensorModuleId, [Type], [Number]) VALUES(@sensorModuleId, 'Humidity', @i)
		SELECT @i = @i + 1
	END
	
	SELECT @i = 1
	WHILE @i <= @numberCarbonDioxideSensors
	BEGIN
		INSERT INTO [Sensor] (SensorModuleId, [Type], [Number]) VALUES(@sensorModuleId, 'Carbon Dioxide', @i)
		SELECT @i = @i + 1
	END

	DELETE FROM SensorModuleProvisioningQueue WHERE SerialNumber = @serialNumber

	COMMIT TRANSACTION SensorProvision
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SensorModuleProvisionFromQueue] TO [ProvisioningServer]
    AS [dbo];

