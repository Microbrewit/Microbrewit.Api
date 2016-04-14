using System.ComponentModel.DataAnnotations;
using Nest;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    [ElasticsearchType(Name = "other")]
    public class OtherDto : IIngredientDto
    {
        [JsonProperty(PropertyName = "otherId")]
        public int Id { get; set; }
        [Required]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string SubType { get; set; }
        [JsonProperty(PropertyName = "subtype")]
        public string Type => "other";

        [JsonProperty(PropertyName = "custom")]
        public bool Custom { get; set; }
    }
}