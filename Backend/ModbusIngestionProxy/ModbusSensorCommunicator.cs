using EasyModbus;
using Solarponics.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Solarponics.ModbusIngestionProxy
{
    public class ModbusSensorCommunicator : IModbusSensorCommunicator, IDisposable
    {
        private readonly ModbusClient modbusClient;
        private readonly IIngestionClient ingestionClient;

        public string IpAddress { get; }
        public short Port { get; }

        public DateTime? LastReadingsSubmitted { get; private set; }
        public SensorModuleModbusTcp SensorModule { get; }

        public ModbusSensorCommunicator(SensorModuleModbusTcp sensorModule, IIngestionClient ingestionClient)
        {
            this.SensorModule = sensorModule;
            this.modbusClient = new ModbusClient();
            this.ingestionClient = ingestionClient;
        }

        public Task<SensorModuleReading> GetReadings()
        {
            if (SensorModule.Sensors == null || SensorModule.Sensors.Length < 1)
            {
                Console.WriteLine($"No sensors attached to {SensorModule.Name}, not reading");
                return Task.FromResult(new SensorModuleReading());
            }

            if (!modbusClient.IsConnected)
            {
                Console.WriteLine($"Connecting to Modbus on {SensorModule.Name}");
                modbusClient.Connect(SensorModule.IpAddress, SensorModule.Port);
            }

            var task = Task.Run(() =>
            {
                Console.WriteLine($"Reading {SensorModule.Sensors.Length} sensors on {SensorModule.Name}");
                var readings = this.modbusClient.ReadInputRegisters(0, SensorModule.Sensors.Length);

                var temperatureSensor = SensorModule.Sensors.FirstOrDefault(s => s.Type == SensorType.Temperature);
                var carbonDioxideSensor = SensorModule.Sensors.FirstOrDefault(s => s.Type == SensorType.CarbonDioxide);
                var humiditySensor = SensorModule.Sensors.FirstOrDefault(s => s.Type == SensorType.Humidity);

                var reading = new SensorModuleReading();

                if (temperatureSensor != null)
                    reading.Temperature = readings[temperatureSensor.Number - 1] / 10m;
                if (carbonDioxideSensor != null)
                    reading.CarbonDioxide = readings[carbonDioxideSensor.Number - 1] / 10m;
                if (humiditySensor != null)
                    reading.Humidity = readings[humiditySensor.Number - 1] / 10m;

                Console.WriteLine($"Temp: {reading.Temperature}     Humidity: {reading.Humidity}     CO2: {reading.CarbonDioxide}   from   {SensorModule.Name}");

                return reading;
            });

            return task;
        }


        public void SubmitIngestionReading(SensorModuleReading result)
        {
            Console.WriteLine($"Submitting {result.Temperature}  /  {result.Humidity}  /  {result.CarbonDioxide}  to  {SensorModule.Name}");

            try
            {
                this.LastReadingsSubmitted = DateTime.UtcNow;
                if (!this.ingestionClient.IsConnectedAndStarted)
                    this.ingestionClient.Start(this.SensorModule.SerialNumber);

                var temperatureSensor = SensorModule.Sensors.FirstOrDefault(s => s.Type == SensorType.Temperature);
                var carbonDioxideSensor = SensorModule.Sensors.FirstOrDefault(s => s.Type == SensorType.CarbonDioxide);
                var humiditySensor = SensorModule.Sensors.FirstOrDefault(s => s.Type == SensorType.Humidity);

                if (temperatureSensor != null && result.Temperature.HasValue)
                    this.ingestionClient.SendReading(SensorType.Temperature, temperatureSensor.Number, result.Temperature.Value);
                if (carbonDioxideSensor != null && result.CarbonDioxide.HasValue)
                    this.ingestionClient.SendReading(SensorType.CarbonDioxide, carbonDioxideSensor.Number, result.CarbonDioxide.Value);
                if (humiditySensor != null && result.Humidity.HasValue)
                    this.ingestionClient.SendReading(SensorType.Humidity, humiditySensor.Number, result.Humidity.Value);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to submit sensor readings for {SensorModule.Name}: " + ex);
            }
        }

        public void Dispose()
        {
            if (modbusClient.IsConnected)
                modbusClient.Disconnect();
            (this.ingestionClient as IDisposable)?.Dispose();
        }
    }
}