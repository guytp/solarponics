using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Abstractions.Views;

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
            HandleKey(e.Key);
        }

        private void HandleKey(Key key)
        {
            var vm = ((ILoginViewModel)ViewModel);

            if (key == Key.Enter || key == Key.Return)
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

            if (key == Key.Escape)
            {
                isUserIdSelected = true;
                vm.Pin = null;
                vm.UserId = null;
                return;
            }

            if (key == Key.Back)
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
            switch (key)
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
                vm.Pin = vm.Pin == null ? c.ToString() : vm.Pin + c;
        }

        public IViewModel ViewModel => DataContext as ILoginViewModel;

        private void OnNumberButtonClick(object sender, RoutedEventArgs e)
        {
            Key key;
            var b = (sender as Button);
            if (b == null)
                return;
            var buttonText = b.Name.Substring(b.Name.Length - 1, 1);
            if (buttonText == "0")
                key = Key.D0;
            else if (buttonText == "1")
                key = Key.D1;
            else if (buttonText == "2")
                key = Key.D2;
            else if (buttonText == "3")
                key = Key.D3;
            else if (buttonText == "4")
                key = Key.D4;
            else if (buttonText == "5")
                key = Key.D5;
            else if (buttonText == "6")
                key = Key.D6;
            else if (buttonText == "7")
                key = Key.D7;
            else if (buttonText == "8")
                key = Key.D8;
            else if (buttonText == "9")
                key = Key.D9;
            else if (buttonText == "E")
                key = Key.Enter;
            else if (buttonText == "X")
                key = Key.Escape;
            else
                return;
            HandleKey(key);
        }
    }
}