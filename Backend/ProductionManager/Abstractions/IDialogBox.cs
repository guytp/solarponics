using System;
using Solarponics.ProductionManager.Enums;

namespace Solarponics.ProductionManager.Abstractions
{
    public interface IDialogBox
    {
        bool Show(string message, Exception exception = null, DialogBoxButtons buttons = DialogBoxButtons.Ok, string title = "Solarponics");
    }
}