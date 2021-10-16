using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions.Views
{
    public interface ISetupSensorModuleModbusTcpView : IView
    {
        ISetupSensorModuleModbusTcpViewModel SetupSensorModuleModbusTcpViewModel { get; }
    }
}