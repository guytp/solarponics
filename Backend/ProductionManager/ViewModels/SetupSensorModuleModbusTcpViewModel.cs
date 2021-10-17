using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ApiClients;
using Solarponics.ProductionManager.Abstractions.Hardware;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Commands;
using Solarponics.ProductionManager.Core;
using Solarponics.ProductionManager.LabelDefinitions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Solarponics.ProductionManager.ViewModels
{
    public class SetupSensorModuleModbusTcpViewModel : ViewModelBase, ISetupSensorModuleModbusTcpViewModel
    {
        private readonly IDialogBox dialogBox;
        private readonly ISensorModuleApiClient apiClient;
        private readonly IHardwareProvider hardwareProvider;
        private readonly ILocationApiClient locationApiClient;

        public SetupSensorModuleModbusTcpViewModel(IDialogBox dialogBox, ILoggedInButtonsViewModel loggedInButtonsViewModel, ISensorModuleApiClient apiClient, IHardwareProvider hardwareProvider, ILocationApiClient locationApiClient)
        {
            this.dialogBox = dialogBox;
            LoggedInButtonsViewModel = loggedInButtonsViewModel;
            this.apiClient = apiClient;
            this.hardwareProvider = hardwareProvider;
            this.locationApiClient = locationApiClient;
            AddCommand = new RelayCommand(_ => this.Add());
            DeleteCommand = new RelayCommand(_ => this.Delete());
            PrintLabelCommand = new RelayCommand(_ => this.PrintLabel());
            SensorNumbers = new byte[]
            {
                1, 2, 3, 4, 5, 6, 7, 8, 9, 10
            };
        }

        public byte[] SensorNumbers { get; }
        public bool IsUiEnabled { get; private set; }

        public ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }

        public SensorModuleModbusTcp[] SensorModules { get; private set; }

        public SensorModuleModbusTcp SelectedSensorModule { get; set; }

        public bool IsSensorSelected => SelectedSensorModule != null;

        public ICommand DeleteCommand { get; }

        public ICommand PrintLabelCommand { get; }

        public ICommand AddCommand { get; }

        public bool IsAddEnabled =>
            !string.IsNullOrWhiteSpace(IpAddress) && !string.IsNullOrWhiteSpace(Port) && ushort.TryParse(Port, out _) && SelectedRoom != null && SelectedLocation != null && !string.IsNullOrWhiteSpace(SerialNumber) && !string.IsNullOrWhiteSpace(Name) && System.Net.IPAddress.TryParse(IpAddress, out _)
            && (IsTemperatureSensorEnabled || IsHumiditySensorEnabled || IsCarbonDioxideSensorEnabled)
            && (!IsTemperatureSensorEnabled || (TemperatureSensorNumber.HasValue && !string.IsNullOrWhiteSpace(TemperatureSensorCriticalLowBelow) && !string.IsNullOrWhiteSpace(TemperatureSensorCriticalHighAbove) && !string.IsNullOrWhiteSpace(TemperatureSensorWarningHighAbove) && !string.IsNullOrWhiteSpace(TemperatureSensorWarningLowBelow) && decimal.TryParse(TemperatureSensorCriticalHighAbove, out _) && decimal.TryParse(TemperatureSensorCriticalLowBelow, out _) && decimal.TryParse(TemperatureSensorWarningHighAbove, out _) && decimal.TryParse(TemperatureSensorWarningLowBelow, out _)))
            && (!IsHumiditySensorEnabled || (HumiditySensorNumber.HasValue && !string.IsNullOrWhiteSpace(HumiditySensorCriticalLowBelow) && !string.IsNullOrWhiteSpace(HumiditySensorCriticalHighAbove) && !string.IsNullOrWhiteSpace(HumiditySensorWarningHighAbove) && !string.IsNullOrWhiteSpace(HumiditySensorWarningLowBelow) && decimal.TryParse(HumiditySensorCriticalHighAbove, out _) && decimal.TryParse(HumiditySensorCriticalLowBelow, out _) && decimal.TryParse(HumiditySensorWarningHighAbove, out _) && decimal.TryParse(HumiditySensorWarningLowBelow, out _)))
            && (!IsCarbonDioxideSensorEnabled || (CarbonDioxideSensorNumber.HasValue && !string.IsNullOrWhiteSpace(CarbonDioxideSensorCriticalLowBelow) && !string.IsNullOrWhiteSpace(CarbonDioxideSensorCriticalHighAbove) && !string.IsNullOrWhiteSpace(CarbonDioxideSensorWarningHighAbove) && !string.IsNullOrWhiteSpace(CarbonDioxideSensorWarningLowBelow) && decimal.TryParse(CarbonDioxideSensorCriticalHighAbove, out _) && decimal.TryParse(CarbonDioxideSensorCriticalLowBelow, out _) && decimal.TryParse(CarbonDioxideSensorWarningHighAbove, out _) && decimal.TryParse(CarbonDioxideSensorWarningLowBelow, out _)));

        public Location[] Locations { get; private set; }
        public Room[] Rooms { get; private set; }


        public string IpAddress { get; set; }
        public string Port { get; set; }
        public Room SelectedRoom { get; set; }
        public Location SelectedLocation { get; set; }
        public string SerialNumber { get; set; }
        public string Name { get; set; }

        public bool IsTemperatureSensorEnabled { get; set; }
        public byte? TemperatureSensorNumber { get; set; }
        public string TemperatureSensorCriticalLowBelow { get; set; }
        public string TemperatureSensorWarningLowBelow { get; set; }
        public string TemperatureSensorWarningHighAbove { get; set; }
        public string TemperatureSensorCriticalHighAbove { get; set; }

        public bool IsHumiditySensorEnabled { get; set; }
        public byte? HumiditySensorNumber { get; set; }
        public string HumiditySensorCriticalLowBelow { get; set; }
        public string HumiditySensorWarningLowBelow { get; set; }
        public string HumiditySensorWarningHighAbove { get; set; }
        public string HumiditySensorCriticalHighAbove { get; set; }

        public bool IsCarbonDioxideSensorEnabled { get; set; }
        public byte? CarbonDioxideSensorNumber { get; set; }
        public string CarbonDioxideSensorCriticalLowBelow { get; set; }
        public string CarbonDioxideSensorWarningLowBelow { get; set; }
        public string CarbonDioxideSensorWarningHighAbove { get; set; }
        public string CarbonDioxideSensorCriticalHighAbove { get; set; }

        private async void Add()
        {
            if (!IsUiEnabled || !IsAddEnabled)
                return;

            var temperatureNumber = -1;
            var humidityNumber = -2;
            var carbonDioxideNumber = -3;

            if (this.SensorModules.Any(sm => sm.Name.Equals(Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                this.dialogBox.Show("Name is already in use");
                return;
            }

            try
            {
                this.IsUiEnabled = false;

                var sensors = new List<Sensor>();

                if (IsTemperatureSensorEnabled)
                {
                    if (!decimal.TryParse(TemperatureSensorCriticalHighAbove, out decimal criticalHighAbove))
                    {
                        this.dialogBox.Show("Temperature Sensor critical high above is not a valid number");
                        return;
                    }

                    if (!decimal.TryParse(TemperatureSensorCriticalLowBelow, out decimal criticalLowBelow))
                    {
                        this.dialogBox.Show("Temperature Sensor critical low below is not a valid number");
                        return;
                    }

                    if (!decimal.TryParse(TemperatureSensorWarningHighAbove, out decimal warningHighAbove))
                    {
                        this.dialogBox.Show("Temperature Sensor warning high above is not a valid number");
                        return;
                    }

                    if (!decimal.TryParse(TemperatureSensorWarningLowBelow, out decimal warningLowBelow))
                    {
                        this.dialogBox.Show("Temperature Sensor warning low below is not a valid number");
                        return;
                    }

                    if (!(criticalLowBelow < warningLowBelow && warningLowBelow < warningHighAbove && warningHighAbove < criticalHighAbove))
                    {
                        this.dialogBox.Show("Temperature Sensor warning/critical values not in sequential order");
                        return;
                    }

                    sensors.Add(new Sensor
                    {
                        CriticalHighAbove = criticalHighAbove,
                        CriticalLowBelow = criticalLowBelow,
                        Number = TemperatureSensorNumber.Value,
                        Type = SensorType.Temperature,
                        WarningHighAbove = warningHighAbove,
                        WarningLowBelow = warningLowBelow
                    });

                    temperatureNumber = TemperatureSensorNumber.Value;
                }
                if (IsHumiditySensorEnabled)
                {
                    if (!decimal.TryParse(HumiditySensorCriticalHighAbove, out decimal criticalHighAbove))
                    {
                        this.dialogBox.Show("Humidity Sensor critical high above is not a valid number");
                        return;
                    }

                    if (!decimal.TryParse(HumiditySensorCriticalLowBelow, out decimal criticalLowBelow))
                    {
                        this.dialogBox.Show("Humidity Sensor critical low below is not a valid number");
                        return;
                    }

                    if (!decimal.TryParse(HumiditySensorWarningHighAbove, out decimal warningHighAbove))
                    {
                        this.dialogBox.Show("Humidity Sensor warning high above is not a valid number");
                        return;
                    }

                    if (!decimal.TryParse(HumiditySensorWarningLowBelow, out decimal warningLowBelow))
                    {
                        this.dialogBox.Show("Humidity Sensor warning low below is not a valid number");
                        return;
                    }

                    if (!(criticalLowBelow < warningLowBelow && warningLowBelow < warningHighAbove && warningHighAbove < criticalHighAbove))
                    {
                        this.dialogBox.Show("Humidity Sensor warning/critical values not in sequential order");
                        return;
                    }
                    sensors.Add(new Sensor
                    {
                        CriticalHighAbove = criticalHighAbove,
                        CriticalLowBelow = criticalLowBelow,
                        Number = HumiditySensorNumber.Value,
                        Type = SensorType.Humidity,
                        WarningHighAbove = warningHighAbove,
                        WarningLowBelow = warningLowBelow
                    });

                    humidityNumber = HumiditySensorNumber.Value;
                }
                if (IsCarbonDioxideSensorEnabled)
                {
                    if (!decimal.TryParse(CarbonDioxideSensorCriticalHighAbove, out decimal criticalHighAbove))
                    {
                        this.dialogBox.Show("CarbonDioxide Sensor critical high above is not a valid number");
                        return;
                    }

                    if (!decimal.TryParse(CarbonDioxideSensorCriticalLowBelow, out decimal criticalLowBelow))
                    {
                        this.dialogBox.Show("CarbonDioxide Sensor critical low below is not a valid number");
                        return;
                    }

                    if (!decimal.TryParse(CarbonDioxideSensorWarningHighAbove, out decimal warningHighAbove))
                    {
                        this.dialogBox.Show("CarbonDioxide Sensor warning high above is not a valid number");
                        return;
                    }

                    if (!decimal.TryParse(CarbonDioxideSensorWarningLowBelow, out decimal warningLowBelow))
                    {
                        this.dialogBox.Show("CarbonDioxide Sensor warning low below is not a valid number");
                        return;
                    }

                    if (!(criticalLowBelow < warningLowBelow && warningLowBelow < warningHighAbove && warningHighAbove < criticalHighAbove))
                    {
                        this.dialogBox.Show("CarbonDioxide Sensor warning/critical values not in sequential order");
                        return;
                    }

                    sensors.Add(new Sensor
                    {
                        CriticalHighAbove = criticalHighAbove,
                        CriticalLowBelow = criticalLowBelow,
                        Number = CarbonDioxideSensorNumber.Value,
                        Type = SensorType.CarbonDioxide,
                        WarningHighAbove = warningHighAbove,
                        WarningLowBelow = warningLowBelow
                    });

                    carbonDioxideNumber = CarbonDioxideSensorNumber.Value;
                }

                if (sensors.Count < 1)
                {
                    this.dialogBox.Show("At least one sensor must be selected");
                    return;
                }

                if (humidityNumber == temperatureNumber || humidityNumber == carbonDioxideNumber || temperatureNumber == carbonDioxideNumber)
                {
                    this.dialogBox.Show("All sensors must have a different number");
                    return;
                }


                if (!ushort.TryParse(this.Port, out ushort port))
                {
                    this.dialogBox.Show("Invalid port specified.");
                    return;
                }
                
                if (!System.Net.IPAddress.TryParse(IpAddress, out _))
                {
                    this.dialogBox.Show("Invalid IP address specified.");
                    return;
                }
                var sensorModule = new SensorModuleModbusTcp
                {
                    IpAddress = this.IpAddress,
                    Location = this.SelectedLocation.Name,
                    Room = this.SelectedRoom.Name,
                    Name = this.Name,
                    Port = port,
                    SerialNumber = this.SerialNumber,
                    Sensors = sensors.ToArray()
                };
                var id = await this.apiClient.AddModbusTcp(sensorModule);
                sensorModule.Id = id;
                foreach (var sensor in sensors)
                    sensor.SensorModuleId = id;
                var modules = new List<SensorModuleModbusTcp>(this.SensorModules)
                {
                    sensorModule
                };
                this.SensorModules = modules.OrderBy(sm => sm.Name).ToArray();

                try
                {
                    hardwareProvider.LabelPrinter.Print(new SensorModuleLabelDefinition(this.SelectedSensorModule));
                }
                catch (Exception ex)
                {
                    this.dialogBox.Show("Failed to print label.", exception: ex);
                }
                this.ResetAddUi();
                this.dialogBox.Show("Sensor module added.");
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("Error deleting sensor module.", exception: ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }

        private async void Delete()
        {
            if (!IsUiEnabled || !IsSensorSelected)
                return;

            try
            {
                if (!this.dialogBox.Show("Are you sure you want to delete this module?", buttons: Enums.DialogBoxButtons.YesNo))
                {
                    return;
                }

                this.IsUiEnabled = false;
                var deletedModule = this.SelectedSensorModule;
                await this.apiClient.Delete(deletedModule.Id);
                this.SelectedSensorModule = null;
                this.SensorModules = this.SensorModules.Where(sm => sm != deletedModule).ToArray();
                this.dialogBox.Show("Sensor module deleted.");
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("Error deleting sensor module.", exception: ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }

        private void PrintLabel()
        {
            if (!IsUiEnabled || !IsSensorSelected)
                return;

            if (hardwareProvider?.LabelPrinter == null)
            {
                this.dialogBox.Show("No printer is attached to the system.");
                return;
            }

            try
            {
                hardwareProvider.LabelPrinter.Print(new SensorModuleLabelDefinition(this.SelectedSensorModule));
                this.dialogBox.Show("Label printed.");
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("Failed to print label.", exception: ex);
            }
        }

        public async override Task OnShow()
        {
            try
            {
                this.IsUiEnabled = false;
                this.SelectedSensorModule = null;
                this.SelectedLocation = null;
                this.SelectedRoom = null;
                this.ResetAddUi();
                this.SensorModules = await this.apiClient.GetModbusTcp();

                this.Locations = await this.locationApiClient.Get();
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("Error loading sensor modules.", exception: ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }
        }

        private void ResetAddUi()
        {
            this.IpAddress = null;
            this.Port = null;
            this.SelectedRoom = null;
            this.SelectedLocation = null;
            this.SerialNumber = null;
            this.Name = null;

            this.IsTemperatureSensorEnabled = false;
            this.TemperatureSensorNumber = null;
            this.TemperatureSensorCriticalLowBelow = null;
            this.TemperatureSensorWarningLowBelow = null;
            this.TemperatureSensorWarningHighAbove = null;
            this.TemperatureSensorCriticalHighAbove = null;

            this.IsHumiditySensorEnabled = false;
            this.HumiditySensorNumber = null;
            this.HumiditySensorCriticalLowBelow = null;
            this.HumiditySensorWarningLowBelow = null;
            this.HumiditySensorWarningHighAbove = null;
            this.HumiditySensorCriticalHighAbove = null;

            this.IsCarbonDioxideSensorEnabled = false;
            this.CarbonDioxideSensorNumber = null;
            this.CarbonDioxideSensorCriticalLowBelow = null;
            this.CarbonDioxideSensorWarningLowBelow = null;
            this.CarbonDioxideSensorWarningHighAbove = null;
            this.CarbonDioxideSensorCriticalHighAbove = null;
        }

        private void OnSelectedLocationChanged()
        {
            this.Rooms = this.SelectedLocation?.Rooms;
        }
    }
}