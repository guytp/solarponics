using LiveCharts;
using LiveCharts.Wpf;
using Solarponics.Models;
using System;
using System.Linq;
using System.Windows.Media;

namespace Solarponics.ProductionManager.Data
{
    public class SensorReadingAggregate
    {
        private const int DesiredLabels = 15;

        public SensorReadingAggregate(string name, SensorType type, AggregateSensorReading[] readings, decimal criticalLowBelow, decimal warningLowBelow, decimal warningHighAbove, decimal criticalHighAbove)
        {
            Name = name;
            Type = type;
            CriticalLowBelow = criticalLowBelow;
            CriticalHighAbove = criticalHighAbove;
            WarningHighAbove = warningHighAbove;
            WarningLowBelow = warningLowBelow;
            var redBrush = new SolidColorBrush(Color.FromRgb(0xFF, 0x39, 0x39));
            var yellowBrush = new SolidColorBrush(Color.FromRgb(0xF8, 0xA7, 0x25));
            var greenBrush = new SolidColorBrush(Color.FromRgb(0x35, 0xE5, 0x7E));

            var includeCriticalHigh = (readings.Any(r => r.Maximum > criticalHighAbove || r.Minimum > criticalHighAbove || r.Average > criticalHighAbove));
            var includeCriticalLow = (readings.Any(r => r.Maximum < criticalLowBelow || r.Minimum < criticalLowBelow || r.Average < criticalLowBelow));
            var includeWarningLow = (readings.Any(r => r.Maximum < warningLowBelow || r.Minimum < warningLowBelow || r.Average < warningLowBelow));
            var includeWarningHigh = (readings.Any(r => r.Maximum > warningHighAbove || r.Minimum > warningHighAbove || r.Average > warningHighAbove));

            Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Min",
                    Values = new ChartValues<decimal>(readings.Select(r => r.Minimum)),
                    Fill = Brushes.Transparent,
                },
                new LineSeries
                {
                    Title = "Avg",
                    Values = new ChartValues<decimal>(readings.Select(r => r.Average)),
                    Stroke = greenBrush,
                    Fill = Brushes.Transparent,
                },
                new LineSeries
                {
                    Title = "Max",
                    Values = new ChartValues<decimal>(readings.Select(r => r.Maximum)),
                    Fill = Brushes.Transparent,
                }
            };

            var highInsert = 0;
            if (includeCriticalHigh)
            {
                Series.Insert(highInsert,
                    new LineSeries
                    {
                        Title = "Critical (High)",
                        Values = new ChartValues<decimal>(Enumerable.Repeat(criticalHighAbove, readings.Length)),
                        PointGeometry = null,
                        DataLabels = false,
                        Fill = Brushes.Transparent,
                        Stroke = redBrush
                    });
                highInsert++;
            }
            if (includeWarningHigh)
                Series.Insert(highInsert,
                    new LineSeries
                    {
                        Title = "Warning (High)",
                        Values = new ChartValues<decimal>(Enumerable.Repeat(warningHighAbove, readings.Length)),
                        PointGeometry = null,
                        DataLabels = false,
                        Fill = Brushes.Transparent,
                        Stroke = yellowBrush
                    });

            if (includeWarningLow)
                Series.Add(new LineSeries
                {
                    Title = "Warning (Low)",
                    Values = new ChartValues<decimal>(Enumerable.Repeat(warningLowBelow, readings.Length)),
                    PointGeometry = null,
                    DataLabels = false,
                    Fill = Brushes.Transparent,
                    Stroke = yellowBrush
                });

            if (includeCriticalLow)
                Series.Add(new LineSeries
                {
                    Title = "Critical (Low)",
                    Values = new ChartValues<decimal>(Enumerable.Repeat(criticalLowBelow, readings.Length)),
                    PointGeometry = null,
                    DataLabels = false,
                    Fill = Brushes.Transparent,
                    Stroke = redBrush
                });

            Labels = readings.Select(r => r.Time.ToString()).ToArray();
        }

        public string Name { get; }

        public string[] Labels { get; }

        public SensorType Type { get; }

        public decimal CriticalLowBelow { get; }

        public decimal WarningLowBelow { get; }

        public decimal WarningHighAbove { get; }

        public decimal CriticalHighAbove { get; }

        public SeriesCollection Series { get; }
    }
}