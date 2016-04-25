using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Mapper.CustomResolvers;

namespace Microbrewit.Api.Mapper.Profile
{
    public class BeerProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            // Creates a AutoMapper.Mapper for the beer class to a simpler beer class.
            CreateMap<Beer, BeerSimpleDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.BeerId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.ABV, conf => conf.MapFrom(rec => rec.ABV))
                .ForMember(dto => dto.IBU, conf => conf.MapFrom(rec => rec.IBU))
                .ForMember(dto => dto.BeerStyle, conf => conf.MapFrom(rec => rec.BeerStyle))
                .ForMember(dto => dto.SRM, conf => conf.MapFrom(rec => rec.SRM));
               
            CreateMap<Beer, BeerDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.BeerId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.ForkOfId, conf => conf.MapFrom(rec => rec.ForkeOfId))
                .ForMember(dto => dto.ForkOf, conf => conf.ResolveUsing<ForkOfResolver>())
                .ForMember(dto => dto.ABV, conf => conf.MapFrom(rec => rec.ABV))
                .ForMember(dto => dto.IBU, conf => conf.MapFrom(rec => rec.IBU))
                .ForMember(dto => dto.SRM, conf => conf.MapFrom(rec => rec.SRM))
                .ForMember(dto => dto.BeerStyle, conf => conf.MapFrom(rec => rec.BeerStyle))
                .ForMember(dto => dto.Recipe, conf => conf.MapFrom(rec => rec.Recipe))
                .ForMember(dto => dto.Breweries, conf => conf.MapFrom(rec => rec.Breweries))
                .ForMember(dto => dto.Brewers, conf => conf.MapFrom(rec => rec.Brewers));

            CreateMap<Recipe, DTO>()
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Beer.Name))
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.RecipeId));

            CreateMap<Brewery, DTO>()
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.BreweryId));

            CreateMap<Brewery,BrewerySimpleDto>()
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.BreweryId));


            CreateMap<BreweryBeer, BeerDto>()
                 .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Beer.Name))
                 .ForMember(dto => dto.IBU, conf => conf.MapFrom(rec => rec.Beer.IBU))
                 .ForMember(dto => dto.ABV, conf => conf.MapFrom(rec => rec.Beer.ABV))
                 .ForMember(dto => dto.SRM, conf => conf.MapFrom(rec => rec.Beer.SRM))
                 .ForMember(dto => dto.BeerStyle, conf => conf.MapFrom(rec => rec.Beer.BeerStyle))
                 .ForMember(dto => dto.CreatedDate, conf => conf.MapFrom(rec => rec.Beer.CreatedDate))
                 .ForMember(dto => dto.UpdatedDate, conf => conf.MapFrom(rec => rec.Beer.UpdatedDate))
                 .ForMember(dto => dto.ForkOfId, conf => conf.MapFrom(rec => rec.Beer.ForkeOfId))
                 .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.BeerId));

            CreateMap<UserBeer , DTOUser>()
               .ForMember(dto => dto.UserId, conf => conf.MapFrom(rec => rec.UserId));

            CreateMap<UserBeer, BeerSimpleDto>()
                 .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Beer.Name))
                 .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.BeerId));

            CreateMap<BreweryBeer, BeerSimpleDto>()
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Beer.Name))
                .ForMember(dto => dto.IBU, conf => conf.MapFrom(rec => rec.Beer.IBU))
                .ForMember(dto => dto.ABV, conf => conf.MapFrom(rec => rec.Beer.ABV))
                .ForMember(dto => dto.SRM, conf => conf.MapFrom(rec => rec.Beer.SRM))
                .ForMember(dto => dto.BeerStyle, conf => conf.MapFrom(rec => rec.Beer.BeerStyle))
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.BeerId));

            CreateMap<UserBeer, BeerDto>()
                 .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Beer.Name))
                 .ForMember(dto => dto.IBU, conf => conf.MapFrom(rec => rec.Beer.IBU))
                 .ForMember(dto => dto.ABV, conf => conf.MapFrom(rec => rec.Beer.ABV))
                 .ForMember(dto => dto.SRM, conf => conf.MapFrom(rec => rec.Beer.SRM))
                 .ForMember(dto => dto.BeerStyle, conf => conf.MapFrom(rec => rec.Beer.BeerStyle))
                 .ForMember(dto => dto.CreatedDate, conf => conf.MapFrom(rec => rec.Beer.CreatedDate))
                 .ForMember(dto => dto.UpdatedDate, conf => conf.MapFrom(rec => rec.Beer.UpdatedDate))
                 .ForMember(dto => dto.ForkOfId, conf => conf.MapFrom(rec => rec.Beer.ForkeOfId))
                 .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.BeerId));

            CreateMap<ABV, ABVDto>()
                .ForMember(dto => dto.Standard, conf => conf.MapFrom(rec => rec.Standard));
               

            CreateMap<IBU, IBUDto>()
                .ForMember(dto => dto.Standard, conf => conf.MapFrom(rec => rec.Standard))
                .ForMember(dto => dto.Tinseth, conf => conf.MapFrom(rec => rec.Tinseth))
                .ForMember(dto => dto.Rager, conf => conf.MapFrom(rec => rec.Rager));

            CreateMap<SRM, SRMDto>()
                .ForMember(dto => dto.Standard, conf => conf.MapFrom(rec => rec.Standard))
                .ForMember(dto => dto.Mosher, conf => conf.MapFrom(rec => rec.Mosher))
                .ForMember(dto => dto.Daniels, conf => conf.MapFrom(rec => rec.Daniels));

            CreateMap<BeerDto,Beer>()
                .ForMember(dto => dto.BeerId, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.ABV, conf => conf.MapFrom(rec => rec.ABV))
                .ForMember(dto => dto.BeerStyleId, conf => conf.MapFrom(rec => rec.BeerStyle.Id))
                .ForMember(dto => dto.IBU, conf => conf.MapFrom(rec => rec.IBU))
                .ForMember(dto => dto.SRM, conf => conf.MapFrom(rec => rec.SRM))
                .ForMember(dto => dto.Recipe, conf => conf.MapFrom(rec => rec.Recipe))
                .ForMember(dto => dto.ForkeOfId, conf => conf.MapFrom(rec => rec.ForkOfId))
                .ForMember(dto => dto.Breweries, conf => conf.MapFrom(rec => rec.Breweries))
                .ForMember(dto => dto.Brewers, conf => conf.ResolveUsing<BeerBrewerResolver>());

            CreateMap<BeerDto, BeerSimpleDto>()
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.ABV, conf => conf.MapFrom(rec => rec.ABV))
                .ForMember(dto => dto.IBU, conf => conf.MapFrom(rec => rec.IBU))
                 .ForMember(dto => dto.BeerStyle, conf => conf.MapFrom(rec => rec.BeerStyle))
                .ForMember(dto => dto.SRM, conf => conf.MapFrom(rec => rec.SRM));


            CreateMap<ABVDto, ABV>()
                .ForMember(dto => dto.Standard, conf => conf.MapFrom(rec => rec.Standard));

            CreateMap<IBUDto, IBU>()
                .ForMember(dto => dto.Standard, conf => conf.MapFrom(rec => rec.Standard))
                .ForMember(dto => dto.Rager, conf => conf.MapFrom(rec => rec.Rager))
                .ForMember(dto => dto.Tinseth, conf => conf.MapFrom(rec => rec.Tinseth));

            CreateMap<SRMDto, SRM>()
                .ForMember(dto => dto.Standard, conf => conf.MapFrom(rec => rec.Standard))
                .ForMember(dto => dto.Mosher, conf => conf.MapFrom(rec => rec.Mosher))
                .ForMember(dto => dto.Daniels, conf => conf.MapFrom(rec => rec.Daniels));

            CreateMap<DTO, Brewery>()
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.BreweryId, conf => conf.MapFrom(rec => rec.Id));

            CreateMap<BrewerySimpleDto, Brewery>()
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.BreweryId, conf => conf.MapFrom(rec => rec.Id));

            CreateMap<DTOUser, UserBeer>()
                .ForMember(dto => dto.UserId, conf => conf.MapFrom(rec => rec.UserId));
               //.ForMember(dto => dto.User.Gravatar, conf => conf.MapFrom(rec => rec.Gravatar));

            CreateMap<UserBeer, DTOUser>()
               .ForMember(dto => dto.UserId, conf => conf.MapFrom(rec => rec.UserId))
               .ForMember(dto => dto.Gravatar, conf => conf.MapFrom(rec => rec.User.Gravatar));

        }
    }
}