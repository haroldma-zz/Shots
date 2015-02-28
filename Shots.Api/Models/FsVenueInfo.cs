using Newtonsoft.Json;

namespace Shots.Api.Models
{
    public class FsVenueInfo
    {
        [JsonProperty("fsVenue_name")]
        public string FsVenueName { get; set; }

        public int Id { get; set; }
    }
}