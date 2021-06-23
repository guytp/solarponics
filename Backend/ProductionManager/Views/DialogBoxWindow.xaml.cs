using System.Windows.Input;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Enums;

namespace Solarponics.ProductionManager.Views
{
    public partial class DialogBoxWindow : IDialogBoxWindow
    {
        public DialogBoxWindow(IDialogBoxWindowViewModel viewModel)
        {
            InitializeComponent();
            MouseLeftButtonDown += (sender, args) => DragMove();
            DataContext = viewModel;
            viewModel.ButtonPressed += (sender, args) =>
            {
                DialogResult = args.Button == DialogBoxButton.First;
                Button = args.Button;
                Close();
            };
        }

        public DialogBoxButton Button { get; private set; }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            var vm = (IDialogBoxWindowViewModel) DataContext;
            if (e.Key == Key.Escape)
                (vm.IsDualButtons ? vm.SecondButtonCommand : vm.FirstButtonCommand).Execute(null);
            else if (e.Key == Key.Enter || e.Key == Key.Return)
                vm.FirstButtonCommand.Execute(null);
        }
    }
}