using System.Windows.Controls;
using System.Windows.Input;
using Solarponics.ProductionManager.Abstractions;

namespace Solarponics.ProductionManager.Core
{
    public class ViewBase : UserControl, IView
    {
        public IViewModel ViewModel => (IViewModel) DataContext;

        public virtual void HandlePreviewKeyDown(KeyEventArgs e)
        {
        }
    }
}