using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nest;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    [ElasticsearchType(Name = "mashStep")]
    public class MashStepDto : IStepDto
    {
        [Required]
        [JsonProperty(PropertyName = "stepNumber")]
        public int StepNumber { get; set; }
        [JsonProperty(PropertyName = "length")]
        public decimal Length { get; set; }
        [JsonProperty(PropertyName = "volume")]
        public int Volume { get; set; }
        [JsonProperty(PropertyName = "temperature")]
        public int Temperature { get; set; }
        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }
        public string Type => "mash";
        [JsonProperty(PropertyName = "ingredients")]
        public IList<IIngredientStepDto> Ingredients { get; set; }


    }
}