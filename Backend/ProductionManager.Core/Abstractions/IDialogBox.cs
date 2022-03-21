using System;
using Solarponics.ProductionManager.Core.Enums;

namespace Solarponics.ProductionManager.Core.Abstractions
{
    public interface IDialogBox
    {
        bool Show(string message, Exception exception = null, DialogBoxButtons buttons = DialogBoxButtons.Ok, string title = "Solarponics");
    }
}