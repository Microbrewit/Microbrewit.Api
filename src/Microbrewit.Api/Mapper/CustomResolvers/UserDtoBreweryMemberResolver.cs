using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class UserDtoBreweryMemberResolver : ValueResolver<UserDto,IList<BreweryMember>>
    {
        protected override IList<BreweryMember> ResolveCore(UserDto source)
        {
            
            var members = new List<BreweryMember>();
            if (source.Breweries == null) return members;

            foreach (var breweryDto in source.Breweries)
            {
                var member = new BreweryMember()
                {
                    BreweryId = breweryDto.Id,
                    MemberUsername = source.Username,
                };
                members.Add(member);
            }
            return members;
        }
    }
}