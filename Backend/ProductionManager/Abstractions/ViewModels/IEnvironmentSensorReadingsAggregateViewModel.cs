using Solarponics.Models;
using Solarponics.ProductionManager.Data;

namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface IEnvironmentSensorReadingsAggregateViewModel : IViewModel
    {
        bool IsUiEnabled { get; }

        ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }

        string[] Locations { get; }

        string SelectedLocation { get; set; }

        SensorRoomGroupAggregate[] SensorsGroupedByRoom { get; }

        string LastUpdatedAgo { get; }
        string LastUpdatedColour { get; }

        AggregateTimeframe SelectedTimeframe { get; set; }

        AggregateTimeframe[] Timeframes { get; }
    }
}