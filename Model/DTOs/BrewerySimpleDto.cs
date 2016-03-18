using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    //  [ElasticType(Name = "brewery")]
    public class BrewerySimpleDto
    {
        [JsonProperty(PropertyName = "breweryId")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
