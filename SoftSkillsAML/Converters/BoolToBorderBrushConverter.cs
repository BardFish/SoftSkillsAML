using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace SoftSkillsAML.Converters
{
    internal class BoolToBorderBrushConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return value is bool b && b ? Brushes.Green : Brushes.Red;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return new Avalonia.Data.BindingNotification(value);
        }
    }
}
