﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace EmoteTool.ViewModels
{
    internal class StringToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) 
            => string.IsNullOrWhiteSpace(value as string) 
                ? Visibility.Collapsed 
                : Visibility.Visible;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}