using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microbrewit.Api.Model.DTOs
{
    public class SpargeStepDto : IStepDto
    {
        [Required]
        [JsonProperty(PropertyName = "stepNumber")]
        public int StepNumber { get; set; }
        [JsonProperty(PropertyName = "recipeId")]
        public int RecipeId { get; set; }
        [JsonProperty(PropertyName = "temperature")]
        public int Temperature { get; set; }
        [Required]
        [JsonProperty(PropertyName = "amount")]
        public int Amount { get; set; }
        [JsonProperty(PropertyName = "notes")]
        public string Notes { get; set; }
        public string Type => "sparge";
        public IList<IIngredientStepDto> Ingredients { get; set; }
       
    }
}
