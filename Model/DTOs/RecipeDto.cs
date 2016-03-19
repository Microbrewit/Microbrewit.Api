using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Microbrewit.Api.Model.JsonTypeConverters;
using Nest;

namespace Microbrewit.Api.Model.DTOs
{
    [ElasticsearchType(Name = "recipe")]
    public class RecipeDto
    {
        [JsonProperty(PropertyName = "recipeId")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "notes")]
        public string  Notes { get; set; }
        [Required]
        [JsonProperty(PropertyName = "volume")]
        public int Volume { get; set; }
        [JsonProperty(PropertyName = "og")]
        public double OG { get; set; }
        [JsonProperty(PropertyName = "fg")]
        public double FG { get; set; }
        [JsonProperty(PropertyName = "efficiency")]
        public double Efficiency { get; set; }
        [JsonProperty(PropertyName = "totalBoilTime")]
        public double TotalBoilTime { get; set; }
        [Required]
        [JsonConverter(typeof(StepsJsonTypeConverter))]
        [JsonProperty(PropertyName = "steps")]
        public IList<IStepDto> Steps { get; set; }
        [JsonProperty(PropertyName = "dataType")]
        public string DataType { get { return "recipe"; } }
    }
}