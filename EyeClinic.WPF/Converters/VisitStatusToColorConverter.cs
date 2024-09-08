using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using EyeClinic.Core.Enums;

namespace EyeClinic.WPF.Converters
{
    public class VisitStatusToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is not byte visitStatus)
                return new SolidColorBrush(Colors.White);

            return visitStatus switch {
                (int)PatientVisitStatus.Created => new SolidColorBrush(Colors.White),
                (int)PatientVisitStatus.Started => new SolidColorBrush(Colors.LightYellow),
                (int)PatientVisitStatus.Finished => new SolidColorBrush(Colors.LightPink),
                _ => new SolidColorBrush(Colors.White)
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return new SolidColorBrush(Colors.White);
        }
    }
}
