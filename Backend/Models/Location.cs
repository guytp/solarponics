namespace Solarponics.Models
{
    public class Location
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Room[] Rooms { get; set; }
    }
}