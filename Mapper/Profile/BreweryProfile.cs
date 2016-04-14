using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Mapper.CustomResolvers;
using System.Linq;

namespace Microbrewit.Api.Mapper.Profile
{
    public class BreweryProfile : AutoMapper.Profile
    {
        private string _imagePath = "";//ConfigurationManager.AppSettings["imagePath"];

        protected override void Configure()
        {
            CreateMap<Brewery, BreweryDto>()
                .ForMember(dest => dest.Id, conf => conf.MapFrom(src => src.BreweryId))
                .ForMember(dest => dest.Name, conf => conf.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, conf => conf.MapFrom(src => src.Description))
                .ForMember(dest => dest.SubType, conf => conf.MapFrom(src => src.Type))
                .ForMember(dest => dest.Members, conf => conf.MapFrom(src => src.Members))
                .ForMember(dest => dest.Beers, conf => conf.MapFrom(src => src.Beers))
                .ForMember(dest => dest.Origin, conf => conf.MapFrom(src => src.Origin))
                .ForMember(dest => dest.Avatar, conf => conf.MapFrom(src => (src.Avatar == null || !src.Avatar.Any()) ? null : src.Avatar.Contains("http://") ? src.Avatar : "avatar/" + src.Avatar))
                .ForMember(dest => dest.HeaderImage, conf => conf.MapFrom(src => (src.HeaderImage != null && src.HeaderImage.Any()) ? _imagePath + "header/" + src.HeaderImage : null))
                .ForMember(dest => dest.GeoLocation, conf => conf.ResolveUsing<BreweryGeoLocationResolver>())
                .ForMember(dest => dest.Socials, conf => conf.ResolveUsing<BrewerySocialResolver>());

            CreateMap<BreweryMember, DTOUser>()
                .ForMember(dest => dest.UserId, conf => conf.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Role, conf => conf.MapFrom(src => src.Role))
                .ForMember(dest => dest.Gravatar, conf => conf.MapFrom(src => src.Member.Gravatar));

            CreateMap<BreweryMember, BreweryMemberDto>()
                .ForMember(dest => dest.Username, conf => conf.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Role, conf => conf.MapFrom(src => src.Role))
                .ForMember(dest => dest.Avatar, conf => conf.MapFrom(src => (src.Member.Avatar != null && src.Member.Avatar.Any()) ? _imagePath + "avatar/" + src.Member.Avatar : null))
                .ForMember(dest => dest.Gravatar, conf => conf.MapFrom(src => src.Member.Gravatar));

            CreateMap<BreweryMember, BreweryDto>()
                .ForMember(dest => dest.Name, conf => conf.MapFrom(src => src.Brewery.Name))
                .ForMember(dest => dest.Id, conf => conf.MapFrom(src => src.Brewery.BreweryId))
                .ForMember(dest => dest.GeoLocation, conf => conf.ResolveUsing<BreweryMemberGeoLocationResolver>())
                .ForMember(dest => dest.SubType, conf => conf.MapFrom(src => src.Brewery.Type));

            CreateMap<BreweryBeer, BrewerySimpleDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.BreweryId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Brewery.Name));

            CreateMap<Beer, DTO>()
                .ForMember(dest => dest.Id, conf => conf.MapFrom(src => src.BeerId))
                .ForMember(dest => dest.Name, conf => conf.MapFrom(src => src.Name));

            CreateMap<BreweryDto, Brewery>()
                .ForMember(dest => dest.BreweryId, conf => conf.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, conf => conf.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, conf => conf.MapFrom(src => src.Description))
                .ForMember(dest => dest.Type, conf => conf.MapFrom(src => src.SubType))
                .ForMember(dest => dest.Members, conf => conf.ResolveUsing<BreweryMemberResolver>())
                .ForMember(dest => dest.Beers, conf => conf.MapFrom(src => src.Beers))
                .ForMember(dest => dest.OriginId, conf => conf.MapFrom(src => src.Origin.Id))
                .ForMember(dest => dest.Address, conf => conf.MapFrom(src => src.Address))
                .ForMember(dest => dest.Latitude, conf => conf.MapFrom(src => src.GeoLocation.Latitude))
                .ForMember(dest => dest.Longitude, conf => conf.MapFrom(src => src.GeoLocation.Longitude))
                .ForMember(dest => dest.Avatar, conf => conf.ResolveUsing<BreweryAvatarResolver>())
                .ForMember(dest => dest.HeaderImage, conf => conf.ResolveUsing<BrewerHeaderImageResolver>())
                .ForMember(dest => dest.Socials, conf => conf.ResolveUsing<BreweryDtoSocialResolver>());

            CreateMap<BreweryMemberDto, BreweryMember>()
               .ForMember(dest => dest.UserId, conf => conf.MapFrom(src => src.Username))
               .ForMember(dest => dest.Role, conf => conf.MapFrom(src => src.Role));

        }
    }
}