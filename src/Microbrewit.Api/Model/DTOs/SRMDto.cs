using Nest;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    [ElasticsearchType(Name = "srm")]
    public class SRMDto
    {
        [JsonProperty(PropertyName = "srmId")]
        public int SrmId { get; set; }
        // Malt Calculate Units
        [JsonProperty(PropertyName = "standard")]
        public double Standard { get; set; }
        [JsonProperty(PropertyName = "mosher")]
        public double Mosher { get; set; }
        [JsonProperty(PropertyName = "daniels")]
        public double Daniels { get; set; }
        [JsonProperty(PropertyName = "morey")]
        public double Morey { get; set; }
    }
}