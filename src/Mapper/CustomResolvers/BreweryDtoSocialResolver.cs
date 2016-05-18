using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class BreweryDtoSocialResolver : ValueResolver<BreweryDto,IEnumerable<BrewerySocial>>
    {
        //readonly IBreweryRepository _breweryRepository = new BreweryDapperRepository();

        protected override IEnumerable<BrewerySocial> ResolveCore(BreweryDto source)
        {
            var brewerySocials = new List<BrewerySocial>();
            // var dbBrewerySocials = _breweryRepository.GetBrewerySocials(source.Id);
            // if (source.Socials == null) return brewerySocials;
            // foreach (var social in source.Socials)
            // {
            //     var brewerySocial = dbBrewerySocials.SingleOrDefault(s => s.Site == social.Key);
            //     if (brewerySocial != null)
            //     {
            //         if (brewerySocial.Url != social.Value)
            //             brewerySocial.Url = social.Value;
            //         brewerySocials.Add(brewerySocial);
            //     }
            //     else
            //     {
            //         brewerySocial = new BrewerySocial {BreweryId = source.Id, Site = social.Key, Url = social.Value};
            //         brewerySocials.Add(brewerySocial);
            //     }
            //     
            // }
            return brewerySocials;
        }
    }
}
