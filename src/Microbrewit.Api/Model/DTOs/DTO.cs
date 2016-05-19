using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class DTO
    {
        [Required]
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}