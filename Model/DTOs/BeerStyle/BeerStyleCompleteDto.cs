using System.Collections.Generic;
using Newtonsoft.Json;
using Microbrewit.Api.Configuration;

namespace Microbrewit.Api.Model.DTOs
{
    public class BeerStyleCompleteDto
    {
        [JsonProperty(PropertyName = "links")]
        public Links Links { get; set; }
        [JsonProperty(PropertyName = "beerStyles")]
        public IList<BeerStyleDto> BeerStyles { get; set; }

        public BeerStyleCompleteDto()
        {
            Links = new Links()
            {
                Href = ApiConfiguration.ApiSettings.Url + "/beerstyles/:id",
                Type = "beerstyle"
            };
        }
    }
}