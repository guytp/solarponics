
CREATE PROCEDURE SensorModuleProvisioningQueueAdd
(
	@serialNumber VARCHAR(50),
	@name NVARCHAR(50),
	@location NVARCHAR(50),
	@room NVARCHAR(50),
	@numberTemperatureSensors TINYINT,
	@numberHumiditySensors TINYINT,
	@numberCarbonDioxideSensors TINYINT,
	@networkType NVARCHAR(10),
	@wirelessSsid NVARCHAR(50),
	@wirelessKey NVARCHAR(50),
	@ipType NVARCHAR(10),
	@ipAddress NVARCHAR(15),
	@ipBroadcast NVARCHAR(15),
	@ipGateway NVARCHAR(15),
	@ipDns NVARCHAR(15),
	@serverAddress NVARCHAR(100)
)
AS
BEGIN
	SET NOCOUNT ON
	
	IF EXISTS (SELECT 1 FROM SensorModule WHERE SerialNumber = @serialNumber)
		THROW 50000, N'Sensor Module already provisioned with this serial number', 1

	IF EXISTS (SELECT 1 FROM SensorModuleProvisioningQueue WHERE SerialNumber = @serialNumber)
		THROW 50000, N'Sensor Module already in provisioning queue with this serial number', 1

	INSERT
		INTO SensorModuleProvisioningQueue
			(SerialNumber, [Name], [Location], Room, NumberTemperatureSensors, NumberHumiditySensors,
			NumberCarbonDioxideSensors, NetworkType, WirelessSsid, WirelessKey, IpType, IpAddress,
			IpBroadcast, IpGateway, IpDns, ServerAddress)
		VALUES
			(@serialNumber, @name, @location, @room, @numberTemperatureSensors, @numberHumiditySensors,
			@numberCarbonDioxideSensors, @networkType, @wirelessSsid, @wirelessKey, @ipType, @ipAddress,
			@ipBroadcast, @ipGateway, @ipDns, @serverAddress)
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SensorModuleProvisioningQueueAdd] TO [WebApi]
    AS [dbo];

