using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class SubstitutResolver : ValueResolver<HopDto, IList<Hop>>
    {
        // private IHopElasticsearch _hopElasticsearch = new HopElasticsearch();
        // private IHopRepository _hopRepository = new HopDapperRepository();

        protected override IList<Hop> ResolveCore(HopDto dto)
        {
                List<Hop> hops = new List<Hop>();
//             if (dto.Substituts == null) return hops;
//             if (dto.Substituts != null)
//             {
//                 foreach (var sub in dto.Substituts)
//                 {
// 
//                     if (sub.Id > 0)
//                     {
//                         var hop = _hopRepository.GetSingle(sub.Id);
//                         if (hop != null)
//                         {
//                             hops.Add(hop);
//                         }
//                     }
//                     else
//                     {
//                         var hopDtos = _hopElasticsearch.Search(sub.Name, 0, 1).FirstOrDefault();
// 
//                         if (hopDtos != null)
//                         {
//                             hops.Add(Mapper.Map<HopDto, Hop>(hopDtos));
//                         }
//                     }
//                 }
//             }
            return hops;
        }
    }
}