using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microbrewit.Api.Model.Database;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class UserSocialResolver : ValueResolver<User,Dictionary<string,string>>
    {
        protected override Dictionary<string, string> ResolveCore(User source)
        {
            var socials = new Dictionary<string, string>();
            if (source.Socials == null) return socials;
            foreach (var social in source.Socials.GroupBy(s => s.Site).Select(s => s.First()))
            {
                socials.Add(social.Site,social.Url);
            }
            return socials;
        }
    }
}
