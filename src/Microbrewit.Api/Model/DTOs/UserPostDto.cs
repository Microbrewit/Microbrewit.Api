using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Microbrewit.Api.Model.Validation;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Settings;
using Microsoft.Extensions.Options;

namespace Microbrewit.Api.Model.DTOs
{
    public class UserPostDto
    {
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "brewery")]
        public DTO Brewery { get; set; }
        [JsonProperty(PropertyName = "settings")]
        public string Settings { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [JsonProperty(PropertyName = "confirmPassword")]
        public string ConfirmPassword { get; set; }
        [JsonProperty(PropertyName = "geoLocation")]
        public GeoLocationDto GeoLocation { get; set; }
        [JsonProperty(PropertyName = "headerImage")]
        public string HeaderImage { get; set; }
        [JsonProperty(PropertyName = "avatar")]
        public string Avatar { get; set; }
        [JsonProperty(PropertyName = "socials")]
        public Dictionary<string, string> Socials { get; set; }


        

        

        
    }
}