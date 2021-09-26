namespace Solarponics.ProductionManager.Data
{
    public class LabelDefinition
    {
        public string Barcode { get; }

        public string Text { get; }
        public BarcodeSize BarcodeSize { get; }

        public LabelDefinition(string text, string barcode = null, BarcodeSize barcodeSize = BarcodeSize.Medium)
        {
            Text = text;
            Barcode = barcode;
            BarcodeSize = barcodeSize;
        }
    }
}