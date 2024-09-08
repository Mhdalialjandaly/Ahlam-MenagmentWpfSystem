using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using static System.Windows.Media.ColorConverter;

namespace EyeClinic.WPF.Converters
{
    internal class BackGroundRemaining : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var redHex = ConvertFromString("Green");
            var blackHex = ConvertFromString("AliceBlue");

            if (blackHex == null || redHex == null)
                return null;

            if (value is not true)
                return new SolidColorBrush(
                    (Color)blackHex);

            if (value is not false)
                return new SolidColorBrush(
                    (Color)redHex);


            return new SolidColorBrush(
                    (Color)redHex);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }


    }
}
