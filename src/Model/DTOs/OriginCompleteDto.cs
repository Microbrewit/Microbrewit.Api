using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class OriginCompleteDto
    {
        [JsonProperty(PropertyName = "links")]
        public Links Links { get; set; }
        [JsonProperty(PropertyName = "origins")]
        public IEnumerable<OriginDto> Origins { get; set; }

        public OriginCompleteDto()
        {
            Links = new Links();
        }
    }
}
