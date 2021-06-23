using System;
using System.Globalization;
using System.Windows.Data;

namespace Solarponics.ProductionManager.Converters
{
    public class TextToMaskedPasswordConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var original = (string) value;
            return original == null ? null : new string('⬤', original.Length);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}