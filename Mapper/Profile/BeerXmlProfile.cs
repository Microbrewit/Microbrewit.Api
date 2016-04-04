using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Mapper.CustomResolvers;

namespace Microbrewit.Api.Mapper.Profile
{
    public class BeerXmlProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            CreateMap<Model.BeerXml.Recipe, BeerDto>()
                .ForMember(dest => dest.Name, conf => conf.MapFrom(src => src.Name))
                .ForMember(dest => dest.ABV, conf => conf.MapFrom(src => new ABVDto()))
                .ForMember(dest => dest.SRM, conf => conf.MapFrom(src => new SRMDto()))
                .ForMember(dest => dest.IBU, conf => conf.MapFrom(src => new IBUDto()))
                .ForMember(dest => dest.BeerStyle, conf => conf.ResolveUsing<BeerXmlBeerStyleSimpleResolver>());
                //.ForMember(dest => dest.Recipe, conf => conf.ResolveUsing<BeerXmlResolver>());
        }
    }
}