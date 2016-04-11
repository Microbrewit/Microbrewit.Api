using System.Linq;
using AutoMapper;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class UserAvatarResolver : ValueResolver<UserDto, string>
    {
        protected override string ResolveCore(UserDto source)
        {
            if (source.Avatar == null) return string.Empty;
            var image = source.Avatar.Split('/').LastOrDefault();
            return image;
        }
    }
}
