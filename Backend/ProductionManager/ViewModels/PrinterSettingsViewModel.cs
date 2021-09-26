using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Commands;
using Solarponics.ProductionManager.Core;
using System.Collections.Generic;
using System.Printing;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Solarponics.ProductionManager.ViewModels
{
    public class PrinterSettingsViewModel : ViewModelBase, IPrinterSettingsViewModel
    {
        public PrinterSettingsViewModel(PrinterSettings settings)
        {
            this.ResetCommand = new RelayCommand(_ => this.Reset());

            var printServer = new LocalPrintServer();
            var queueName = QueueName;
            var queues = new List<string>();
            foreach (var queue in printServer.GetPrintQueues())
            {
                queues.Add(queue.FullName);
            }
            QueueNames = queues.ToArray();

            if (settings != null)
            {
                this.DriverName = settings.DriverName;
                this.QueueName = settings.QueueName;
            }
            this.IsReset = settings == null;
            this.DriverNames = new [] { "ZplLabelPrinter" };
        }

        public bool IsValid => IsReset || (!string.IsNullOrEmpty(this.DriverName) && !string.IsNullOrEmpty(this.QueueName)); 

        public bool IsReset { get; private set; }

        public ICommand ResetCommand { get; }

        public string QueueName { get; set; }

        public string[] QueueNames { get; private set; }
        
        public string[] DriverNames { get; }

        public string DriverName { get; set; }

        private void Reset()
        {
            this.DriverName = null;
            this.QueueName = null;
            this.IsReset = true;
        }

        protected void OnDriverNameChanged()
        {
            this.IsReset = false;
        }

        protected void OnQueueNameChanged()
        {
            this.IsReset = false;
        }
    }
}