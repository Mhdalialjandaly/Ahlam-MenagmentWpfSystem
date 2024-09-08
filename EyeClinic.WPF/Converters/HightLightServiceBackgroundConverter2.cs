using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using EyeClinic.Core.Enums;

namespace EyeClinic.WPF.Converters
{
    public class HighlightServiceBackgroundConverter3 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var grayHex = ColorConverter.ConvertFromString("White");
            var redHex = ColorConverter.ConvertFromString("#FF0000");

            if (grayHex == null || redHex == null)
                return null;

            if (value is List<PatientService> services && parameter is string currentService)
            {
                if (services.Any(e => e.ToString() == currentService))
                    return new SolidColorBrush((Color)redHex);

                return new SolidColorBrush((Color)grayHex);
            }

            return new SolidColorBrush((Color)grayHex);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class HighlightServiceBackgroundConverter4 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var grayHex = ColorConverter.ConvertFromString("#a5f7aa");
            var purpleHex = ColorConverter.ConvertFromString("#0DB00A");

            if (grayHex == null || purpleHex == null)
                return null;

            if (value is List<PatientService> services && parameter is string currentService)
            {
                if (services.Any(e => e.ToString() == currentService))
                    return new SolidColorBrush((Color)purpleHex);

                return new SolidColorBrush((Color)grayHex);
            }

            return new SolidColorBrush((Color)grayHex);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}