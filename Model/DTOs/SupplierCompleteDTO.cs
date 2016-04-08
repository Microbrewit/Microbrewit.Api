using System.Collections.Generic;
using Microbrewit.Api.Settings;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class SupplierCompleteDto
    {
        [JsonProperty(PropertyName = "links")]
        public Links Links { get; set; }
        [JsonProperty(PropertyName = "suppliers")]
        public IList<SupplierDto> Suppliers { get; set; }

        public SupplierCompleteDto()
        {
            Links = new Links()
            {
                Href = ApiConfiguration.ApiSettings.Url + "/origins/:id",
                Type = "origin"
            };
        }
    }
}