using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.Hardware;
using Solarponics.ProductionManager.Data;
using System;

namespace Solarponics.ProductionManager.Hardware
{
    public class SerialScale: SerialDeviceBase, IScale
    {
        private readonly IDialogBox dialogBox;
        public SerialScale(ScaleSettings settings, IDialogBox dialogBox)
            : base(settings, new byte[] { (byte)'\r', (byte)'\n' })
        {
            this.dialogBox = dialogBox;
        }

        public event EventHandler<WeightReadEventArgs> WeightRead;

        protected override void StringRead(string str)
        {
            var d = System.Windows.Application.Current?.Dispatcher;

            var act = new Action(() =>
            {
            decimal weight;
            try
            {
                weight = decimal.Parse(str);
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("Error parsing weight from scales: " + str + " to a number.", exception: ex);
                return;
            }

            this.WeightRead?.Invoke(this, new WeightReadEventArgs(weight));
            });

            if (d == null || d.CheckAccess())
                act();
            else
                d.Invoke(act);
        }
    }
}