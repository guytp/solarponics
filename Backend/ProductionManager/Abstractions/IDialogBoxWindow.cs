using Solarponics.ProductionManager.Enums;

namespace Solarponics.ProductionManager.Abstractions
{
    public interface IDialogBoxWindow
    {
        bool? ShowDialog();
        DialogBoxButton Button { get; }
    }
}