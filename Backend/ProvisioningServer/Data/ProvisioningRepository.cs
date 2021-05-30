using System.Threading.Tasks;
using Solarponics.Data;
using Solarponics.Models;
using Solarponics.Networking.Abstractions;

namespace Solarponics.ProvisioningServer.Data
{
    internal class ProvisioningRepository : IProvisioningRepository
    {
        private const string ProcedureNameSensorModuleProvisioningQueueGetBySerialNumber =
            "[dbo].[SensorModuleProvisioningQueueGetBySerialNumber]";

        private const string ProcedureNameSensorModuleProvisionFromQueue = "[dbo].[SensorModuleProvisionFromQueue]";


        public ProvisioningRepository(IDatabaseConnection connection)
        {
            Connection = connection;
        }

        private IDatabaseConnection Connection { get; }


        public async Task<SensorModuleConfig> GetConfig(string serialNumber)
        {
            using var storedProcedure =
                Connection.CreateStoredProcedure(ProcedureNameSensorModuleProvisioningQueueGetBySerialNumber, new[]
                {
                    new StoredProcedureParameter("@serialNumber", serialNumber)
                });
            await storedProcedure.ExecuteReaderAsync();
            var row = await storedProcedure.GetFirstOrDefaultRowAsync<SensorModuleConfigRow>();
            return row == null ? null : ConvertRowToConfig(row);
        }

        public async Task Provision(string serialNumber)
        {
            using var storedProcedure =
                Connection.CreateStoredProcedure(ProcedureNameSensorModuleProvisionFromQueue, new[]
                {
                    new StoredProcedureParameter("@serialNumber", serialNumber)
                });
            await storedProcedure.ExecuteNonQueryAsync();
        }

        private static SensorModuleConfig ConvertRowToConfig(SensorModuleConfigRow row)
        {
            return new SensorModuleConfig
            {
                SerialNumber = row.SerialNumber,
                Name = row.Name,
                Location = row.Location,
                Room = row.Room,
                SensorConfig = new SensorModuleSensorConfig
                {
                    CarbonDioxideSensors = row.NumberCarbonDioxideSensors,
                    HumiditySensors = row.NumberHumiditySensors,
                    TemperatureSensors = row.NumberTemperatureSensors
                },
                NetworkType = (NetworkType) row.NetworkType,
                WirelessConfig = (NetworkType) row.NetworkType != NetworkType.Wireless
                    ? null
                    : new WirelessConfig
                    {
                        Key = row.WirelessKey,
                        Ssid = row.WirelessSsid
                    },
                ServerAddress = row.ServerAddress,
                IpConfigType = (IpConfigType) row.IpType,
                StaticIpConfig = (IpConfigType) row.IpType == IpConfigType.Dhcp
                    ? null
                    : new IpConfig
                    {
                        Address = row.IpAddress,
                        Broadcast = row.IpBrodcast,
                        Dns = row.IpDns,
                        Gateway = row.IpGateway
                    }
            };
        }
    }
}