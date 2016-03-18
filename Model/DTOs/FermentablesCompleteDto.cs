using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microbrewit.Api.Model.DTOs
{
    public class FermentablesCompleteDto
    {
        private static readonly string apiPath = "http://dev.microbew.it/";// ConfigurationManager.AppSettings["api"];
        //public Meta Meta { get; set; }
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
                    Href = apiPath + "/supplier/:id",
                    Type = "supplier",
                }
            };
        }
    }
}