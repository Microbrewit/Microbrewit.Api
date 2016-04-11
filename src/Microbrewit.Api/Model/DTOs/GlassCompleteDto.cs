using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class GlassCompleteDto
    {
        private static readonly string apiPath = "http://dev.microbew.it/";// ConfigurationManager.AppSettings["api"];
        [JsonProperty(PropertyName = "links")]
        public Links Links { get; set; }
        [JsonProperty(PropertyName = "glasses")]
        public IList<GlassDto> Glasses { get; set; }
    }
}
