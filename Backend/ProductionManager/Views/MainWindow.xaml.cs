using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Solarponics.ProductionManager.Abstractions;

namespace Solarponics.ProductionManager.Views
{
    public partial class MainWindow : IMainWindow
    {
        public MainWindow(IMainWindowViewModel viewModel, INavigator navigator, ILoginView loginView)
        {
            navigator.SetLoginView(loginView);
            InitializeComponent();
            DataContext = viewModel;

            if ((int) SystemParameters.PrimaryScreenWidth == 5120)
            {
                WindowState = WindowState.Normal;
                Width = SystemParameters.PrimaryScreenWidth / 2;
                Height = SystemParameters.MaximizedPrimaryScreenHeight;
                Top = SystemParameters.WorkArea.Top;
                Left = Width / 2;
            }
            else
            {
                WindowState = WindowState.Maximized;
            }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            ((IMainWindowViewModel) DataContext).ActiveView?.HandlePreviewKeyDown(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
        }
    }
}