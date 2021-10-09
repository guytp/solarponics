namespace Solarponics.ProductionManager.Data
{
    public class SensorRoomGroupAggregate
    {
        public SensorRoomGroupAggregate(string roomName, SensorReadingAggregate[] sensors)
        {
            RoomName = roomName;
            Sensors = sensors;
        }

        public string RoomName { get; }

        public SensorReadingAggregate[] Sensors { get; }
    }
}