namespace Microbrewit.Api.Model.Database
{
    public class BeerStyleGlass
    {
        public int BeerStyleId { get; set; }
        public int GlassId { get; set; }

        public BeerStyle BeerStyle { get; set; }
        public Glass Glass { get; set; }

    }
}
