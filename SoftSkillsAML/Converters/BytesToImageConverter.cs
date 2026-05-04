using System;
using System.Globalization;
using System.IO;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace SoftSkillsAML.Converters
{
    internal class BytesToImageConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is byte[] bytes && bytes.Length > 0)
            {
                try
                {
                    return new Bitmap(new MemoryStream(bytes));
                }
                catch
                {
                }
            }

            return new Bitmap(AssetLoader.Open(new Uri("avares://SoftSkillsAML/Assets/placeholder.jpg")));
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            return new Avalonia.Data.BindingNotification(value);
        }
    }
}
