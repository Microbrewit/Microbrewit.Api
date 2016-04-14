namespace Microbrewit.Api.Model.Database
{
    public class BreweryMember
    {
        //public int Id { get; set; }
        public string Role { get; set; }
        public string UserId { get; set; }
        public int BreweryId { get; set; }
        public bool Confirmed { get; set; }

        public User Member { get; set; }
        public Brewery Brewery { get; set; }
    }
}
