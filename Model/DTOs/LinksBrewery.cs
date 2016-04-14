using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class LinksBrewery
    {
        [JsonProperty(PropertyName = "beer")]
        public Links Beer { get; set; }
        [JsonProperty(PropertyName = "user")]
        public Links User { get; set; }
    }
}
