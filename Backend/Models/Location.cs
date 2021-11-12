using System.ComponentModel;

namespace Solarponics.Models
{
    public class Location : INotifyPropertyChanged
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public Room[] Rooms { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}