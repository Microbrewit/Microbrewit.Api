using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{

    public class Links
    {
         [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }
         [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
}