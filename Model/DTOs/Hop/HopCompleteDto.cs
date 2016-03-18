using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microbrewit.Api.Model.DTOs
{
    public class HopCompleteDto
    {
        private static readonly string apiPath = "http://dev.microbew.it/";// ConfigurationManager.AppSettings["api"];
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
                    Href = apiPath + "/origins/:id",
                    Type = "origin"
                },
                HopSubstitutions = new Links()
                {
                    Href = apiPath + "/hop/:id",
                    Type = "hop"
                }

            };
        }
    }
}