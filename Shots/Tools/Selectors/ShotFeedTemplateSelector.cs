using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Shots.Web.Models;

namespace Shots.Tools.Selectors
{
    internal class ShotFeedTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ShotTemplate { get; set; }
        public DataTemplate AdTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item)
        {
            var shot = (ShotItem) item;
            return shot.Ad == null ? ShotTemplate : AdTemplate;
        }
    }
}