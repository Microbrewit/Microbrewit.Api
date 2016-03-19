using Nest;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    [ElasticsearchType(Name = "abv")]
    public class ABVDto
    {
        [JsonProperty(PropertyName = "abvId")]
        public int AbvId { get; set; }
        [JsonProperty(PropertyName = "standard")]
        public double Standard { get; set; }
        [JsonProperty(PropertyName = "miller")]
        public double Miller { get; set; }
        [JsonProperty(PropertyName = "advanced")]
        public double Advanced { get; set; }
        [JsonProperty(PropertyName = "advancedAlternative")]
        public double AdvancedAlternative { get; set; }
        [JsonProperty(PropertyName = "simple")]
        public double Simple { get; set; }
        [JsonProperty(PropertyName = "alternativeSimple")]
        public double AlternativeSimple { get; set; }
    }
}