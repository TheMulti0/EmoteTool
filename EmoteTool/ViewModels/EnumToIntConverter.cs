using System;
using System.Globalization;
using System.Windows.Data;

namespace EmoteTool.ViewModels
{
    internal class EnumToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var @enum = value as Enum;
            int enumValue = System.Convert.ToInt32(@enum);
            return enumValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}