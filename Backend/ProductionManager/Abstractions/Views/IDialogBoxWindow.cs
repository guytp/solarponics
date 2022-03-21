using Solarponics.ProductionManager.Core.Enums;

namespace Solarponics.ProductionManager.Abstractions.Views
{
    public interface IDialogBoxWindow
    {
        bool? ShowDialog();
        DialogBoxButton Button { get; }
    }
}