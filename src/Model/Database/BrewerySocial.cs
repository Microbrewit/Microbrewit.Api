namespace Microbrewit.Api.Model.Database
{
    public class BrewerySocial
    {
        public int SocialId { get; set; }
        public int BreweryId { get; set; }
        public string Site { get; set; }
        public string Url { get; set; }
        public Brewery Brewery { get; set; }
    }
}
