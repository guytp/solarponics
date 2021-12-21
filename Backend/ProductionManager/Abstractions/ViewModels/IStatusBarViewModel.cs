namespace Solarponics.ProductionManager.Abstractions.ViewModels
{
    public interface IStatusBarViewModel
    {
        public string UserName { get; }
        public string Time { get; }
        public string Date { get; }
        public int DayOfYear { get; }
    }
}