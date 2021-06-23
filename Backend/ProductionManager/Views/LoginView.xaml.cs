using System.Windows.Input;
using Solarponics.ProductionManager.Abstractions;

namespace Solarponics.ProductionManager.Views
{
    public partial class LoginView : ILoginView
    {
        private bool isUserIdSelected = true;

        public LoginView(ILoginViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }

        public void HandlePreviewKeyDown(KeyEventArgs e)
        {
            OnPreviewKeyDown(e);
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            var vm = ((ILoginViewModel) ViewModel);

            if (e.Key == Key.Enter || e.Key == Key.Return)
            {
                if (isUserIdSelected)
                {
                    isUserIdSelected = false;
                }
                else if (vm.IsLoginEnabled)
                {
                    vm.LoginCommand.Execute(null);
                }

                return;
            }

            if (e.Key == Key.Escape)
            {
                isUserIdSelected = true;
                vm.Pin = null;
                vm.UserId = null;
                return;
            }

            if (e.Key == Key.Back)
            {
                var existing = isUserIdSelected ? vm.UserId : vm.Pin;
                if (!string.IsNullOrEmpty(existing))
                {
                    existing = existing[..^1];
                }

                if (isUserIdSelected)
                    vm.UserId = existing;
                else
                    vm.Pin = existing;
            }

            if (!isUserIdSelected && string.IsNullOrEmpty(vm.UserId))
                isUserIdSelected = true;

            char c;
            switch (e.Key)
            {
                case Key.D0:
                case Key.NumPad0:
                    c = '0';
                    break;
                case Key.D1:
                case Key.NumPad1:
                    c = '1';
                    break;
                case Key.D2:
                case Key.NumPad2:
                    c = '2';
                    break;
                case Key.D3:
                case Key.NumPad3:
                    c = '3';
                    break;
                case Key.D4:
                case Key.NumPad4:
                    c = '4';
                    break;
                case Key.D5:
                case Key.NumPad5:
                    c = '5';
                    break;
                case Key.D6:
                case Key.NumPad6:
                    c = '6';
                    break;
                case Key.D7:
                case Key.NumPad7:
                    c = '7';
                    break;
                case Key.D8:
                case Key.NumPad8:
                    c = '8';
                    break;
                case Key.D9:
                case Key.NumPad9:
                    c = '9';
                    break;
                default:
                    return;
            }

            if (isUserIdSelected && (vm.UserId == null || vm.UserId.Length < 4))
                vm.UserId = vm.UserId == null ? c.ToString() : vm.UserId + c;
            else if (!isUserIdSelected && (vm.Pin == null || vm.Pin.Length < 4))
                vm.Pin = vm.Pin == null ?  c.ToString() : vm.Pin + c;
        }

        public IViewModel ViewModel => DataContext as ILoginViewModel;
    }
}