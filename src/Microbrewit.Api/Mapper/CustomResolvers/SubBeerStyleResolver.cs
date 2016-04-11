using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.Database;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class SubBeerStyleResolver : ValueResolver<BeerStyle, IList<string>>
    {
        protected override IList<string> ResolveCore(BeerStyle BeerStyle)
        {
            var subStyles = new List<string>();
            if (BeerStyle.SubStyles != null)
            {
                foreach (var subStyle in BeerStyle.SubStyles)
                {
                    subStyles.Add(subStyle.Name);
                }
            }
            return subStyles;
        }

    }
}