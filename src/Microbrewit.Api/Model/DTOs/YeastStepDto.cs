using System.ComponentModel.DataAnnotations;
using Nest;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    [ElasticsearchType(Name = "yeast")]
    public class YeastStepDto : IIngredientStepDto
    {
        [Required]
        [JsonProperty(PropertyName = "yeastId")]
        public int YeastId { get; set; }
        [JsonProperty(PropertyName = "stepNumber")]
        public int Number { get; set; }
        [JsonProperty(PropertyName = "productCode")]
        public string ProductCode { get; set; }
        [JsonProperty(PropertyName = "recipeId")]
        public int RecipeId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [Required]
        [JsonProperty(PropertyName = "amount")]
        public int Amount { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type => "yeast";
        public string SubType { get; set; }

        [JsonProperty(PropertyName = "supplier")]
        public DTO Supplier { get; set; }
    }
}