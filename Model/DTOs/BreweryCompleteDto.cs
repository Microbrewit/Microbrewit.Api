using System.Collections.Generic;
using Microbrewit.Api.Configuration;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class BreweryCompleteDto
    {
        [JsonProperty(PropertyName = "links")]
        public LinksBrewery Links { get; set; }
        [JsonProperty(PropertyName = "breweries")]
        public IEnumerable<BreweryDto> Breweries { get; set; }

        public BreweryCompleteDto()
        {
            Links = new LinksBrewery()
            {
                Beer = new Links() 
                {
                    Href = ApiConfiguration.ApiSettings.Url + "/users/:username",
                    Type = "user"
                },
                User = new Links()
                {
                    Href = ApiConfiguration.ApiSettings.Url + "beers/:id",
                    Type = "beer"
                }

            };
        }
    }
}
