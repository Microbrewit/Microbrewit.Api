namespace Microbrewit.Api.Model.Database
{
    public class IBU
    {
        public int IbuId { get; set; }
        public double Standard { get; set; }
        public double Tinseth { get; set; }
        public double Rager { get; set; }

        public Beer Beer { get; set; }

    }
}
