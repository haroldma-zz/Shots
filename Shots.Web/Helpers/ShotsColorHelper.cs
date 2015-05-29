using System.Collections.Generic;
using Windows.UI.Xaml.Media;
using Shots.Core.Extensions;

namespace Shots.Web.Helpers
{
    /// <summary>
    ///     Represent a color utilized for profiles.
    /// </summary>
    public class ShotsColorHelper
    {
        /// <summary>
        ///     All colors available for profiles.
        /// </summary>
        public static List<ShotsColorHelper> Colors = new List<ShotsColorHelper>
        {
            new ShotsColorHelper("Ruby", "#F4274E"),
            new ShotsColorHelper("Tangerine", "#FF584B"),
            new ShotsColorHelper("Amber", "#FF9935"),
            new ShotsColorHelper("Seafoam", "#30BF91"),
            new ShotsColorHelper("Lime", "#63E252"),
            new ShotsColorHelper("Cerulean", "#39C4E6"),
            new ShotsColorHelper("Sapphire", "#4070EB"),
            new ShotsColorHelper("Violet", "#843CC8"),
            new ShotsColorHelper("Heliotrope", "#CC45CE"),
            new ShotsColorHelper("Candy", "#FF6EC0")
        };

        public ShotsColorHelper(string name, string hex)
        {
            Name = name;
            Brush = hex.ToColorBrush();
        }

        public string Name { get; set; }
        public SolidColorBrush Brush { get; set; }

        public static ShotsColorHelper GetColorById(int id)
        {
            return id <= Colors.Count ? Colors[id - 1] : Colors[0];
        }
    }
}