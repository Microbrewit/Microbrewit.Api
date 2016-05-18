using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class LinksYeast
    {
        [JsonProperty(PropertyName = "yeasts.supplier")]
        public Links YeastsSupplier { get; set; }
    }
}