using Solarponics.Models;
using System;
using System.Linq;

namespace Solarponics.ProductionManager.LabelDefinitions
{
    public class SensorModuleLabelDefinition : LabelDefinition
    {
        public SensorModuleLabelDefinition(SensorModuleModbusTcp sensorModule)
        {
            var label = $"Name:     {sensorModule.Name}{Environment.NewLine}";
            label = $"S/N:      {sensorModule.SerialNumber}{Environment.NewLine}";
            label += $"IP:       {sensorModule.IpAddress}{Environment.NewLine}";
            label += $"Port:     {sensorModule.Port}{Environment.NewLine}";
            var sensors = string.Empty;
            if (sensorModule.Sensors != null)
            {
                if (sensorModule.Sensors.Any(s => s.Type == SensorType.CarbonDioxide))
                    sensors = AddSensorLine("CO2", sensors);
                if (sensorModule.Sensors.Any(s => s.Type == SensorType.Humidity))
                    sensors = AddSensorLine("Humidity", sensors);
                if (sensorModule.Sensors.Any(s => s.Type == SensorType.Temperature))
                    sensors = AddSensorLine("Temperature", sensors);
            }
            if (sensors == string.Empty)
                sensors = "None";
            label += $"Sensors:  {sensors}{Environment.NewLine}";
            Text = label;
            Barcode = $"SM{sensorModule.Id}";
            BarcodeSize = BarcodeSize.Small;
            TextSize = TextSize.Small;
            MaxTextWidth = 30;
            MaxLinesToPrint = 8;
        }

        private string AddSensorLine(string sensor, string existing)
        {
            var spacing = existing == string.Empty ? string.Empty : "          ";
            return existing += spacing + sensor + Environment.NewLine;
        }
    }
}