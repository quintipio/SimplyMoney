using System;
using Windows.UI.Xaml.Data;

namespace CompteWin10.Converter
{
    /// <summary>
    /// Converter pour afficher une date dans un string
    /// </summary>
    public class DateStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var date = (DateTime)value;

            return date.Day + "/" + date.Month + "/" + date.Year;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
