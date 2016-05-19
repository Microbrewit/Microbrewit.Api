using System.Collections.Generic;

namespace Microbrewit.Api.Model.Database
{
    public class Flavour
    {
        public int FlavourId { get; set; }
        public string Name { get; set; }

        public virtual ICollection<HopFlavour> Hops { get; set; }
    }
}
