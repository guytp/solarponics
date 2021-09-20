using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Abstractions.Views
{
    public interface ISetupSupplierView : IView
    {
        ISetupSupplierViewModel SetupSupplierViewModel { get; }
    }
}