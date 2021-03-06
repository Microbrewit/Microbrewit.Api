﻿namespace Microbrewit.Api.Model.Database
{
    public class BoilStepFermentable
    {
        public int FermentableId { get; set; }
        public int StepNumber { get; set; }
        public int RecipeId { get; set; }
        public int Amount { get; set; }
        public double Lovibond { get; set; }
        public int PPG { get; set; }

        public BoilStep BoilStep { get; set; }
        public Fermentable Fermentable { get; set; }
    }
}
