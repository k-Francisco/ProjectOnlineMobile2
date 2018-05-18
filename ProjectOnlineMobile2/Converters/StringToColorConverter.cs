using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace ProjectOnlineMobile2.Converters
{
    public class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string valueAsString = value.ToString();

            if (string.IsNullOrWhiteSpace(valueAsString))
            {
                return Color.Black;
            }
            else
            {
                return Color.FromHex(valueAsString);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
