namespace Solarponics.Models
{
    public class SensorModuleModbusTcp : SensorModule
    {
        public string IpAddress { get; set; }
        public ushort Port { get; set; }

        public override bool Equals(object obj) => this.EqualsModbus(obj as SensorModuleModbusTcp);
        private bool EqualsModbus(SensorModuleModbusTcp p)
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
            return (IpAddress == p.IpAddress) && (Port == p.Port) && base.EqualsSensorModule(p);
        }

        public override int GetHashCode() => (IpAddress, Port, base.GetHashCode()).GetHashCode();

        public static bool operator ==(SensorModuleModbusTcp obj1, SensorModuleModbusTcp obj2)
        {
            if (obj1 is null)
            {
                if (obj2 is null)
                {
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return obj1.Equals(obj2);
        }

        public static bool operator !=(SensorModuleModbusTcp obj1, SensorModuleModbusTcp obj2) => !(obj1 == obj2);
    }
}