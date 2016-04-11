using System.Collections.Generic;
using Microbrewit.Api.Configuration;
using Microbrewit.Api.Settings;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class YeastCompleteDto
    {
        [JsonProperty(PropertyName = "links")]
        public LinksYeast Links { get; set; }
        [JsonProperty(PropertyName = "yeasts")]
        public IList<YeastDto> Yeasts { get; set; }

        public YeastCompleteDto()
        {
            Links = new LinksYeast()
            {
                YeastsSupplier = new Links()
                {
                    Href = ApiConfiguration.ApiSettings.Url + "/suplliers/:id",
                    Type = "supplier",
                }

            };
        }
    }
}