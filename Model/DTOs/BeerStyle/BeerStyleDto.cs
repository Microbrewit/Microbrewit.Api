using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nest;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    [ElasticsearchType(Name = "beerStyle")]
    public class BeerStyleDto
    {
        [JsonProperty(PropertyName = "beerStyleId")]
        public int Id { get; set; }
        [Required]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "superBeerStyle")]
        public DTO SuperBeerStyle { get; set; }
        [JsonProperty(PropertyName = "og")]
        public Og Og { get; set; }
        [JsonProperty(PropertyName = "fg")]
        public Fg Fg { get; set; }
        [JsonProperty(PropertyName = "ibu")]
        public Ibu Ibu { get; set; }
        [JsonProperty(PropertyName = "srm")]
        public Srm Srm { get; set; }
        [JsonProperty(PropertyName = "abv")]
        public Abv Abv { get; set; }
        [JsonProperty(PropertyName = "comments")]
        public string Comments { get; set; }
        [JsonProperty(PropertyName = "subBeerStyles")]
        public IList<DTO> SubBeerStyles { get; set; }
        [JsonProperty(PropertyName = "hops")]
        public IList<DTO> Hops { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get { return "beerstyle"; } }
       
    }
}