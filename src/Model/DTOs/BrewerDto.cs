using Nest;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    [ElasticsearchType(Name = "brewer")]
    public class BrewerDto
    {
        [JsonProperty(PropertyName = "brewerId")]
        public string Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string BrewerName { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string DataType => "brewer";
    }

}