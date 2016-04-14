using System.Collections.Generic;

namespace Microbrewit.Api.Model.Database
{
    public class Glass
    {
        public int GlassId { get; set; }
        public string Name { get; set; }

        public ICollection<BeerStyleGlass> BeerStyles { get; set; }

    }
}
