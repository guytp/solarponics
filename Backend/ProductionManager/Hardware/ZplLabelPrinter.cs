using SharpZebra;
using SharpZebra.Commands;
using Solarponics.Models;
using Solarponics.ProductionManager.Abstractions;
using Solarponics.ProductionManager.Abstractions.Hardware;
using Solarponics.ProductionManager.Data;
using System;
using System.Collections.Generic;

namespace Solarponics.ProductionManager.Hardware
{
    public class ZplLabelPrinter : ILabelPrinter, IDisposable
    {
        private readonly SharpZebra.Printing.PrinterSettings settings;
        private readonly IDialogBox dialogBox;
        private SpoolPrinter printer;
        private const int BarcodeHeight = 40;
        private const int StartTop = 20;
        private const int Left = 20;
        private const int VerticalSeparation = 40;
        private const int TextHeight = 20;

        public ZplLabelPrinter(LabelPrinterSettings settings, IDialogBox dialogBox)
        {
            this.dialogBox = dialogBox;
            this.settings = new SharpZebra.Printing.PrinterSettings
            {
                PrinterName = settings.QueueName
            };
        }

        public void Start()
        {
            this.printer = new SpoolPrinter(this.settings);
        }

        public void Dispose()
        {
            this.printer = null;
        }

        public void Print(LabelDefinition label)
        {
            var buffer = new List<byte>();
            var top = StartTop;


            if (!string.IsNullOrEmpty(label.Barcode))
            {
                buffer.AddRange(ZPLCommands.BarCodeWrite(Left, top, BarcodeHeight, ElementDrawRotation.NO_ROTATION, new Barcode
                {
                    Type = BarcodeType.CODE128_AUTO,
                    BarWidthNarrow = label.BarcodeSize == BarcodeSize.Small ? 1 : 2
                }, true, label.Barcode));
                top += BarcodeHeight;
            }

            if (!string.IsNullOrEmpty(label.Text))
            {
                var lines = label.Text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (buffer.Count > 0)
                {
                    top += VerticalSeparation;
                }

                foreach (var line in lines)
                {
                    buffer.AddRange(ZPLCommands.TextWrite(Left, top, ElementDrawRotation.NO_ROTATION, TextHeight, line));
                    top += TextHeight;
                }
            }

            buffer.AddRange(ZPLCommands.PrintBuffer());

            if (buffer.Count == 0)
            {
                this.dialogBox.Show("Label contained nothing to print.");
                return;
            }

            try
            {
                buffer.InsertRange(0, System.Text.Encoding.ASCII.GetBytes("^XA"));
                this.printer.Print(buffer.ToArray());
            }
            catch (Exception ex)
            {
                this.dialogBox.Show("Error printing to Zebra printer.", exception: ex);
            }
        }
    }
}