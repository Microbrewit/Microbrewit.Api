using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class SourceDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("site")]
        public string Site { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
