using System;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Shots.Core.Extensions
{
    /// <summary>
    ///     Extension and helper methods for converting color values
    ///     between different RGB data types and different color spaces.
    /// </summary>
    public static class ColorExtensions
    {
        public static SolidColorBrush ToColorBrush(this string hexaColor)
        {
            return new SolidColorBrush(
                Color.FromArgb(
                    255,
                    Convert.ToByte(hexaColor.Substring(1, 2), 16),
                    Convert.ToByte(hexaColor.Substring(3, 2), 16),
                    Convert.ToByte(hexaColor.Substring(5, 2), 16)
                    )
                );
        }
    }
}