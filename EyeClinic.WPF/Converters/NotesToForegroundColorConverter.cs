using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using static System.Windows.Media.ColorConverter;

namespace EyeClinic.WPF.Converters
{
    public class NotesToForegroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var blackHex = ConvertFromString("#000000");
            var redHex = ConvertFromString("#F20000");

            if (blackHex == null || redHex == null)
                return null;

            if (value is not string result)
                return new SolidColorBrush(
                    (Color)blackHex);

            if (string.IsNullOrWhiteSpace(result))
                return new SolidColorBrush(
                    (Color)blackHex);


            return new SolidColorBrush(
                    (Color)redHex);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return null;
        }

       
    }
}
