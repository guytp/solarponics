using System.Linq;
using System.Threading.Tasks;
using Solarponics.Data;
using Solarponics.Models;
using Solarponics.WebApi.Abstractions;

namespace Solarponics.WebApi.Data
{
    internal class SensorModuleRepository : ISensorModuleRepository
    {
        private const string ProcedureNameSensorModuleGetAll =
            "[dbo].[SensorModuleGetAll]";

        private const string ProcedureNameSensorModuleProvisioningQueueGetBySerialNumber =
            "[dbo].[SensorModuleProvisioningQueueGetBySerialNumber]";

        private const string ProcedureNameSensorModuleProvisioningQueueGetAll =
            "[dbo].[SensorModuleProvisioningQueueGetAll]";

        private const string ProcedureNameSensorModuleProvisioningQueueDelete =
            "[dbo].[SensorModuleProvisioningQueueDelete]";

        private const string ProcedureNameSensorModuleProvisioningQueueAdd =
            "[dbo].[SensorModuleProvisioningQueueAdd]";

        public SensorModuleRepository(IDatabaseConnection connection)
        {
            Connection = connection;
        }

        private IDatabaseConnection Connection { get; }

        public async Task<SensorModule[]> GetAll()
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameSensorModuleGetAll, null);
            await storedProcedure.ExecuteReaderAsync();
            var modules = (await storedProcedure.GetDataSetAsync<SensorModule>())?.ToArray();
            if (modules == null)
                return null;

            var sensors = (await storedProcedure.GetDataSetAsync<Sensor>()).ToArray();
            foreach (var module in modules)
            {
                module.Sensors = sensors.Where(s => s.SensorModuleId == module.Id).ToArray();
                if (module.Sensors.Length == 0)
                    module.Sensors = null;
            }

            return modules;
        }

        public async Task<SensorModuleConfig[]> ProvisionQueueGetAll()
        {
            using var storedProcedure =
                Connection.CreateStoredProcedure(ProcedureNameSensorModuleProvisioningQueueGetAll, null);
            await storedProcedure.ExecuteReaderAsync();
            var rows = (await storedProcedure.GetDataSetAsync<SensorModuleConfigRow>())?.ToArray();
            return rows == null || rows.Length == 0 ? null : rows.Select(ConvertRowToConfig).ToArray();
        }

        public async Task ProvisioningQueueAdd(SensorModuleConfig config)
        {
            using var storedProcedure =
                Connection.CreateStoredProcedure(ProcedureNameSensorModuleProvisioningQueueAdd, new[]
                {
                    new StoredProcedureParameter("@serialNumber", config.SerialNumber),
                    new StoredProcedureParameter("@name", config.Name),
                    new StoredProcedureParameter("@location", config.Location),
                    new StoredProcedureParameter("@room", config.Room),
                    new StoredProcedureParameter("@numberTemperatureSensors",
                        config.SensorConfig?.TemperatureSensors ?? 0),
                    new StoredProcedureParameter("@numberHumiditySensors",
                        config.SensorConfig?.HumiditySensors ?? 0),
                    new StoredProcedureParameter("@numberCarbonDioxideSensors",
                        config.SensorConfig?.CarbonDioxideSensors ?? 0),
                    new StoredProcedureParameter("@networkType", config.NetworkType),
                    new StoredProcedureParameter("@wirelessSsid",
                        config.NetworkType == NetworkType.Wireless ? config.WirelessConfig?.Ssid : null),
                    new StoredProcedureParameter("@wirelessKey",
                        config.NetworkType == NetworkType.Wireless ? config.WirelessConfig?.Key : null),
                    new StoredProcedureParameter("@ipType", config.IpConfigType),
                    new StoredProcedureParameter("@ipAddress",
                        config.IpConfigType == IpConfigType.Dhcp ? null : config.StaticIpConfig?.Address),
                    new StoredProcedureParameter("@ipBroadcast",
                        config.IpConfigType == IpConfigType.Dhcp ? null : config.StaticIpConfig?.Broadcast),
                    new StoredProcedureParameter("@ipGateway",
                        config.IpConfigType == IpConfigType.Dhcp ? null : config.StaticIpConfig?.Gateway),
                    new StoredProcedureParameter("@ipDns",
                        config.IpConfigType == IpConfigType.Dhcp ? null : config.StaticIpConfig?.Dns),
                    new StoredProcedureParameter("@serverAddress", config.ServerAddress)
                });
            await storedProcedure.ExecuteNonQueryAsync();
        }

        public async Task<SensorModuleConfig> ProvisionQueueGetBySerialNumber(string serialNumber)
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

        public async Task ProvisioningQueueDelete(string serialNumber)
        {
            using var storedProcedure =
                Connection.CreateStoredProcedure(ProcedureNameSensorModuleProvisioningQueueDelete, new[]
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
                NetworkType = (NetworkType)row.NetworkType,
                WirelessConfig = (NetworkType)row.NetworkType != NetworkType.Wireless
                    ? null
                    : new WirelessConfig
                    {
                        Key = row.WirelessKey,
                        Ssid = row.WirelessSsid
                    },
                ServerAddress = row.ServerAddress,
                IpConfigType = (IpConfigType)row.IpType,
                StaticIpConfig = (IpConfigType)row.IpType == IpConfigType.Dhcp
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