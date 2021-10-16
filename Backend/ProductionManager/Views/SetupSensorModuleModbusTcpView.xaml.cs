using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;

namespace Solarponics.ProductionManager.Views
{
    public partial class SetupSensorModuleModbusTcpView : ISetupSensorModuleModbusTcpView
    {
        public SetupSensorModuleModbusTcpView(ISetupSensorModuleModbusTcpViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public ISetupSensorModuleModbusTcpViewModel SetupSensorModuleModbusTcpViewModel => DataContext as ISetupSensorModuleModbusTcpViewModel;
    }
}