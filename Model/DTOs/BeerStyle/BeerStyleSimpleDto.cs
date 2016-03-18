using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    //[ElasticType(Name = "beerStyle")]
    public class BeerStyleSimpleDto
    {
        [JsonProperty(PropertyName = "beerStyleId")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
       
    }
}