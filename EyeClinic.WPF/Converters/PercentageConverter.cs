using System;
using System.Globalization;
using System.Windows.Data;

namespace EyeClinic.WPF.Converters
{
    public class PercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            double.TryParse(parameter.ToString(), out double result);

            return System.Convert.ToDouble(value) * System.Convert.ToDouble(result);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return value;
        }
    }
}
