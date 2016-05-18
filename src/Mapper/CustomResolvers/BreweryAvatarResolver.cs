using System.Linq;
using AutoMapper;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class BreweryAvatarResolver : ValueResolver<BreweryDto, string>
    {
        protected override string ResolveCore(BreweryDto source)
        {
            if (source.Avatar == null) return string.Empty;
            var image = source.Avatar.Split('/').LastOrDefault();
            return image;
        }
    }
}
