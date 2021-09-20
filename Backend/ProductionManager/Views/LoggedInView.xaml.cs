using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ViewModels;

namespace Solarponics.ProductionManager.Views
{
    public partial class LoggedInView : ILoggedInView
    {
        public LoggedInView(ILoggedInViewModel viewModel)
        {
            InitializeComponent();
            this.DataContextChanged += OnDataContextChanged;
            DataContext = viewModel;
        }

        private void OnDataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            (e.NewValue as ILoggedInViewModel).LoggedInButtonsViewModel.HomeView = this;
        }

        public ILoggedInViewModel LoggedInViewModel => (ILoggedInViewModel) ViewModel;
    }
}