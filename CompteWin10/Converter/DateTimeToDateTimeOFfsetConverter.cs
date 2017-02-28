using System;
using Windows.UI.Xaml.Data;

namespace CompteWin10.Converter
{
    public class DateTimeToDateTimeOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return new DateTimeOffset((DateTime)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value != null)
            {
                return ((DateTimeOffset) value).DateTime;
            }
            else
            {
                return DateTime.Today;
            }
        }
    }

}
