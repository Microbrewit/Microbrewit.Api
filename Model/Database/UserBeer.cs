namespace Microbrewit.Api.Model.Database
{

    public class UserBeer
    {
        public string UserId { get; set; }
        public int BeerId { get; set; }
        public bool Confirmed { get; set; }
        public User User { get; set; }
        public Beer Beer { get; set; }

    }
}
