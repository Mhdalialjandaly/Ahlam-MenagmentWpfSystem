using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using EyeClinic.Core.Enums;

namespace EyeClinic.WPF.Converters
{
    public class OperationToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is Operation operation) {
                return operation != Operation.View ? Visibility.Collapsed : Visibility.Visible;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return null;
        }
    }
}