using System.Collections.Generic;

namespace Microbrewit.Api.Model.Database
{
    public class BoilStep
    {
        public int StepNumber { get; set; }
        public int Length { get; set; }
        public int Volume { get; set; }
        public string Notes { get; set; }
        public int RecipeId { get; set; }

        public Recipe Recipe { get; set; }
        public ICollection<BoilStepHop> Hops { get; set; }
        public ICollection<BoilStepFermentable> Fermentables { get; set; }
        public ICollection<BoilStepOther> Others { get; set; }

    }
}
