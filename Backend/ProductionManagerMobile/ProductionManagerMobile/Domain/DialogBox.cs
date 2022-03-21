using Solarponics.ProductionManager.Core.Abstractions;
using Solarponics.ProductionManager.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Solarponics.ProductionManagerMobile.Domain
{
    public class DialogBox : IDialogBox
    {
        public bool Show(string message, Exception exception = null, DialogBoxButtons buttons = DialogBoxButtons.Ok, string title = "Solarponics")
        {
            bool result;
            if (buttons == DialogBoxButtons.Ok)
            {
                Shell.Current.CurrentPage.DisplayAlert(title, message, "Ok");
                result = true;
            }
            else if (buttons == DialogBoxButtons.YesNo)
                result = Shell.Current.CurrentPage.DisplayAlert(title, message, "Yes", "No").Result;
            else
                result = Shell.Current.CurrentPage.DisplayAlert(title, message, "Ok", "Cancel").Result;
            return result;
        }
    }
}