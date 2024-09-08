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
    public class ZeroToVisibilityConverter : IValueConverter
    {
      public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is double doubleResult)
                return doubleResult == 0 ? Visibility.Collapsed : Visibility.Visible;

            if (value is int intResult)
                return intResult == 0 ? Visibility.Collapsed : Visibility.Visible;

            if (value is byte byteResult)
                return byteResult == 0 ? Visibility.Collapsed : Visibility.Visible;

            return Visibility.Collapsed;
        }  

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return value;
        }
    }
}
