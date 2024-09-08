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
    public class NullOrEmptyToVisibilityBackConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value == null)
                return Visibility.Visible;

            if (value is ICollection collection)
                if (collection.IsNullOrEmpty())
                    return Visibility.Visible;
                else
                    return Visibility.Collapsed;

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
