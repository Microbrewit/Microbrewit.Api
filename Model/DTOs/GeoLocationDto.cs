using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
//    [ElasticType(Name = "beer")]
    public class GeoLocationDto
    {
        [JsonProperty(PropertyName = "latitude")]
        public double Latitude { get; set; }
        [JsonProperty(PropertyName = "longitude")]
        public double Longitude { get; set; }
    }
}
