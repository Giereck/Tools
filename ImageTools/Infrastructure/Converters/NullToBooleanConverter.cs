using System;
using System.Globalization;
using System.Windows.Data;

namespace ImageTools.Infrastructure.Converters
{
    public class NullToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = value == null;

            if (parameter is bool b)
            {
                result = b ? !result : result;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}