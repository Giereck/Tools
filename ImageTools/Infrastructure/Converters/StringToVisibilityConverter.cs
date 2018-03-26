using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ImageTools.Infrastructure.Converters
{
    public class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var result = Visibility.Collapsed;

            if(value is string s)
            {
                result = string.IsNullOrWhiteSpace(s) ? Visibility.Collapsed : Visibility.Visible;
            }

            if (parameter is bool invert && invert)
            {
                result = result == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
