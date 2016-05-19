using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class LinksHop
    {
          
        [JsonProperty(PropertyName = "hops.origins")]
        public Links HopOrigins { get; set; }
        [JsonProperty(PropertyName = "hops.substitutions")]
        public Links HopSubstitutions { get; set; }
    }
}