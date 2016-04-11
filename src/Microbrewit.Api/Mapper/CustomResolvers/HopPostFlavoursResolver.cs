using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class HopPostFlavoursResolver : ValueResolver<HopDto, IList<HopFlavour>>
    {
        //private IHopRepository repository = new HopDapperRepository();

        protected override IList<HopFlavour> ResolveCore(HopDto dto)
        {
            var hopFlavours = new List<HopFlavour>();
            // var flavours = repository.GetFlavours();
            // if (dto.Flavours != null)
            // {
            //     foreach (var item in dto.Flavours)
            //     {
            //         var flavour = flavours.SingleOrDefault(f => f.Name == item);
            //         if (flavour != null)
            //             hopFlavours.Add(new HopFlavour { FlavourId = flavour.FlavourId, HopId = dto.Id });
            //     }
            // }
            return hopFlavours;
        }

    }
}
