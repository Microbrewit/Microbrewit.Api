using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Nest;

namespace Microbrewit.Api.Model.DTOs
{
    [ElasticsearchType(Name = "fermentable")]
    public class FermentableDto : IIngredientDto
    {
        [JsonProperty(PropertyName = "fermentableId")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "supplier")]
        public SupplierDto Supplier { get; set; }
        [Required]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "lovibond")]
        public double Lovibond { get; set; }
        [JsonProperty(PropertyName = "ppg")]
        public int PPG { get; set; }
        [Required]
        [JsonProperty(PropertyName = "subType")]
        public string SubType { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type => "fermentable";
        [Required]
        [JsonProperty(PropertyName = "custom")]
        public bool Custom { get; set; }
        [JsonProperty(PropertyName = "superFermentableId")]
        public int? SuperFermentableId { get; set; }
        [JsonProperty(PropertyName = "subFermentables")]
        public IList<DTO> SubFermentables { get; set; }
    }

}