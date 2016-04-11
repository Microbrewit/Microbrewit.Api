using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class UserGeoLocationResolver : ValueResolver<User,GeoLocationDto>
    {
        protected override GeoLocationDto ResolveCore(User brewery)
        {
            return new GeoLocationDto
            {
                Latitude = brewery.Latitude,
                Longitude = brewery.Longitude
            };
        }
    }
}