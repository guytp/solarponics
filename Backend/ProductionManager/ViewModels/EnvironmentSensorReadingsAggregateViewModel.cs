using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ApiClients;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Solarponics.ProductionManager.ViewModels
{
    public class EnvironmentSensorReadingsAggregateViewModel : ViewModelBase, IEnvironmentSensorReadingsAggregateViewModel
    {
        private readonly IDialogBox dialogBox;
        private readonly ISensorModuleApiClient sensorModuleApiClient;
        private readonly ISensorReadingApiClient sensorReadingApiClient;
        private SensorModule[] sensorModules;
        private bool isShown;
        private DateTime? lastUpdated;
        private readonly Timer timerUpdate;
        private readonly Timer timerCounter;

        public EnvironmentSensorReadingsAggregateViewModel(IDialogBox dialogBox, ISensorModuleApiClient sensorModuleApiClient, ISensorReadingApiClient sensorReadingApiClient, ILoggedInButtonsViewModel loggedInButtonsViewModel)
        {
            this.dialogBox = dialogBox;
            this.sensorModuleApiClient = sensorModuleApiClient;
            this.sensorReadingApiClient = sensorReadingApiClient;
            this.LoggedInButtonsViewModel = loggedInButtonsViewModel;
            Timeframes = new[]
            {
                AggregateTimeframe.OneMinute,
                AggregateTimeframe.FiveMinutes,
                AggregateTimeframe.FifteenMinutes,
                AggregateTimeframe.ThirtyMinutes,
                AggregateTimeframe.OneHour,
                AggregateTimeframe.FourHours,
                AggregateTimeframe.TwelveHours,
                AggregateTimeframe.OneDay
            };
            this.timerUpdate = new Timer(30000)
            {
                AutoReset = false,
                Enabled = false
            };
            timerUpdate.Elapsed += OnTimerUpdateElapsed;
            this.timerCounter = new Timer(500)
            {
                AutoReset = true,
                Enabled = false
            };
            timerCounter.Elapsed += OnTimerCounterElapsed;
            this.SelectedTimeframe = AggregateTimeframe.ThirtyMinutes;
        }

        public ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }

        public string[] Locations { get; private set; }

        public string SelectedLocation { get; set; }

        public SensorRoomGroupAggregate[] SensorsGroupedByRoom { get; private set; }

        public AggregateTimeframe SelectedTimeframe { get; set; }

        public AggregateTimeframe[] Timeframes { get; }

        public bool IsUiEnabled { get; private set; }

        public string LastUpdatedAgo
        {
            get
            {
                if (!lastUpdated.HasValue)
                    return "Loading data...";

                var interval = DateTime.UtcNow.Subtract(lastUpdated.Value);
                int unit;
                string unitName;
                if (interval.Minutes < 1)
                {
                    unit = interval.Seconds;
                    if (unit < 40)
                    {
                        return "Recently updated";
                    }
                    unitName = "second";
                }
                else if (interval.Hours < 1)
                {
                    unit = interval.Minutes;
                    unitName = "minute";
                }
                else if (interval.Days < 1)
                {
                    unit = interval.Hours;
                    unitName = "hour";
                }
                else
                {
                    unit = interval.Days;
                    unitName = "day";
                }

                return $"Last updated {unit} {unitName}" + (unit > 1 ? "s" : string.Empty) + " ago";
            }
        }

        public string LastUpdatedColour
        {
            get
            {
                if (!lastUpdated.HasValue)
                    return "#F8A725";
                var interval = DateTime.UtcNow.Subtract(lastUpdated.Value).Minutes;
                return interval > 0 ? "#FF3939" : "White";
            }
        }

        public async override Task OnShow()
        {
            this.isShown = true;
            this.timerCounter.Enabled = true;
            try
            {
                this.IsUiEnabled = false;
                this.SelectedLocation = null;
                this.Locations = null;
                this.sensorModules = await this.sensorModuleApiClient.Get() ?? new SensorModule[0];
                this.Locations = this.sensorModules.Select(sm => sm.Location).Distinct().OrderBy(s => s).ToArray();
                if (this.Locations.Length < 1)
                {
                    this.dialogBox.Show("No sensors have been configured");
                    return;
                }
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("Unexpected error refreshing list of sensors", exception: ex);
            }
            finally
            {
                this.IsUiEnabled = true;
            }

            this.SelectedLocation = this.Locations[0];
        }

        public override Task OnHide()
        {
            this.isShown = false;
            this.timerCounter.Enabled = false;
            return base.OnHide();
        }

        private async void OnTimerUpdateElapsed(object sender, ElapsedEventArgs e)
        {
            await this.RefreshSensorData(false);
        }

        private void OnTimerCounterElapsed(object sender, ElapsedEventArgs e)
        {
            base.OnPropertyChanged(nameof(LastUpdatedColour));
            base.OnPropertyChanged(nameof(LastUpdatedAgo));
        }

#pragma warning disable IDE0051 // Remove unused private members
        private async void OnSelectedLocationChanged()
#pragma warning restore IDE0051 // Remove unused private members
        {
            this.SensorsGroupedByRoom = null;
            this.timerUpdate.Enabled = false;
            await this.RefreshSensorData(true);
        }

#pragma warning disable IDE0051 // Remove unused private members
        private async void OnSelectedTimeframeChanged()
#pragma warning restore IDE0051 // Remove unused private members
        {
            if (!this.isShown)
                return;

            this.SensorsGroupedByRoom = null;
            this.timerUpdate.Enabled = false;
            await this.RefreshSensorData(true);
        }

        private async Task RefreshSensorData(bool showError)
        {
            if (!System.Windows.Application.Current.Dispatcher.CheckAccess())
            {
                var act = new Action(async () => await RefreshSensorData(showError));
                await System.Windows.Application.Current.Dispatcher.InvokeAsync(act);
                return;
            }

            try
            {
                if (showError)
                    this.IsUiEnabled = false;
                var sensorModulesToCheck = this.sensorModules.Where(sm => sm.Location == this.SelectedLocation).OrderBy(sm => sm.Name).ToArray();
                var rooms = sensorModulesToCheck.Select(sm => sm.Room).Distinct().OrderBy(s => s).ToArray();
                var groups = new List<SensorRoomGroupAggregate>();
                foreach (var room in rooms)
                {
                    var groupedSensors = new List<SensorReadingAggregate>();

                    foreach (var sensorModule in sensorModulesToCheck.Where(sm => sm.Room == room && sm.Location == this.SelectedLocation))
                    {
                        foreach (var sensor in sensorModule.Sensors)
                        {
                            var readings = await this.sensorReadingApiClient.GetAggregate(sensor.Id, SelectedTimeframe);
                            if (readings == null)
                                continue;

                            groupedSensors.Add(new SensorReadingAggregate(sensorModule.Name, sensor.Type, readings, sensor.CriticalLowBelow, sensor.WarningLowBelow, sensor.WarningHighAbove, sensor.CriticalHighAbove));
                        }
                    }

                    if (groupedSensors.Count > 0)
                        groups.Add(new SensorRoomGroupAggregate(room, groupedSensors.ToArray()));
                }

                this.SensorsGroupedByRoom = groups.ToArray();
                if (this.SensorsGroupedByRoom.Length < 1 && showError && SelectedLocation != null)
                        this.dialogBox.Show("There are no sensors in this location");
                
                this.lastUpdated = DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                if (showError)
                    this.dialogBox.Show("Unexpected error loading sensor data", exception: ex);
            }
            finally
            {
                this.timerUpdate.Enabled = isShown;
                if (showError)
                    this.IsUiEnabled = true;
            }

        }

    }
}