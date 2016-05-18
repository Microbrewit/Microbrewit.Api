using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using Nest;

namespace Microbrewit.Api.Model.DTOs
{
    [ElasticsearchType(Name = "origin")]
    public class OriginDto
    {
        [JsonProperty(PropertyName = "originId")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        [Required]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type => "origin";
    }
}
