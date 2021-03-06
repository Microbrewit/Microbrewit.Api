﻿namespace Microbrewit.Api.Model.Database
{
    public class FermentationStepYeast
    {
        public int YeastId { get; set; }
        public int StepNumber { get; set; }
        public int RecipeId { get; set; }
        public int Amount { get; set; }

        public FermentationStep FermentationStep { get; set; }
        public Yeast Yeast { get; set; }
    }
}
