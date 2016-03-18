using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class FermentableLinks
    {
        [JsonProperty(PropertyName = "supplierid")]
        public int MaltsterId { get; set; }
    }
}