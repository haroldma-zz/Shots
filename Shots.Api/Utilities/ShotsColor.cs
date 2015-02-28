using System.Collections.Generic;
using Windows.UI.Xaml.Media;

namespace Shots.Api.Utilities
{
    /// <summary>
    ///     Represent a color utilized for profiles.
    /// </summary>
    public class ShotsColor
    {
        /// <summary>
        ///     All colors available for profiles.
        /// </summary>
        public static List<ShotsColor> Colors = new List<ShotsColor>
        {
            new ShotsColor("Ruby", "#F4274E"),
            new ShotsColor("Tangerine", "#FF584B"),
            new ShotsColor("Amber", "#FF9935"),
            new ShotsColor("Seafoam", "#30BF91"),
            new ShotsColor("Lime", "#63E252"),
            new ShotsColor("Cerulean", "#39C4E6"),
            new ShotsColor("Sapphire", "#4070EB"),
            new ShotsColor("Violet", "#843CC8"),
            new ShotsColor("Heliotrope", "#CC45CE"),
            new ShotsColor("Candy", "#FF6EC0")
        };

        public ShotsColor(string name, string hex)
        {
            Name = name;
            Brush = hex.ToColorBrush();
        }

        public string Name { get; set; }
        public SolidColorBrush Brush { get; set; }

        public static ShotsColor GetColorById(int id)
        {
            return id <= Colors.Count ? Colors[id - 1] : Colors[0];
        }
    }
}