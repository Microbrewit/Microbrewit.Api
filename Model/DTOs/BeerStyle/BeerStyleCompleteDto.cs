using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class BeerStyleCompleteDto
    {
        private static readonly string apiPath = "";//ConfigurationManager.AppSettings["api"];
        //public Meta Meta { get; set; }
        [JsonProperty(PropertyName = "links")]
        public Links Links { get; set; }
        [JsonProperty(PropertyName = "beerStyles")]
        public IList<BeerStyleDto> BeerStyles { get; set; }

        public BeerStyleCompleteDto()
        {
            Links = new Links()
            {
                Href = apiPath + "/beerstyles/:id",
                Type = "beerstyle"
            };
        }
    }
}