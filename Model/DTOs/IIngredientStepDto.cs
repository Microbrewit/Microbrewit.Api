using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public interface IIngredientStepDto
    {
        [JsonProperty(PropertyName = "name")]
        string Name { get; set; }
        [JsonProperty(PropertyName = "type")]
        string Type { get;}
        [JsonProperty(PropertyName = "subType")]
        string SubType { get; set; }
    }
}
