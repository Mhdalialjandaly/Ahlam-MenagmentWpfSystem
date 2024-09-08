using System;
using System.Globalization;
using System.Windows.Data;

namespace EyeClinic.WPF.Converters
{
    public class PayoutToTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is bool payoutType) {
                return payoutType ? "Pay" : "Cash";
            }

            return "Review";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}