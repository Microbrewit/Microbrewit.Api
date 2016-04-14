namespace Microbrewit.Api.Model.Database
{
    public class BreweryBeer
    {
        public int BreweryId { get; set; }
        public int BeerId { get; set; }
        public Beer Beer { get; set; }
        public Brewery Brewery { get; set; }
    }
}
