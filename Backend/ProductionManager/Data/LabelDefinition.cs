namespace Solarponics.ProductionManager.Data
{
    public class LabelDefinition
    {
        public string Barcode { get; }

        public string Text { get; }

        public LabelDefinition(string text, string barcode)
        {
            Text = text;
            Barcode = barcode;
        }
    }
}