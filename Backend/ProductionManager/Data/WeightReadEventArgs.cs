namespace Solarponics.ProductionManager.Data
{
    public class WeightReadEventArgs
    {
        public decimal Weight { get; }

        public WeightReadEventArgs(decimal data)
        {
            Weight = data;
        }
    }
}