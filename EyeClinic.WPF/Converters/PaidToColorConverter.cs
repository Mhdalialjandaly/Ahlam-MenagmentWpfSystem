using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace EyeClinic.WPF.Converters
{
    public class PaidToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var blackHex = ColorConverter.ConvertFromString("#000000");
            var blueHex = ColorConverter.ConvertFromString("#110dff");

            if (blackHex == null || blueHex == null)
                return null;

            if(value is bool paid)
                return new SolidColorBrush(paid ? (Color)blackHex : (Color)blueHex);

            return new SolidColorBrush((Color)blackHex);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return null;
        }
    }
}