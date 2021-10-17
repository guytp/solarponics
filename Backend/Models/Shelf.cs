namespace Solarponics.Models
{
    public class Shelf
    {
        public int Id { get; set; }
        public int RoomId { get; set;  }
        public string Name { get; set;  }
        public string RoomName { get; set; }
        public string LocationName { get; set; }
    }
}