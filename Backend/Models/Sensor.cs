namespace Solarponics.Models
{
    public class Sensor
    {
        public int Id { get; set; }
        public int SensorModuleId { get; set; }
        public SensorType Type { get; set; }
        public byte Number { get; set; }
        public decimal CriticalLowBelow { get; set; }
        public decimal WarningLowBelow { get; set; }
        public decimal WarningHighAbove { get; set; }
        public decimal CriticalHighAbove { get; set; }

        public override bool Equals(object obj) => this.EqualsSensor(obj as Sensor);
        private bool EqualsSensor(Sensor p)
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
            return (Id == p.Id) && (SensorModuleId == p.SensorModuleId) && (Type == p.Type) && (p.Number == Number) && (CriticalLowBelow == p.CriticalLowBelow) && (WarningLowBelow == p.WarningLowBelow) && (WarningHighAbove == p.WarningHighAbove) && (CriticalHighAbove == p.CriticalHighAbove);
        }

        public override int GetHashCode() => (Type, SensorModuleId, Number, CriticalLowBelow, WarningLowBelow, WarningHighAbove, CriticalHighAbove).GetHashCode();

    }
}