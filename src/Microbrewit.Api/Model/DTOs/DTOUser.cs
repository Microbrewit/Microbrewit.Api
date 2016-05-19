using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class DTOUser
    {
        [Required]
        [JsonProperty(PropertyName = "id")]
        public string UserId { get; set; }
        [JsonProperty(PropertyName = "gravatar")]
        public string Gravatar { get; set; }
        [JsonProperty(PropertyName = "role")]
        public string Role { get; set; }
        [JsonProperty(PropertyName = "dataType")]
        public string DataType { get { return "user"; } }
    }
}
