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
            foreach (var flavour in dto.Flavours)
            {
                var hopFlavour = new HopFlavour
                {
                    HopId = dto.Id,
                    Flavour = new Flavour {
                        Name = flavour,
                    }
                };
                hopFlavours.Add(hopFlavour);
            }
            return hopFlavours;
        }

    }
}
