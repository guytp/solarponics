namespace Solarponics.Models
{
    public class SensorModule
    {
        public string Room { get; set; }
        public string Location { get; set; }
        public int Id { get; set; }
        public Sensor[] Sensors { get; set; }
        public string SerialNumber { get; set; }
        public string Name { get; set; }

        protected bool EqualsSensorModule(SensorModule p)
        {
            if (p is null)
            {
                return false;
            }

            // Optimization for a common success case.
            if (ReferenceEquals(this, p))
            {
                return true;
            }

            // If run-time types are not exactly the same, return false.
            if (this.GetType() != p.GetType())
            {
                return false;
            }

            // Return true if the fields match.
            // Note that the base class is not invoked because it is
            // System.Object, which defines Equals as reference equality.
            var sensorsMatch = false;
            if (Sensors == null && p.Sensors == null)
                sensorsMatch = true;
            else if ((Sensors != null && p.Sensors == null) || (Sensors == null && p.Sensors != null))
                sensorsMatch = false;
            else if (Sensors.Length != p.Sensors.Length)
                sensorsMatch = false;
            else
            {
                var allEquals = true;
                for (var i = 0; i < Sensors.Length; i++)
                {
                    if (!Sensors[i].Equals(p.Sensors[i]))
                    {
                        allEquals = false;
                        break;
                    }
                }
                sensorsMatch = allEquals;
            }
            return (Room == p.Room) && (Location == p.Location) && (Id == p.Id) && (p.SerialNumber == SerialNumber) && (Name == p.Name) && (sensorsMatch);
        }

        public override int GetHashCode() => (Room, Location, Id, SerialNumber, Name, (Sensors?.GetHashCode()) ?? 0).GetHashCode();

    }
}