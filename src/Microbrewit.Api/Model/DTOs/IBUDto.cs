﻿using Nest;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    [ElasticsearchType(Name = "ibu")]
    public class IBUDto
    {
        [JsonProperty(PropertyName = "ibuId")]
        public int IbuId { get; set; }
        [JsonProperty(PropertyName = "standard")]
        public double Standard { get; set; }
        [JsonProperty(PropertyName = "tinseth")]
        public double Tinseth { get; set; }
        [JsonProperty(PropertyName = "rager")]
        public double Rager { get; set; }
    }
}