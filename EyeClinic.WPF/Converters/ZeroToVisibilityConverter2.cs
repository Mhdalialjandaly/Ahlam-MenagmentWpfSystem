using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace EyeClinic.WPF.Converters
{
    internal class ZeroToVisibilityConverter2 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double doubleResult)
                return doubleResult == 0 ? Visibility.Visible : Visibility.Collapsed;

            if (value is int intResult)
                return intResult == 0 ? Visibility.Visible : Visibility.Collapsed;

            if (value is byte byteResult)
                return byteResult == 0 ? Visibility.Visible : Visibility.Collapsed;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
