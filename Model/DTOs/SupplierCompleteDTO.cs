using System.Collections.Generic;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class SupplierCompleteDto
    {
        private static readonly string apiPath = "http://dev.microbew.it/";// ConfigurationManager.AppSettings["api"];
        [JsonProperty(PropertyName = "links")]
        public Links Links { get; set; }
        [JsonProperty(PropertyName = "suppliers")]
        public IList<SupplierDto> Suppliers { get; set; }

        public SupplierCompleteDto()
        {
            Links = new Links()
            {
                Href = apiPath + "/origins/:id",
                Type = "origin"
            };
        }
    }
}