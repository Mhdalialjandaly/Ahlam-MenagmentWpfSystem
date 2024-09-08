using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Media;
using EyeClinic.Core.Enums;

namespace EyeClinic.WPF.Converters
{
    public class HighlightServiceBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var grayHex = ColorConverter.ConvertFromString("white");
            var greenHex = ColorConverter.ConvertFromString("#23D4F9");

            if (grayHex == null || greenHex == null)
                return null;

            if (value is List<PatientService> services && parameter is string currentService) {
                if (services.Any(e => e.ToString() == currentService))
                    return new SolidColorBrush((Color)greenHex);

                return new SolidColorBrush((Color)grayHex);
            }

            return new SolidColorBrush((Color)grayHex);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return null;
        }
    }
    public class HighlightServiceBackgroundConverter2 : IValueConverter 
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object Convert2(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var grayHex = ColorConverter.ConvertFromString("white");
            var RedHex = ColorConverter.ConvertFromString("#23D4F9");

            if (grayHex == null || RedHex == null)
                return null;

            if (value is List<PatientService> services && parameter is string currentService)
            {
                if (services.Any(e => e.ToString() == currentService))
                    return new SolidColorBrush((Color)RedHex);

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