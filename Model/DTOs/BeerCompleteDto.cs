using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    //  [ElasticType(Name = "beerComplete")]
    public class BeerCompleteDto
    {
        [JsonProperty(PropertyName = "links")]
        public Links Links { get; set; }
        [JsonProperty(PropertyName = "beers")]
        public IEnumerable<BeerDto> Beers { get; set; }
        public BeerCompleteDto()
        {
            Links = new Links()
            {
                Href = "http://api.microbrew.it/beers/:id",
                Type = "beer"
            };
        }
    }
}
