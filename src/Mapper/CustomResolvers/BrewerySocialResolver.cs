using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.Database;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class BrewerySocialResolver : ValueResolver<Brewery,Dictionary<string,string>>
    {
        protected override Dictionary<string, string> ResolveCore(Brewery source)
        {
            var socials = new Dictionary<string, string>();
            if (source.Socials == null) return socials;
            foreach (var social in source.Socials)
            {
                socials.Add(social.Site,social.Url);
            }
            return socials;
        }
    }
}
