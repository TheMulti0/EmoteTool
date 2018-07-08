using System;
using System.Globalization;
using System.Windows.Data;

namespace EmoteTool.ViewModels
{
    internal class BoolToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var boolValue = (bool) value;
            double opacity = 0;

            if (boolValue)
            {
                opacity = 0.3;
            }
            return opacity;
        }

        public object ConvertBack(object value, Type targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}