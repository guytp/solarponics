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
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
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