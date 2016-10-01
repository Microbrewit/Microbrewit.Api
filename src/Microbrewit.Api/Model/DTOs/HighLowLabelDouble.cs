using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class HighLowLabelDouble
    {
        [JsonProperty(PropertyName = "low")]
        public double Low { get; set; }
        [JsonProperty(PropertyName = "high")]
        public double High { get; set; }
        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }
    }
}