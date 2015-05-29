using Newtonsoft.Json;

namespace Shots.Web.Models
{
    public class PageInfo
    {
        [JsonProperty("is_first_page")]
        public bool IsFirstPage { get; set; }

        [JsonProperty("last_id")]
        public object LastId { get; set; }

        [JsonProperty("min_id")]
        public object MinId { get; set; }
        
        public long EntryCount { get; set; }
    }
}