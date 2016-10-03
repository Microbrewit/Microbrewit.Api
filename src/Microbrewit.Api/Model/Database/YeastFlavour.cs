namespace Microbrewit.Api.Model.Database
{
    public class YeastFlavour
    {
        public int FlavourId { get; set; }
        public int YeastId { get; set; }
        public Yeast yeast { get; set; }
        public Flavour Flavour { get; set; }
    }
}
