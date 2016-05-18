using Newtonsoft.Json;
using System.Collections.Generic;


namespace Microbrewit.Api.Model.DTOs
{
    public class BeerStyleLinks
    {
        [JsonProperty(PropertyName = "subbeerstyleids")]
        public IList<int> SubBeerStyleIds { get; set; }
    }
}