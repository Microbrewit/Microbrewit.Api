using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class AcidDto
    {
        [JsonProperty(PropertyName = "alpha")]
        public AlphaAcidDto AlphaAcid { get; set; }
        [JsonProperty(PropertyName = "beta")]
        public BetaAcidDto BetaAcid { get; set; }
    }
}
