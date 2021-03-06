﻿using System.Collections.Generic;

namespace Microbrewit.Api.Model.Database
{
    public class MashStep
    {
        //public int Id { get; set; }
        public int StepNumber { get; set; }
        public int Temperature { get; set; }
        public string Type { get; set; }
        public decimal Length { get; set; }
        public int Volume { get; set; }
        public string Notes { get; set; }
        public int RecipeId { get; set; }
        
        public Recipe Recipe { get; set; }
        public ICollection<MashStepFermentable> Fermentables { get; set; }
        public ICollection<MashStepHop> Hops { get; set; }
        public ICollection<MashStepOther> Others { get; set; }




    }
}
