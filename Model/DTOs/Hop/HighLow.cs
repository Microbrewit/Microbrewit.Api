using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class HighLow
    {
        [JsonProperty(PropertyName = "low")]
        public double Low { get; set; }
        [JsonProperty(PropertyName = "high")]
        public double High { get; set; }

    }
}
