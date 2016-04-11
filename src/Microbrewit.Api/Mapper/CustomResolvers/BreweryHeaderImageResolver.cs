using System.Linq;
using AutoMapper;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class BrewerHeaderImageResolver : ValueResolver<BreweryDto, string>
    {
        protected override string ResolveCore(BreweryDto source)
        {
            if (source.HeaderImage == null) return string.Empty;
            var image = source.HeaderImage.Split('/').LastOrDefault();
            return image;
        }
    }
}
