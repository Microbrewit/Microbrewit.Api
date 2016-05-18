using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class HopPostComplete
    {
        [JsonProperty(PropertyName = "hops")]
        public IList<HopDto> Hops { get; set; }
    }
}
