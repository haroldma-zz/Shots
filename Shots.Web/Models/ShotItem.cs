using System;
using Newtonsoft.Json;
using Shots.Web.Converters;

namespace Shots.Web.Models
{
    public class ShotItem
    {
        private DateTime _time;

        [JsonProperty("react_to")]
        [JsonConverter(typeof (ArrayForNullConverter<ShotItem>))]
        public ShotItem ReactTo { get; set; }

        public Resource Resource { get; set; }

        public string DescriptionFormatted
            =>
                (ReactTo != null ? "@<b>" + ReactTo.User.Username + " — " : "") +
                Resource?.Description;

        public DateTime Time
        {
            // Some shots only have the time in the resource
            get { return _time == DateTime.MinValue && Resource != null ? Resource.Time : _time; }
            set { _time = value; }
        }

        public UserInfo User { get; set; }
        public bool Seen { get; set; }
        public Ad Ad { get; set; }
    }
}