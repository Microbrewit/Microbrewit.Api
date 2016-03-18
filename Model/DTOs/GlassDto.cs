using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    // [ElasticType(Name = "glass")]
    public class GlassDto
    {
        [JsonProperty(PropertyName = "glassId")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "dataType")]
        public string DataType { get { return "glass"; } }
    }
}
