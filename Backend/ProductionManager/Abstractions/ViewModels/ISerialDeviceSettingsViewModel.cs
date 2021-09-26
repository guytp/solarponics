using Solarponics.Models;
using System.ComponentModel;
using System.Windows.Input;

namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface ISerialDeviceSettingsViewModel : INotifyPropertyChanged, IViewModel
    {
        bool IsReset { get; }
        bool IsValid { get; }

        ICommand ResetCommand { get; }
        
        string DriverName { get; set; }
        string[] DriverNames { get; }

        string SerialPort { get; set; }
        
        int? BaudRate { get; set; }
        
        Parity? Parity { get; set; }
            
        int? DataBits { get; set; }

        StopBits? StopBits { get; set; }

        string[] SerialPortOptions { get; }
        int[] BaudRateOptions { get; }
        Parity[] ParityOptions { get; }
        int[] DataBitOptions { get; }
        StopBits[] StopBitsOptions { get; }
    }
}