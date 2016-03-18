using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class BreweryCompleteDto
    {
        private static readonly string apiPath = "http://dev.microbew.it/";// ConfigurationManager.AppSettings["api"];
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
                    Href = apiPath + "/users/:username",
                    Type = "user"
                },
                User = new Links()
                {
                    Href = apiPath + "beers/:id",
                    Type = "beer"
                }

            };
        }
    }
}
