namespace Solarponics.ProductionManager.Data
{
    public class SensorRoomGroupCurrent
    {
        public SensorRoomGroupCurrent(string roomName, SensorReadingCurrent[] sensors)
        {
            RoomName = roomName;
            Sensors = sensors;
        }

        public string RoomName { get; }

        public SensorReadingCurrent[] Sensors { get; }
    }
}