using Newtonsoft.Json;
using System.Collections.Generic;
using Microbrewit.Api.Configuration;

namespace Microbrewit.Api.Model.DTOs
{
    public class HopCompleteDto
    {
        [JsonProperty(PropertyName = "links")]
        public LinksHop Links { get; set; }
        [JsonProperty(PropertyName = "hops")]
        public IEnumerable<HopDto> Hops { get; set; }

        public HopCompleteDto()
        {

            Links = new LinksHop()
            {
                HopOrigins = new Links()
                {
                    Href = ApiConfiguration.ApiSettings.Url + "/origins/:id",
                    Type = "origin"
                },
                HopSubstitutions = new Links()
                {
                    Href = ApiConfiguration.ApiSettings.Url + "/hop/:id",
                    Type = "hop"
                }

            };
        }
    }
}