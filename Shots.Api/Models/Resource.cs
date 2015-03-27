using System;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Newtonsoft.Json;

namespace Shots.Api.Models
{
    public class Resource : NotifyPropertyChangedObject
    {
        private string _description;
        private bool _isLike;
        private int _ratioHeight;

        [JsonProperty("auto_ts")]
        public DateTime? AutoTs { get; set; }

        public string Caption { get; set; }
        // The explore section has shots that use different property names, like description and caption
        public string Description
        {
            get { return Caption ?? _description; }
            set { _description = value; }
        }

        public string Filter { get; set; }
        public string Front { get; set; }
        public FsVenueInfo FsVenueInfo { get; set; }
        public int Height { get; set; }
        public string Id { get; set; }

        public bool IsLike
        {
            get { return _isLike; }
            set
            {
                _isLike = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public string LikeParseText
        {
            get
            {
                // Using !@ it will not include @ on text, just link it.
                // !<<{0}>> is utilized for like page
                const string moreThanTwo = "!@{0}!@, !@{1} and {2} more";
                const string onlyTwo = "!@{0} and !@{1}";
                const string onlyOne = "!@{0}";
                string fmt;

                switch (LikeCount)
                {
                    case 0:
                        return null;
                    case 1:
                        fmt = string.Format(onlyOne, Likes[0].Username);
                        break;
                    case 2:
                        fmt = string.Format(onlyTwo, Likes[0].Username, Likes[1].Username);
                        break;
                    default:
                        fmt = string.Format(moreThanTwo, Likes[0].Username, Likes[1].Username, LikeCount);
                        break;
                }

                return fmt;
            }
        }

        public int Views { get; set; }
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

        public int RatioHeight
        {
            get
            {
                if (_ratioHeight > 0) return _ratioHeight;
                if (Window.Current == null || Window.Current.CoreWindow == null) return 533;

                var ratio = (double) Width/Height;
                _ratioHeight = (int) (Window.Current.CoreWindow.Bounds.Width/ratio);
                return _ratioHeight;
            }
        }
    }
}