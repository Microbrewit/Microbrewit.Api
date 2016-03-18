using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class RecipeCompleteDto
    {
        private static readonly string apiPath = "http://dev.microbew.it/";// ConfigurationManager.AppSettings["api"];
        [JsonProperty(PropertyName = "links")]
        public Links Links { get; set; }
        [JsonProperty(PropertyName = "recipes")]
        public IList<RecipeDto> Recipes { get; set; }

        public RecipeCompleteDto()
        {
            Links = new Links()
            {
                Href = apiPath + "/recipes/:id",
                Type = "recipes"
            };
        }
    }
}