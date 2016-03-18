using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microbrewit.Api.Model.Database;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class HopAromaWheelResolver : ValueResolver<Hop, IList<string>>
    {
        protected override IList<string> ResolveCore(Hop hop)
        {
            return (from hopFlavour in hop.AromaWheel where hopFlavour.Flavour != null select hopFlavour.Flavour.Name).ToList();
        }
    }
}