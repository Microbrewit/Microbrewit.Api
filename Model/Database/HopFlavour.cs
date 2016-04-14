namespace Microbrewit.Api.Model.Database
{
    public class HopFlavour
    {
        public int FlavourId { get; set; }
        public int HopId { get; set; }

        public Hop Hop { get; set; }
        public Flavour Flavour { get; set; }
    }
}
