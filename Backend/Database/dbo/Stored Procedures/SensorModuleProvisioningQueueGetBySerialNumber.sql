

CREATE PROCEDURE SensorModuleProvisioningQueueGetBySerialNumber
(
	@serialNumber NVARCHAR(50)
)
AS
BEGIN
	SET NOCOUNT ON
	SELECT
				SerialNumber, [Name], [Location], Room, NumberTemperatureSensors, NumberHumiditySensors,
				NumberCarbonDioxideSensors, NetworkType, WirelessSsid, WirelessKey, IpType, IpAddress,
				IpBroadcast, IpGateway, IpDns, ServerAddress
			FROM SensorModuleProvisioningQueue
			WHERE
				SerialNumber = @serialNumber
END