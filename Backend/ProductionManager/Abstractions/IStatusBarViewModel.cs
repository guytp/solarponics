namespace Solarponics.ProductionManager.Abstractions
{
    public interface IStatusBarViewModel
    {
        public string UserName { get; }
        public string Time { get; }
        public string Date { get; }
    }
}