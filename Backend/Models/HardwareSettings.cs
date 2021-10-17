namespace Solarponics.Models
{
    public class HardwareSettings
    {
        public BarcodeScannerSettings BarcodeScanner { get; set; }

        public LabelPrinterSettings LabelPrinterSmall { get; set; }
        public LabelPrinterSettings LabelPrinterLarge { get; set; }

        public ScaleSettings Scale { get; set; }
    }
}