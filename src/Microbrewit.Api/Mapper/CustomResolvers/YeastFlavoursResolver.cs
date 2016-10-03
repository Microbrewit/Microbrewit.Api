using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class YeastFlavoursResolver : ValueResolver<YeastDto, IList<YeastFlavour>>
    {
        //private IHopRepository repository = new HopDapperRepository();

        protected override IList<YeastFlavour> ResolveCore(YeastDto dto)
        {
            var flavours = new List<YeastFlavour>();
            foreach (var flavourStr in dto.Flavours)
            {
                flavours.Add(new YeastFlavour{YeastId = dto.Id, Flavour = new Flavour {Name = flavourStr}});
            }
            return flavours;
        }

    }
}
