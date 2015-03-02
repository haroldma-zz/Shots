using Newtonsoft.Json;

namespace Shots.Api.Models
{
    public class PageInfo
    {
        [JsonProperty("is_first_page")]
        public bool IsFirstPage { get; set; }

        [JsonProperty("last_id")]
        public string LastId { get; set; }

        [JsonProperty("min_id")]
        public string MinId { get; set; }
        
        public long EntryCount { get; set; }
    }
}