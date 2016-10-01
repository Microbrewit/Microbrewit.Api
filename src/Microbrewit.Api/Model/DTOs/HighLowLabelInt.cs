using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class HighLowLabelInt
    {
        [JsonProperty(PropertyName = "low")]
        public int? Low { get; set; }
        [JsonProperty(PropertyName = "high")]
        public int? High { get; set; }
        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }
    }
}