using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace EyeClinic.WPF.Converters
{
    public class NullToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var defaultColor = ColorConverter.ConvertFromString("#FF00355D");
            var greenHex = ColorConverter.ConvertFromString("#0DB00A");

            if (defaultColor == null || greenHex == null)
                return null;

            if (value == null)
                return new SolidColorBrush((Color)defaultColor);

            return new SolidColorBrush((Color)greenHex);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return null;
        }
    }
}