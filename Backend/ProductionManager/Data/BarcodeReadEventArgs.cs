namespace Solarponics.ProductionManager.Data
{
    public class BarcodeReadEventArgs
    {
        public string Barcode { get; }

        public BarcodeReadEventArgs(string data)
        {
            Barcode = data;
        }
    }
}