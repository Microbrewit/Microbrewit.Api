using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nest;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    [ElasticsearchType(Name = "fermentationStep")]
    public class FermentationStepDto :IStepDto
    {
        [Required]
        public int StepNumber { get; set; }
        [JsonProperty(PropertyName = "length")]
        public int Length { get; set; }
        [JsonProperty(PropertyName = "volume")]
        public int Volume { get; set; }
        [JsonProperty(PropertyName = "temperature")]
        public int Temperature { get; set; }
        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }
        public string Type => "fermentation";
        public IList<IIngredientStepDto> Ingredients { get; set; }
    }
}