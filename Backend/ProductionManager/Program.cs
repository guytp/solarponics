using System;

namespace Solarponics.ProductionManager
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            try
            {
                var app = new App();
                app.Run();
            }
            catch (Exception ex)
            {
                App.HandleUnhandledError(ex);
            }
        }
    }
}