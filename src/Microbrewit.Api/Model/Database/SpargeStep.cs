﻿using System.Collections.Generic;

namespace Microbrewit.Api.Model.Database
{
    public class SpargeStep
    {
        public int StepNumber { get; set; }
        public int Temperature { get; set; }
        public int Amount { get; set; }
        public string Notes { get; set; }
        public string Type { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public ICollection<SpargeStepHop> Hops { get; set; }
    }
}
