using System.Windows.Input;
using Solarponics.ProductionManager.Abstractions;

namespace Solarponics.ProductionManager.Views
{
    public partial class LoggedInView : ILoggedInView
    {
        public LoggedInView(ILoggedInViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }


        public ILoggedInViewModel LoggedInViewModel => (ILoggedInViewModel) ViewModel;
    }
}