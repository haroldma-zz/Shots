using Newtonsoft.Json;

namespace Shots.Api.Models
{
    public class PageInfo
    {
        public int SqlCursor { get; set; }
        public int Rcursor { get; set; }
        public int Cursor { get; set; }

        [JsonProperty("last_id")]
        public long LastId { get; set; }
        [JsonProperty("min_id")]
        public long MinId { get; set; }
    }
}