using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    //[ElasticType(Name = "otherStep")]
    public class OtherStepDto : IIngredientStepDto
    {
        [Required]
        [JsonProperty(PropertyName = "otherId")]
        public int OtherId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type => "other";
        public string SubType { get; set; }

        [Required]
        [JsonProperty(PropertyName = "amount")]
        public int Amount { get; set; }
    }
}