using Solarponics.ProductionManager.Domain;
using Solarponics.ProductionManager.Factories;
using System;
using System.Diagnostics;
using System.Linq;
using Solarponics.ProductionManager.Core.Enums;

namespace Solarponics.ProductionManager
{
    public static class Program
    {
        [STAThread]
        public static void Main()
        {
            try
            {
                var proc = Process.GetCurrentProcess();
                var procs = Process.GetProcesses();
                if (procs.Any(p => p.Id != proc.Id && p.ProcessName == proc.ProcessName))
                {
                    var res = new DialogBox(new DialogBoxWindowFactory(), new DialogBoxWindowViewModelFactory()).Show("Production manager is already running.  Do you want to terminate the existing instance?  If you select Yes the existing production manager will be stopped a new instance will run.  If you select No the existing instance will remain.", buttons: DialogBoxButtons.YesNo);
                    if (!res)
                        return;

                    var procsToTerminate = procs.Where(p => p.Id != proc.Id && p.ProcessName == proc.ProcessName);
                    foreach (var procToTerminate in procsToTerminate)
                        procToTerminate.Kill();

                    System.Threading.Thread.Sleep(1000);
                }
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