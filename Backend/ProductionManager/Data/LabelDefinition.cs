namespace Solarponics.ProductionManager.Data
{
    public class LabelDefinition
    {
        public string Barcode { get; protected set; }

        public string Text { get; protected set; }
        public BarcodeSize BarcodeSize { get; protected set; }
        public TextSize TextSize { get; protected set; }
        public int? MaxTextWidth { get; protected set; }
        public int? MaxLinesToPrint { get; protected set; }

        public LabelDefinition(string text, string barcode = null, BarcodeSize barcodeSize = BarcodeSize.Medium, TextSize textSize = TextSize.Large, int? maxTextWidth = null, int? maxLinesToPrint = null)
        {
            Text = text;
            Barcode = barcode;
            BarcodeSize = barcodeSize;
            TextSize = textSize;
            MaxTextWidth = maxTextWidth;
            MaxLinesToPrint = maxLinesToPrint;
        }

        protected LabelDefinition()
        {
        }
    }
}