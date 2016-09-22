using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Mapper.CustomResolvers;
using System.Linq;

namespace Microbrewit.Api.Mapper.Profile
{
    public class FermentableProfile : AutoMapper.Profile
    {
        protected override void Configure()
        {

            CreateMap<Fermentable, FermentableDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.FermentableId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
                .ForMember(dto => dto.Lovibond, conf => conf.MapFrom(rec => rec.Lovibond))
                .ForMember(dto => dto.PPG, conf => conf.MapFrom(rec => rec.PPG))
                .ForMember(dto => dto.SubType, conf => conf.MapFrom(rec => rec.Type))
                .ForMember(dto => dto.SuperFermentableId, conf => conf.MapFrom(rec => rec.SuperFermentableId))
                .ForMember(dto => dto.SubFermentables, conf => conf.MapFrom(rec => rec.SubFermentables))
                .ForMember(dto => dto.Flavours, conf => conf.MapFrom(rec => rec.Flavours.Select(f => f.Name)))
                .ForMember(dto => dto.Supplier, conf => conf.MapFrom(rec => rec.Supplier));
               
            
            CreateMap<Supplier,DTO>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.SupplierId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

            CreateMap<Supplier, SupplierDto>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.SupplierId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

            CreateMap<Fermentable, DTO>()
                .ForMember(dto => dto.Id, conf => conf.MapFrom(rec => rec.FermentableId))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

            CreateMap<FermentableDto,FermentablesCompleteDto>();

            CreateMap<FermentableDto, Fermentable>()
               .ForMember(dto => dto.FermentableId, conf => conf.MapFrom(rec => rec.Id))
               .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
               .ForMember(dto => dto.PPG, conf => conf.MapFrom(rec => rec.PPG))
               .ForMember(dto => dto.Type, conf => conf.MapFrom(rec => rec.SubType))
               .ForMember(dto => dto.SupplierId, conf => conf.MapFrom(rec => rec.Supplier.Id))
               .ForMember(dto => dto.SuperFermentableId, conf => conf.MapFrom(rec => rec.SuperFermentableId))
               .ForMember(dto => dto.SubFermentables, conf => conf.MapFrom(rec => rec.SubFermentables))
               .ForMember(dto => dto.Flavours, conf => conf.MapFrom(rec => rec.Flavours.Select(f => new Flavour{Name = f})))
               .ForMember(dto => dto.Supplier, conf => conf.MapFrom(rec => rec.Supplier));

            CreateMap<FermentableDto, FermentableStepDto>()
              .ForMember(dto => dto.FermentableId, conf => conf.MapFrom(rec => rec.Id))
              .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name))
              .ForMember(dto => dto.PPG, conf => conf.MapFrom(rec => rec.PPG))
              .ForMember(dto => dto.SubType, conf => conf.MapFrom(rec => rec.SubType))
              .ForMember(dto => dto.Supplier, conf => conf.MapFrom(rec => rec.Supplier));

            CreateMap<DTO,Supplier>()
                 .ForMember(dto => dto.SupplierId, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));

            CreateMap<SupplierDto, Supplier>()
                 .ForMember(dto => dto.SupplierId, conf => conf.MapFrom(rec => rec.Id))
                .ForMember(dto => dto.Name, conf => conf.MapFrom(rec => rec.Name));
        }
    }
}