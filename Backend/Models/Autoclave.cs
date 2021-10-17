namespace Solarponics.Models
{
    public class Autoclave
    {
        public int Id { get; set; }
        public int RoomId { get; set;  }
        public string Name { get; set;  }
        public string Details { get; set; }
        public string RoomName { get; set; }
        public string LocationName { get; set; }
    }
}