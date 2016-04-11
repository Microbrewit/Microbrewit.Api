using System.Collections.Generic;
using Microbrewit.Api.Configuration;
using Microbrewit.Api.Settings;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
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
                Href = ApiConfiguration.ApiSettings.Url + "/beers/:id",
                Type = "beer"
            };
        }
    }
}
