using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class UserCompleteDto
    {
        private static readonly string apiPath = "http://dev.microbew.it/";// ConfigurationManager.AppSettings["api"];
        [JsonProperty(PropertyName = "links")]
        public Links Links { get; set; }
        [JsonProperty(PropertyName = "users")]
        public IList<UserDto> Users { get; set; }

        public UserCompleteDto()
        {
            Links = new Links()
            {
                Href = apiPath + "/breweries/:id",
                Type = "brewery"
            };
        }
    }
}