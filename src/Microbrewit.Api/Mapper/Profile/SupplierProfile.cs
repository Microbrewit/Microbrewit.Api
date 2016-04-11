using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Mapper.CustomResolvers;

namespace Microbrewit.Api.Mapper.Profile
{
    public class SupplierProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {
            CreateMap<Supplier, SupplierDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.SupplierId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.Origin, conf => conf.MapFrom(rec => rec.Origin));
            
            CreateMap<Origin,DTO>()
               .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.OriginId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

            CreateMap<SupplierDto,Supplier>()
                 .ForMember(dto => dto.SupplierId, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.OriginId, conf => conf.MapFrom(rec => rec.Origin.Id))
                .ForMember(dto => dto.Origin, conf => conf.MapFrom(rec => rec.Origin));

            CreateMap<DTO,Origin>()
                 .ForMember(dto => dto.OriginId, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

            }
    }
}