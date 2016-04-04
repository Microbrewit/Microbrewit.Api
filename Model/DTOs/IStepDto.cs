using System.Collections.Generic;
using Newtonsoft.Json;
using Microbrewit.Api.Model.JsonTypeConverters;

namespace Microbrewit.Api.Model.DTOs
{
    public interface IStepDto
    {
        [JsonProperty(PropertyName = "type")]
        string Type { get; set; }
        [JsonConverter(typeof(IngredientJsonTypeConverter))]
        [JsonProperty(PropertyName = "ingredients")]
        IList<IIngredientStepDto> Ingredients { get; set; }
        [JsonProperty(PropertyName = "stepNumber")]
        int StepNumber { get; set; }
    }
}
