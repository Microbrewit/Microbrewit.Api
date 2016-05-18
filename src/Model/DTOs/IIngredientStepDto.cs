using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public interface IIngredientStepDto
    {
        [JsonProperty(PropertyName = "name")]
        string Name { get; set; }
        [JsonProperty(PropertyName = "type")]
        string Type { get;}
        [JsonProperty(PropertyName = "subType")]
        string SubType { get; set; }
    }
}
