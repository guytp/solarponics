using Solarponics.Data;
using Solarponics.Models;
using Solarponics.WebApi.Abstractions;
using System.Threading.Tasks;

namespace Solarponics.WebApi.Data
{
    internal class HardwareRepository : IHardwareRepository
    {
        private const string ProcedureNameGet = "[dbo].[ProductionManagerHardwareGet]";
        private const string ProcedureNameRemoveBarcodeScanner = "[dbo].[ProductionManagerHardwareRemoveBarcodeScanner]";
        private const string ProcedureNameRemoveLabelPrinter = "[dbo].[ProductionManagerHardwareRemoveLabelPrinter]";
        private const string ProcedureNameRemoveScale = "[dbo].[ProductionManagerHardwareRemoveScale]";
        private const string ProcedureNameSetBarcodeScanner = "[dbo].[ProductionManagerHardwareSetBarcodeScanner]";
        private const string ProcedureNameSetLabelPrinter = "[dbo].[ProductionManagerHardwareSetLabelPrinter]";
        private const string ProcedureNameSetScale = "[dbo].[ProductionManagerHardwareSetScale]";

        public HardwareRepository(IDatabaseConnection connection)
        {
            Connection = connection;
        }

        private IDatabaseConnection Connection { get; }
        
        public async Task<HardwareSettings> Get(string machineName)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameGet, new StoredProcedureParameter[]
                {
                    new StoredProcedureParameter("@machineName", machineName),
                });
            await storedProcedure.ExecuteReaderAsync();

            var barcodeScanner = await storedProcedure.GetFirstOrDefaultRowAsync<BarcodeScannerSettings>();
            var labelPrinterSmall = await storedProcedure.GetFirstOrDefaultRowAsync<LabelPrinterSettings>();
            var labelPrinterLarge = await storedProcedure.GetFirstOrDefaultRowAsync<LabelPrinterSettings>();
            var scale = await storedProcedure.GetFirstOrDefaultRowAsync<ScaleSettings>();

            return new HardwareSettings
            {
                BarcodeScanner = barcodeScanner,
                LabelPrinterSmall = labelPrinterSmall,
                LabelPrinterLarge = labelPrinterLarge,
                Scale = scale
            };
        }
        
        public async Task SetBarcodeScanner(string machineName, BarcodeScannerSettings settings, int userId)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameSetBarcodeScanner, new StoredProcedureParameter[]
                {
                    new StoredProcedureParameter("@machineName", machineName),
                    new StoredProcedureParameter("@baudRate", settings.BaudRate),
                    new StoredProcedureParameter("@dataBits", settings.DataBits),
                    new StoredProcedureParameter("@driverName", settings.DriverName),
                    new StoredProcedureParameter("@parity", settings.Parity),
                    new StoredProcedureParameter("@serialPort", settings.SerialPort),
                    new StoredProcedureParameter("@stopBits", settings.StopBits),
                    new StoredProcedureParameter("@userId", userId)
                });
            await storedProcedure.ExecuteNonQueryAsync();
        }

        public async Task RemoveBarcodeScanner(string machineName, int userId)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameRemoveBarcodeScanner, new StoredProcedureParameter[]
                {
                    new StoredProcedureParameter("@machineName", machineName),
                    new StoredProcedureParameter("@userId", userId)
                });
            await storedProcedure.ExecuteNonQueryAsync();
        }
                
        public async Task SetScale(string machineName, ScaleSettings settings, int userId)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameSetScale, new StoredProcedureParameter[]
                {
                    new StoredProcedureParameter("@machineName", machineName),
                    new StoredProcedureParameter("@baudRate", settings.BaudRate),
                    new StoredProcedureParameter("@dataBits", settings.DataBits),
                    new StoredProcedureParameter("@driverName", settings.DriverName),
                    new StoredProcedureParameter("@parity", settings.Parity),
                    new StoredProcedureParameter("@serialPort", settings.SerialPort),
                    new StoredProcedureParameter("@stopBits", settings.StopBits),
                    new StoredProcedureParameter("@userId", userId)
                });
            await storedProcedure.ExecuteNonQueryAsync();
        }

        public async Task RemoveScale(string machineName, int userId)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameRemoveScale, new StoredProcedureParameter[]
                {
                    new StoredProcedureParameter("@machineName", machineName),
                    new StoredProcedureParameter("@userId", userId)
                });
            await storedProcedure.ExecuteNonQueryAsync();
        }

        public async Task SetLabelPrinter(string machineName, LabelPrinterSettings settings, int userId, string printerType)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameSetLabelPrinter, new StoredProcedureParameter[]
                {
                    new StoredProcedureParameter("@machineName", machineName),
                    new StoredProcedureParameter("@queueName", settings.QueueName),
                    new StoredProcedureParameter("@driverName", settings.DriverName),
                    new StoredProcedureParameter("@printerType", printerType),
                    new StoredProcedureParameter("@userId", userId)
                });
            await storedProcedure.ExecuteNonQueryAsync();
        }

        public async Task RemoveLabelPrinter(string machineName, int userId, string printerType)
        {
            using var storedProcedure = Connection.CreateStoredProcedure(ProcedureNameRemoveLabelPrinter, new StoredProcedureParameter[]
                {
                    new StoredProcedureParameter("@machineName", machineName),
                    new StoredProcedureParameter("@printerType", printerType),
                    new StoredProcedureParameter("@userId", userId)
                });
            await storedProcedure.ExecuteNonQueryAsync();
        }
    }
}