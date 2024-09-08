using System;
using System.Globalization;
using static System.Windows.Media.ColorConverter;
using System.Windows.Data;
using System.Windows.Media;

namespace EyeClinic.WPF.Converters
{
    public class MedicineToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var blueHex = ConvertFromString("#110dff");
            var blackHex = ConvertFromString("#000000");

            if (value is string result) {
                return string.IsNullOrWhiteSpace(result) ?
                    new SolidColorBrush((Color)blackHex) :
                    new SolidColorBrush((Color)blueHex);
            }

            return new SolidColorBrush(
                (Color)blackHex);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return value;
        }
    }
}
