using EyeClinic.Core.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace EyeClinic.WPF.Converters
{
    internal class RemainingToBackGroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var grayHex = ColorConverter.ConvertFromString("white");
            var redHex = ColorConverter.ConvertFromString("#23D4F9");

            if (grayHex == null || redHex == null)
                return null;

            if (value is int remaining)
            {
                if (remaining>0)
                    return new SolidColorBrush((Color)redHex);

                return new SolidColorBrush((Color)grayHex);
            }

            return new SolidColorBrush((Color)grayHex);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
