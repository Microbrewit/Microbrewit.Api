namespace Microbrewit.Api.Model.Database
{
    public class UserSocial
    {
        public int SocialId { get; set; }
        public string UserId { get; set; }
        public string Site { get; set; }
        public string Url { get; set; }
        public User User { get; set; }
    }
}
