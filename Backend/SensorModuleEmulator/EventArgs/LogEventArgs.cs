namespace Solarponics.SensorModuleEmulator.EventArgs
{
    public class LogEventArgs : System.EventArgs
    {
        public LogEventArgs(string log)
        {
            Log = log;
        }

        public string Log { get; }
    }
}