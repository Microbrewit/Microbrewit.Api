namespace Microbrewit.Api.Model.Database
{
	public class HopBeerStyle
	{
		public int HopId { get; set; }
		public int BeerStyleId { get; set; }
		public Hop Hop { get; set; }
		public BeerStyle BeerStyle { get; set; }
	}
}