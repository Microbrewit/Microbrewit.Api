using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class RecipeSimpleCompleteDto
    {
        private static readonly string apiPath = "http://dev.microbew.it/";// ConfigurationManager.AppSettings["api"];
        [JsonProperty(PropertyName = "links")]
        public Links Links { get; set; }
        [JsonProperty(PropertyName = "recipes")]
        public IList<RecipeSimpleDto> Recipes { get; set; }

        public RecipeSimpleCompleteDto()
        {
            Links = new Links()
            {
                Href = apiPath + "/recipes/:id",
                Type = "recipes"
            };
        }
    }
}