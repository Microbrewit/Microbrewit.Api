using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    // [ElasticType(Name = "fermentableStep")]
    public class FermentableStepDto : IIngredientStepDto
    {
        [Required]
        [JsonProperty(PropertyName = "fermentableId")]
        public int FermentableId { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "lovibond")]
        public double Lovibond { get; set; }
        [JsonProperty(PropertyName = "ppg")]
        public double PPG { get; set; }
        [JsonProperty(PropertyName = "supplier")]
        public SupplierDto Supplier { get; set; }
        public string Type => "fermentable";
        public string SubType { get; set; }
        [Required]
        [JsonProperty(PropertyName = "amount")]
        public int Amount { get; set; }
    }
}