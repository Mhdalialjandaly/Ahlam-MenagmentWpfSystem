using System;
using System.Globalization;
using static System.Windows.Media.ColorConverter;
using System.Windows.Data;
using System.Windows.Media;

namespace EyeClinic.WPF.Converters
{
    public class ReviewTypeColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var blueHex = ConvertFromString("#110dff");
            var redHex = ConvertFromString("#F20000");
            var blackHex = ConvertFromString("#000000");

            if (value is string result) {
                if (result == "Review")
                    return new SolidColorBrush(
                        (Color)blueHex);
                else
                    return new SolidColorBrush(
                        (Color)redHex);
            }

            return new SolidColorBrush(
                (Color)blackHex);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return value;
        }
    }
}
