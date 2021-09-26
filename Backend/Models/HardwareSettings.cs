namespace Solarponics.Models
{
    public class HardwareSettings
    {
        public BarcodeScannerSettings BarcodeScanner { get; set; }

        public LabelPrinterSettings LabelPrinter { get; set; }

        public ScaleSettings Scale { get; set; }
    }
}