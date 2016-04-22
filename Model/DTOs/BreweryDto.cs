using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nest;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    [ElasticsearchType(Name = "brewery")]
    public class BreweryDto
    {
        [JsonProperty(PropertyName = "breweryId")]
        public int Id { get; set; }
        [Required]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "subtype")]
        public string SubType { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type => "brewery";

        [JsonProperty(PropertyName = "members")]
        public IEnumerable<BreweryMemberDto> Members { get; set; }
        [JsonProperty(PropertyName = "beers")]
        public IEnumerable<BeerDto> Beers { get; set; }
        [JsonProperty(PropertyName = "origin")]
        public DTO Origin { get; set; }
        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }
        [JsonProperty(PropertyName = "geoLocation")]
        public GeoLocationDto GeoLocation { get; set; }
        [JsonProperty(PropertyName = "website")]
        public string Website { get; set; }
        [JsonProperty(PropertyName = "established")]
        public string Established { get; set; }
        [JsonProperty(PropertyName = "headerImage")]
        public string HeaderImage { get; set; }
        [JsonProperty(PropertyName = "avatar")]
        public string Avatar { get; set; }
        [JsonProperty(PropertyName = "isCommercial")]
        public bool IsCommercial { get; set; }
        [JsonProperty(PropertyName = "socials")]
        public Dictionary<string,string> Socials { get; set; }
    }
}
