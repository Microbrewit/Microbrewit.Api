namespace Microbrewit.Api.Model.Database
{
    public class Substitute
    {
        public int HopId { get; set; }
        public int SubstituteId { get; set; }

        public Hop Hop { get; set; }
        public Hop Sub { get; set; }
    }
}
