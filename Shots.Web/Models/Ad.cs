using Newtonsoft.Json;

namespace Shots.Web.Models
{
    public class Ad
    {
        public string Id { get; set; }
        public string Template { get; set; }
        public Panel Panel { get; set; }
        public string Name { get; set; }
    }

    public class Button
    {
        public string Caption { get; set; }

        [JsonProperty("action_type")]
        public string ActionType { get; set; }

        public string Action { get; set; }
    }

    public class BackgroundIcon
    {
        [JsonProperty("80")]
        public string Pic80 { get; set; }

        [JsonProperty("160")]
        public string Pic160 { get; set; }

        [JsonProperty("320")]
        public string Pic320 { get; set; }
    }

    public class Panel
    {
        [JsonProperty("background_color")]
        public string BackgroundColor { get; set; }

        [JsonProperty("foreground_color")]
        public string ForegroundColor { get; set; }

        public string Title { get; set; }
        public string Heading { get; set; }
        public Button Button { get; set; }

        [JsonProperty("background_icon")]
        public BackgroundIcon BackgroundIcon { get; set; }
    }
}