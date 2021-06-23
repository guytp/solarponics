using System.Windows.Input;

namespace Solarponics.ProductionManager.Abstractions
{
    public interface IView
    {
        IViewModel ViewModel { get; }
        void HandlePreviewKeyDown(KeyEventArgs e);
    }
}