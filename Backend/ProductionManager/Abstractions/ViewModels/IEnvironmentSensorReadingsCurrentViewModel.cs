using Solarponics.ProductionManager.Data;

namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface IEnvironmentSensorReadingsCurrentViewModel : IViewModel
    {
        bool IsUiEnabled { get; }

        ILoggedInButtonsViewModel LoggedInButtonsViewModel { get; }

        string[] Locations { get; }

        string SelectedLocation { get; set; }

        SensorRoomGroupCurrent[] SensorsGroupedByRoom { get; }

        string LastUpdatedAgo { get; }
        string LastUpdatedColour { get; }
    }
}