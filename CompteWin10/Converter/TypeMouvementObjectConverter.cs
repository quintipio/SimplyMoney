using System;
using Windows.UI.Xaml.Data;
using CompteWin10.Model;

namespace CompteWin10.Converter
{
    /// <summary>
    /// Converter des catégories
    /// </summary>
    public class TypeMouvementObjectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (TypeMouvement) value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
           return value;
        }
    }
}
