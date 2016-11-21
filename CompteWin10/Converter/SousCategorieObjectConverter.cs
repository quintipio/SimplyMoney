﻿using System;
using Windows.UI.Xaml.Data;
using CompteWin10.Model;

namespace CompteWin10.Converter
{
    public class SousCategorieObjectConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (SousCategorie) value;
        }
    }
}
