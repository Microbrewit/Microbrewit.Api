using System.Collections.Generic;

namespace Microbrewit.Api.Model.Database
{
    public class Recipe
    {

        public int RecipeId { get; set; }
        public int Volume { get; set; }
        public string Notes { get; set; }
        public double OG { get; set; }
        public double FG { get; set; }
        public double Efficiency { get; set; }
        public int TotalBoilTime { get; set; }
        public Beer Beer { get; set; }
        public ICollection<SpargeStep> SpargeSteps { get; set; }
        public ICollection<MashStep> MashSteps { get; set; }
        public ICollection<BoilStep> BoilSteps { get; set; }
        public ICollection<FermentationStep> FermentationSteps { get; set; }
        

       
    }
}