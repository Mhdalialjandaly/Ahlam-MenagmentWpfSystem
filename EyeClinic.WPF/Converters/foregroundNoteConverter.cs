using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace EyeClinic.WPF.Converters
{
    internal class foregroundNoteConverter:IValueConverter
    {
        public object Convert(object value, Type typeTarget, object param, CultureInfo culture)
        {
            if ((string)value == "جيد")
            {

                return "Green";
                //return Colors.Black;
            }
            else if ((string)value is "بحاجةطلب")
            {
                return "Red";
                // return Colors.Red;
            }
            return "black";
        }
        public object ConvertBack(object value, Type typeTarget, object param, CultureInfo culture)
        {
            throw new NotImplementedException();

        }
    }
}
