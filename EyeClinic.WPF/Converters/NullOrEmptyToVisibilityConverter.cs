using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using EyeClinic.Core;

namespace EyeClinic.WPF.Converters
{
    public class NullOrEmptyToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null)
                return Visibility.Collapsed;

            if (value is ICollection collection)
                if (collection.IsNullOrEmpty())
                    return Visibility.Collapsed;
                else
                    return Visibility.Visible;

            if (value is string text)
                return string.IsNullOrWhiteSpace(text) ? Visibility.Collapsed : Visibility.Visible;

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
