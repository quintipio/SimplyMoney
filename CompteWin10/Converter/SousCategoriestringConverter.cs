using System;
using Windows.UI.Xaml.Data;
using CompteWin10.Model;

namespace CompteWin10.Converter
{
    public class SousCategorieStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var val = value as SousCategorie;
            return (val == null)?"":val.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
