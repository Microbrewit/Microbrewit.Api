﻿using System.Collections.Generic;

namespace Microbrewit.Api.Model.Database
{
    public class FermentationStep
    {
        //public int Id { get; set; }
        public int StepNumber { get; set; }
        public int Length { get; set; }
        public int Temperature { get; set; }
        public string Notes { get; set; }
        public int Volume { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        public ICollection<FermentationStepHop> Hops { get; set; }
        public ICollection<FermentationStepFermentable> Fermentables { get; set; }
        public ICollection<FermentationStepOther> Others { get; set; }
        public ICollection<FermentationStepYeast> Yeasts { get; set; }

    }
}
