using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class YeastLinks
    {
        [JsonProperty(PropertyName = "supplierid")]
        public int SupplierId { get; set; }
    }
}