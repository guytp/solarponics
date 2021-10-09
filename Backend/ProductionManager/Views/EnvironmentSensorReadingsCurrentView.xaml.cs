using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;

namespace Solarponics.ProductionManager.Views
{
    public partial class EnvironmentSensorReadingsCurrentView : IEnvironmentSensorReadingsCurrentView
    {
        public EnvironmentSensorReadingsCurrentView(IEnvironmentSensorReadingsCurrentViewModel viewModel)
        {
            InitializeComponent();
            this.DataContext = viewModel;
        }

        public IEnvironmentSensorReadingsCurrentViewModel EnvironmentSensorReadingsCurrentViewModel => DataContext as IEnvironmentSensorReadingsCurrentViewModel;
    }
}