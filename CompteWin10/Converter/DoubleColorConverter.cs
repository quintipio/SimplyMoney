using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace CompteWin10.Converter
{
    /// <summary>
    /// Converter pour mettre en rouge les champs double
    /// </summary>
    public class DoubleColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var chiffre = value as double?;

            if (chiffre < 0)
            {
                return new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
            }
            return new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}
