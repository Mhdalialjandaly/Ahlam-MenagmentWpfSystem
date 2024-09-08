using System;
using System.Globalization;
using System.Windows.Data;
using EyeClinic.WPF.AppServices.Localization;

namespace EyeClinic.WPF.Converters
{
    public class PatientVisitTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            if (value is bool explicitCost) {
                return explicitCost ? "NewVisit" : "Review";
            }

            return "Review";

            //if (value is bool explicitCost) {
            //    return explicitCost ? _localizationService.Localize("NewVisit") :
            //        _localizationService.Localize("Review");
            //}

            //return _localizationService.Localize("Review");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            throw new NotImplementedException();
        }
    }
}
