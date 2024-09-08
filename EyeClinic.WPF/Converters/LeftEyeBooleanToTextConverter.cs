using System;
using System.Globalization;
using System.Windows.Data;

namespace EyeClinic.WPF.Converters
{
    public class LeftEyeBooleanToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is bool leftEye) {
                return leftEye ? "Left Eye" : "Right Eye";
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return value;
        }
    }
}
