namespace Microbrewit.Api.Model.Database
{
    public class MashStepOther
    {
        public int OtherId { get; set; }
        public int StepNumber { get; set; }
        public int RecipeId { get; set; }
        public int Amount { get; set; }

        public MashStep MashStep { get; set; }
        public Other Other { get; set; }
    }
}
