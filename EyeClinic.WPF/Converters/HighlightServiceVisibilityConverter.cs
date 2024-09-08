using EyeClinic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace EyeClinic.WPF.Converters
{
    internal class HighlightServiceVisibilityConverter :IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var grayHex = ColorConverter.ConvertFromString("white");
            var greenHex = ColorConverter.ConvertFromString("#23D4F9");

            if (grayHex == null || greenHex == null)
                return null;

            if (value is List<PatientService> services && parameter is string currentService)
            {
                if (services.Any(e => e.ToString() == currentService))
                    return Visibility.Visible;

                return Visibility.Hidden;
            }

            return Visibility.Hidden; 
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
