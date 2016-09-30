using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class SubstitutResolver : ValueResolver<HopDto, IList<Hop>>
    {
        protected override IList<Hop> ResolveCore(HopDto dto)
        {
            List<Hop> hops = new List<Hop>();
            if (dto.Substituts == null) return hops;
            foreach (var substitute in dto.Substituts)
            {
                var hop = new Hop {
                    HopId = substitute.Id,
                    Name = substitute.Name
                };
                hops.Add(hop);
            }
            return hops;
        }
    }
}