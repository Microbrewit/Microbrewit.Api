using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class BeerSimpleCompleteDto
    {
        private static readonly string apiPath = "http://dev.microbew.it/";//ConfigurationManager.AppSettings["api"];
        [JsonProperty(PropertyName = "links")]
        public Links Links { get; set; }
        [JsonProperty(PropertyName = "beers")]
        public IList<BeerSimpleDto> Beers { get; set; }

        public BeerSimpleCompleteDto()
        {
            Links = new Links()
            {
                Href = apiPath + "/beers/:id",
                Type = "beer"
            };
        }
    }
}