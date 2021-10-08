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
        private const int VerticalSeparation = 20;
        private const int TextHeightSmall = 5;
        private const int TextHeightMedium = 10;
        private const int TextHeightLarge = 20;

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
                var lines = label.Text.Replace('\t', ' ').Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                if (buffer.Count > 0)
                {
                    top += VerticalSeparation;
                }

                var textHeight = label.TextSize switch
                {
                    TextSize.Small => TextHeightSmall,
                    TextSize.Medium => TextHeightMedium,
                    TextSize.Large => TextHeightLarge,
                    _ => throw new NotImplementedException(),
                };

                foreach (var line in lines)
                {
                    var partsToPrint = SplitLineToParts(line, label.MaxTextWidth);
                    foreach (var part in partsToPrint)
                    {
                        buffer.AddRange(ZPLCommands.TextWrite(Left, top, ElementDrawRotation.NO_ROTATION, textHeight, part));
                        top += (int)(textHeight * 2.2);
                    }
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

        private string[] SplitLineToParts(string line, int? maxTextWidth)
        {
            if (!maxTextWidth.HasValue || line.Length <= maxTextWidth.Value)
            {
                return new[] { line };
            }

            var parts = new List<string>();
            var words = line.Split(new char[] { ' ', '\t' });
            var currentPart = string.Empty;
            foreach (var word in words)
            {
                var currentPartAfterAddition = currentPart + (currentPart == string.Empty ? string.Empty : " ") + word;
                if (currentPartAfterAddition.Length < maxTextWidth.Value)
                {
                    currentPart = currentPartAfterAddition;
                    continue;
                }

                if (currentPartAfterAddition == word)
                {
                    parts.Add(currentPartAfterAddition);
                    currentPart = string.Empty;
                    continue;
                }

                parts.Add(currentPart);
                currentPart = word;
            }

            if (currentPart != string.Empty)
                parts.Add(currentPart);

            return parts.ToArray();
        }
    }
}