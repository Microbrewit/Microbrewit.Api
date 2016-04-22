using System.ComponentModel.DataAnnotations;
using Nest;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    [ElasticsearchType(Name = "breweryMember")]
    public class BreweryMemberDto
    {
        [Required]
        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }
        [JsonProperty(PropertyName = "gravatar")]
        public string Gravatar { get; set; }
        [JsonProperty(PropertyName = "avatar")]
        public string Avatar { get; set; }
        [JsonProperty(PropertyName = "role")]
        public string Role { get; set; }
        [JsonProperty(PropertyName = "dataType")]
        public string DataType { get { return "breweryMember"; } }

    }
}
