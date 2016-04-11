using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Api.Model.Database
{
    public class Glass
    {
        public int GlassId { get; set; }
        public string Name { get; set; }

        public ICollection<BeerStyleGlass> BeerStyles { get; set; }

    }
}
