using System;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.Factories;
using Solarponics.ProductionManager.Abstractions.ViewModels;
using Solarponics.ProductionManager.Enums;

namespace Solarponics.ProductionManager.Domain
{
    public class DialogBox : IDialogBox
    {
        private readonly IDialogBoxWindowViewModelFactory viewModelFactory;
        private readonly IDialogBoxWindowFactory windowFactory;

        public DialogBox(IDialogBoxWindowFactory windowFactory, IDialogBoxWindowViewModelFactory viewModelFactory)
        {
            this.windowFactory = windowFactory;
            this.viewModelFactory = viewModelFactory;
        }

        public bool Show(string message, Exception exception = null, DialogBoxButtons buttons = DialogBoxButtons.Ok,
            string title = "Solarponics")
        {
            IDialogBoxWindowViewModel viewModel;
            if (buttons == DialogBoxButtons.Ok)
            {
                viewModel = viewModelFactory.Create(title, message, exception?.ToString(), "Ok");
            }
            else
            {
                string firstButton;
                string secondButton;
                switch (buttons)
                {
                    case DialogBoxButtons.OkCancel:
                        firstButton = "Ok";
                        secondButton = "No";
                        break;
                    case DialogBoxButtons.YesNo:
                        firstButton = "Yes";
                        secondButton = "No";
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(buttons), buttons, null);
                }

                viewModel = viewModelFactory.Create(title, message, exception?.ToString(), firstButton, secondButton);
            }

            var window = windowFactory.Create(viewModel);
            window.ShowDialog();
            return window.Button == DialogBoxButton.First;
        }
    }
}