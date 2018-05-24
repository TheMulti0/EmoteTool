using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows.Data;

namespace EmoteTool.ViewModels
{
    internal class AddErrorToDescription : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            FieldInfo fi = value?.GetType()
                                .GetField(value.ToString());
            DescriptionAttribute[] attributes = (DescriptionAttribute[]) fi?.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
            {
                return attributes[0].Description;
            }
            return "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}