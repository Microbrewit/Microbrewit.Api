﻿namespace Microbrewit.Api.Model.Database
{
    public class BoilStepHop
    {
        public int HopId { get; set; }
        public int StepNumber { get; set; }
        public int RecipeId { get; set; }
        public int AAValue { get; set; }
        public int Amount { get; set; }
        public int HopFormId { get; set; }

        public HopForm HopForm { get; set; }
        public BoilStep BoilStep { get; set; }
        public Hop Hop { get; set; }
    }
}
