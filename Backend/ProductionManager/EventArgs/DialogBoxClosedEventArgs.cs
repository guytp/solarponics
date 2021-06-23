using Solarponics.ProductionManager.Enums;

namespace Solarponics.ProductionManager.EventArgs
{
    public class DialogBoxClosedEventArgs : System.EventArgs
    {
        public DialogBoxClosedEventArgs(DialogBoxButton button)
        {
            Button = button;
        }

        public DialogBoxButton Button { get; }
    }
}