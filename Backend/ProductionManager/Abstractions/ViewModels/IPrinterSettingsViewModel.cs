using System.ComponentModel;
using System.Windows.Input;

namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface IPrinterSettingsViewModel : INotifyPropertyChanged, IViewModel
    {
        bool IsValid { get; }
        bool IsReset { get; }
        ICommand ResetCommand { get; }
        string[] DriverNames { get; }
        string DriverName { get; set; }
        string QueueName { get; set; }
        string[] QueueNames { get; }
    }
}