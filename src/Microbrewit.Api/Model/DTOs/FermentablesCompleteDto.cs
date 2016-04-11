using Newtonsoft.Json;
using System.Collections.Generic;
using Microbrewit.Api.Configuration;

namespace Microbrewit.Api.Model.DTOs
{
    public class FermentablesCompleteDto
    {
        [JsonProperty(PropertyName = "links")]
        public LinksFermentable Links { get; set; }
        [JsonProperty(PropertyName = "fermentables")]
        public ICollection<FermentableDto> Fermentables { get; set; }

        public FermentablesCompleteDto()
        {
            Links = new LinksFermentable()
            {
                
                FermentablesMaltster = new Links()
                {
                    Href = ApiConfiguration.ApiSettings.Url + "/supplier/:id",
                    Type = "supplier",
                }
            };
        }
    }
}