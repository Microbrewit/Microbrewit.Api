using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class OtherCompleteDto
    {
        [JsonProperty(PropertyName = "links")]
        public LinksOther Links { get; set; }
        [JsonProperty(PropertyName = "others")]
        public IList<OtherDto> Others { get; set; }
    }
}