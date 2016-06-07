using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Mapper.CustomResolvers;
using System.Linq;

namespace Microbrewit.Api.Mapper.Profile
{
    public class UserProfile : AutoMapper.Profile
    {
        private string _imagePath = "";//ConfigurationManager.AppSettings["imagePath"];

        protected override void Configure()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.UserId, conf => conf.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Username, conf => conf.MapFrom(src => src.Username))
                .ForMember(dest => dest.Gravatar, conf => conf.MapFrom(src => src.Gravatar))
                .ForMember(dest => dest.Breweries, conf => conf.MapFrom(src => src.Breweries))
                .ForMember(dest => dest.Beers, conf => conf.MapFrom(src => src.Beers))
                .ForMember(dest => dest.Roles, conf => conf.MapFrom(src => src.Roles))
                .ForMember(dest => dest.Avatar, conf => conf.MapFrom(src => (src.Avatar != null && src.Avatar.Any()) ? _imagePath + "avatar/" + src.Avatar : null))
                .ForMember(dest => dest.HeaderImage, conf => conf.MapFrom(src => (src.HeaderImage != null && src.HeaderImage.Any()) ? _imagePath + "header/" + src.HeaderImage : null))
                .ForMember(dest => dest.GeoLocation, conf => conf.ResolveUsing<UserGeoLocationResolver>())
                .ForMember(dest => dest.EmailConfirmed, conf => conf.MapFrom(src => src.EmailConfirmed))
                .ForMember(dest => dest.Socials, conf => conf.ResolveUsing<UserSocialResolver>())
                .ForMember(dest => dest.Settings, conf => conf.MapFrom(src => src.Settings));


            CreateMap<UserPostDto, User>()
               .ForMember(dest => dest.Username, conf => conf.MapFrom(src => src.Username))
               .ForMember(dest => dest.Settings, conf => conf.MapFrom(src => src.Settings))
               .ForMember(dest => dest.Socials, conf => conf.ResolveUsing<UserPostDtoSocialResolver>())
               .ForMember(dest => dest.Latitude, conf => conf.MapFrom(src => src.GeoLocation.Latitude))
               .ForMember(dest => dest.Longitude, conf => conf.MapFrom(src => src.GeoLocation.Longitude));         

            CreateMap<UserPostDto, UserDto>()
                .ForMember(dest => dest.Username, conf => conf.MapFrom(src => src.Username))
                .ForMember(dest => dest.Settings, conf => conf.MapFrom(src => src.Settings))
                .ForMember(dest => dest.GeoLocation, conf => conf.MapFrom(src => src.GeoLocation.Latitude));

            CreateMap<UserPutDto, UserDto>()
                .ForMember(dest => dest.Username, conf => conf.MapFrom(src => src.Username))
                .ForMember(dest => dest.Settings, conf => conf.MapFrom(src => src.Settings))
                .ForMember(dest => dest.Socials, conf => conf.MapFrom(src => src.Socials))
                .ForMember(dest => dest.Avatar, conf => conf.MapFrom(src => src.Avatar))
                .ForMember(dest => dest.HeaderImage, conf => conf.MapFrom(src => src.HeaderImage))
                .ForMember(dest => dest.GeoLocation, conf => conf.MapFrom(src => src.GeoLocation));

            CreateMap<UserDto, User>()
               .ForMember(dest => dest.Username, conf => conf.MapFrom(src => src.UserId))
               .ForMember(dest => dest.Username, conf => conf.MapFrom(src => src.Username))
               .ForMember(dest => dest.Gravatar, conf => conf.MapFrom(src => src.Gravatar))
               .ForMember(dest => dest.Breweries, conf => conf.ResolveUsing<UserDtoBreweryMemberResolver>())
               .ForMember(dest => dest.Beers, conf => conf.ResolveUsing<UserDtoUserBeerResolver>())
               .ForMember(dest => dest.Latitude, conf => conf.MapFrom(src => src.GeoLocation.Latitude))
               .ForMember(dest => dest.Longitude, conf => conf.MapFrom(src => src.GeoLocation.Longitude))
               .ForMember(dest => dest.Avatar, conf => conf.ResolveUsing<UserAvatarResolver>())
               .ForMember(dest => dest.HeaderImage, conf => conf.ResolveUsing<UserHeaderImageResolver>())
               .ForMember(dest => dest.Socials, conf => conf.ResolveUsing<UserDtoSocialResolver>())
               .ForMember(dest => dest.EmailConfirmed, conf => conf.MapFrom(src => src.EmailConfirmed))
               .ForMember(dest => dest.Settings, conf => conf.MapFrom(src => src.Settings));


            CreateMap<User, DTOUser>()
                .ForMember(dest => dest.UserId, conf => conf.MapFrom(src => src.Username))
                .ForMember(dest => dest.Gravatar, conf => conf.MapFrom(src => src.Gravatar));
        }


    }
}