
CREATE PROCEDURE SensorModuleProvisioningQueueGetAll
AS
BEGIN
	SET NOCOUNT ON
	SELECT
				SerialNumber, [Name], [Location], Room, NumberTemperatureSensors, NumberHumiditySensors,
				NumberCarbonDioxideSensors, NetworkType, WirelessSsid, WirelessKey, IpType, IpAddress,
				IpBroadcast, IpGateway, IpDns, ServerAddress
			FROM SensorModuleProvisioningQueue
			ORDER BY
				[SerialNumber] ASC,
				[Name] ASC
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SensorModuleProvisioningQueueGetAll] TO [ProvisioningServer]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[SensorModuleProvisioningQueueGetAll] TO [WebApi]
    AS [dbo];

