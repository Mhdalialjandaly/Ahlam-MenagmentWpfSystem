using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using EyeClinic.Model;

namespace EyeClinic.WPF.Converters
{
    public class ReservedColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            var blackHex = ColorConverter.ConvertFromString("#000000");
            var greenHex = ColorConverter.ConvertFromString("#0DB00A");
            var blueHex = ColorConverter.ConvertFromString("#110dff");

            if (blackHex == null || greenHex == null || blueHex == null)
                return null;

            if (value is PatientOperationDto patientOperation) {
                if (patientOperation.IsFinish)
                    return new SolidColorBrush((Color)blueHex);

                return patientOperation.MedicalCenterReserved ? new SolidColorBrush(
                    (Color)greenHex) : new SolidColorBrush(
                    (Color)blackHex);
            }


            return new SolidColorBrush(
                (Color)blackHex);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            return value;
        }
    }
}
