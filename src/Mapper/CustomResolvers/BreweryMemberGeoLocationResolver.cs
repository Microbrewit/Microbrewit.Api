using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class BreweryMemberGeoLocationResolver : ValueResolver<BreweryMember, GeoLocationDto>
    {
        
        protected override GeoLocationDto ResolveCore(BreweryMember breweryMember)
        {
            
            return new GeoLocationDto
            {
                Latitude = breweryMember.Brewery.Latitude,
                Longitude = breweryMember.Brewery.Longitude
            };
        }
    }
}