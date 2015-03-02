using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Newtonsoft.Json;

namespace Shots.Api.Models
{
    public class Resource
    {
        private string _description;

        [JsonProperty("auto_ts")]
        public DateTime? AutoTs { get; set; }

        public string Caption { get; set; }
        // The explore section has shots that use different property names, like description and caption
        public string Description { get { return Caption ?? _description; } set { _description = value; } }
        public string Filter { get; set; }
        public string Front { get; set; }
        public FsVenueInfo FsVenueInfo { get; set; }
        public int Height { get; set; }
        public string Id { get; set; }
        public bool IsLike { get; set; }
        public int LikeCount { get; set; }
        public List<Like> Likes { get; set; }

        [JsonProperty("1080")]
        public string Pic1080 { get; set; }

        [JsonProperty("188")]
        public string Pic188 { get; set; }

        [JsonProperty("279")]
        public string Pic279 { get; set; }

        [JsonProperty("320")]
        public string Pic320 { get; set; }

        [JsonProperty("640")]
        public string Pic640 { get; set; }

        [JsonProperty("750")]
        public string Pic750 { get; set; }

        [JsonProperty("94")]
        public string Pic94 { get; set; }

        [JsonProperty("reactto_id")]
        public string ReactToId { get; set; }

        public string ThumbUrl { get; set; }
        public DateTime Time { get; set; }
        public string Type { get; set; }
        public string Url { get; set; }

        [JsonProperty("user_id")]
        public long UserId { get; set; }

        public string WebIdent { get; set; }
        public int Width { get; set; }

        private int _ratioHeight;
        public int RatioHeight
        {
            get
            {
                if (_ratioHeight > 0) return _ratioHeight;
                if (Window.Current == null || Window.Current.CoreWindow == null) return 533;

                var ratio = (double)Width / Height;
                _ratioHeight = (int)(Window.Current.CoreWindow.Bounds.Width/ratio);
                return _ratioHeight;
            }
        }
    }
}