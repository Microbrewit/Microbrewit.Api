using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Microbrewit.Api.Model.DTOs
{
    // [ElasticType(Name = "origin")]
    public class OriginDto
    {
        [JsonProperty(PropertyName = "originId")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        [Required]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "dataType")]
        public string DataType { get { return "origin"; } }
    }
}
