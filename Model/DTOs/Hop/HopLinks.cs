using Newtonsoft.Json;
using System.Collections.Generic;

namespace Microbrewit.Api.Model.DTOs
{
    public class HopLinks
    {
        [JsonProperty(PropertyName = "originId")]
        public int? OriginId { get; set; }
        [JsonProperty(PropertyName = "substitutesIds")]
        public IList<int> SubstitutesIds { get; set; }
    }
}