using System.Collections.Generic;
using Microbrewit.Api.Settings;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class UserCompleteDto
    {
        [JsonProperty(PropertyName = "links")]
        public Links Links { get; set; }
        [JsonProperty(PropertyName = "users")]
        public IList<UserDto> Users { get; set; }

        public UserCompleteDto()
        {
            Links = new Links()
            {
                Href = ApiConfiguration.ApiSettings.Url + "/breweries/:id",
                Type = "brewery"
            };
        }
    }
}