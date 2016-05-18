using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class UserDtoUserBeerResolver : ValueResolver<UserDto, IList<UserBeer>>
    {
        protected override IList<UserBeer> ResolveCore(UserDto userDto)
        {
            var userBeers = new List<UserBeer>();
            if (userDto.Beers == null || !userDto.Beers.Any()) return userBeers;
            foreach (var beerDto in userDto.Beers)
            {
                var userBeer = new UserBeer
                {
                    UserId = userDto.UserId,
                    BeerId = beerDto.Id,
                };
                userBeers.Add(userBeer);
            }
            return userBeers;
        }
    }
}