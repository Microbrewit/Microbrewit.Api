using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class OriginCompleteDto
    {
        private static readonly string apiPath = "http://dev.microbew.it/";// ConfigurationManager.AppSettings["api"];
        [JsonProperty(PropertyName = "links")]
        public Links Links { get; set; }
        [JsonProperty(PropertyName = "origins")]
        public IEnumerable<OriginDto> Origins { get; set; }
    }
}
