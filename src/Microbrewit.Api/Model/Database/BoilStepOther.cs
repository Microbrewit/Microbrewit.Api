﻿namespace Microbrewit.Api.Model.Database
{
    public class BoilStepOther
    {
        public int OtherId { get; set; }
        public int StepNumber { get; set; }
        public int RecipeId { get; set; }
        public int Amount { get; set; }

        public BoilStep BoilStep { get; set; }
        public Other Other { get; set; }
    }
}
