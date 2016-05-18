using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class LinksFermentable
    {
        [JsonProperty(PropertyName = "fermentables.maltster")]
        public Links FermentablesMaltster { get; set; }
     

    }
}