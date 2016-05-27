using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microbrewit.Api.Model.Database;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class HopAromaWheelResolver : ValueResolver<Hop, IEnumerable<string>>
    {
        protected override IEnumerable<string> ResolveCore(Hop hop)
        {
            return hop.AromaWheels.Select(a => a.Name);
        }
    }
}