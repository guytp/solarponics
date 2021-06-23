using Solarponics.ProductionManager.Abstractions;

namespace Solarponics.ProductionManager.EventArgs
{
    public class ViewEventArgs
    {
        public ViewEventArgs(IView view)
        {
            View = view;
        }

        public IView View { get; }
    }
}