﻿using System.Collections.Generic;
using Nest;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    [ElasticsearchType(Name = "user")]
    public class UserDto
    {
        [JsonProperty(PropertyName = "id")]
        public string UserId { get; set; }
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "gravatar")]
        public string Gravatar { get; set; }
        [JsonProperty(PropertyName = "breweries")]
        public IList<BreweryDto> Breweries { get; set; }
        [JsonProperty(PropertyName = "beers")]
        public IList<BeerDto> Beers { get; set; }
        [JsonProperty(PropertyName = "settings")]
        public string Settings { get; set; }
        [JsonProperty(PropertyName = "geoLocation")]
        public GeoLocationDto GeoLocation { get; set; }
        [JsonProperty(PropertyName = "emailConfirmed")]
        public bool EmailConfirmed { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type => "user";
        [JsonProperty(PropertyName = "headerImage")]
        public string HeaderImage { get; set; }
        [JsonProperty(PropertyName = "avatar")]
        public string Avatar { get; set; }
        [JsonProperty(PropertyName =  "roles")]
        public IEnumerable<string> Roles { get; set; }
        [JsonProperty(PropertyName = "socials")]
        public Dictionary<string, string> Socials { get; set; }
    }
}