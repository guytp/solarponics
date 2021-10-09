using Solarponics.Models;
using System;

namespace Solarponics.ProductionManager.Data
{
    public class SensorReadingCurrent
    {
        private const int DesiredLabels = 15;

        public SensorReadingCurrent(string name, SensorType type, decimal reading, DateTime lastUpdated, decimal criticalLowBelow, decimal warningLowBelow, decimal warningHighAbove, decimal criticalHighAbove)
        {
            Name = name;
            Type = type;
            Reading = reading;
            LastUpdated = lastUpdated;
            CriticalLowBelow = criticalLowBelow;
            CriticalHighAbove = criticalHighAbove;
            WarningHighAbove = warningHighAbove;
            WarningLowBelow = warningLowBelow;
        }

        public string Name { get; }

        public SensorType Type { get; }

        public decimal Reading { get; }

        public DateTime LastUpdated { get; }

        public decimal CriticalLowBelow { get; }

        public decimal WarningLowBelow { get; }

        public decimal WarningHighAbove { get; }

        public decimal CriticalHighAbove { get; }

        public decimal MinValue
        {
            get
            {
                var r = Reading < CriticalLowBelow ? Reading * 0.9m : CriticalLowBelow * 0.9m;
                if (r % 2 != 0)
                    r--;
                return r;
            }
        }

        public decimal MaxValue
        {
            get
            {
                var r = Reading > CriticalHighAbove ? Reading * 1.1m : CriticalHighAbove * 1.1m;
                if (r % 2 != 0)
                    r++;
                return r;
            }
        }

        public int LabelsStep
        {
            get
            {
                var labels = (MaxValue - MinValue) / DesiredLabels;
                if (labels % 2 != 0)
                    labels++;
                return (int)labels;
            }
        }

        public int TicksStep => LabelsStep / 2;

        public string LastUpdatedAgo
        {
            get
            {

                var interval = DateTime.UtcNow.Subtract(LastUpdated);
                int unit;
                string unitName;
                if (interval.Minutes < 1)
                {
                    unit = interval.Seconds;
                    if (unit == 0)
                    {
                        return "Just updated";
                    }
                    unitName = "second";
                }
                else if (interval.Hours < 1)
                {
                    unit = interval.Minutes;
                    unitName = "minute";
                }
                else if (interval.Days < 1)
                {
                    unit = interval.Hours;
                    unitName = "hour";
                }
                else
                {
                    unit = interval.Days;
                    unitName = "day";
                }

                return $"{unit} {unitName}" + (unit > 1 ? "s" : string.Empty) + " ago";
            }
        }

        public string LastUpdatedColour
        {
            get
            {
                var interval = DateTime.UtcNow.Subtract(LastUpdated);
                return interval.Minutes > 5 ? "#FF3939" : "White";
            }
        }
    }
}