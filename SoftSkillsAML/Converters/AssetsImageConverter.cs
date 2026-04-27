using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace SoftSkillsAML.Converters
{
    internal class AssetsImageConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return new Bitmap(AssetLoader.Open(new Uri($"avares://SoftSkillsAML/Assets/{value.ToString()}")));
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return new Avalonia.Data.BindingNotification(value);
        }
    }
}
