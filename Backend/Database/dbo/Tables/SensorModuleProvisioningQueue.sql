CREATE TABLE [dbo].[SensorModuleProvisioningQueue] (
    [SerialNumber]               NVARCHAR (50)  NOT NULL,
    [Name]                       NVARCHAR (50)  NOT NULL,
    [Location]                   NVARCHAR (50)  NOT NULL,
    [Room]                       NVARCHAR (50)  NOT NULL,
    [NumberTemperatureSensors]   TINYINT        NOT NULL,
    [NumberHumiditySensors]      TINYINT        NOT NULL,
    [NumberCarbonDioxideSensors] TINYINT        NOT NULL,
    [NetworkType]                NVARCHAR (10)  NOT NULL,
    [WirelessSsid]               NVARCHAR (50)  NULL,
    [WirelessKey]                NVARCHAR (50)  NULL,
    [IpType]                     NVARCHAR (10)  NOT NULL,
    [IpAddress]                  NVARCHAR (15)  NULL,
    [IpBroadcast]                NVARCHAR (15)  NULL,
    [IpGateway]                  NVARCHAR (15)  NULL,
    [IpDns]                      NVARCHAR (15)  NULL,
    [ServerAddress]              NVARCHAR (100) NOT NULL,
    PRIMARY KEY CLUSTERED ([SerialNumber] ASC)
);

