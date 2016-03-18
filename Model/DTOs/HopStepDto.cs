using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    //[ElasticType(Name = "hopStep")]
    public class HopStepDto : IIngredientStepDto
    {
        
        [JsonProperty(PropertyName = "hopId")]
        [Required]
        public int HopId { get; set; }
        public string Name { get; set; }
        [JsonProperty(PropertyName = "amount")]
        [Required]
        public double Amount { get; set; }
        [JsonProperty(PropertyName = "aaValue")]
        [Required]
        public double AAValue { get; set; }
        [JsonProperty(PropertyName = "origin")]
        public DTO Origin { get; set; }
        [Required]
        public string Type => "hop";
        public string SubType { get; set; }
    }
}