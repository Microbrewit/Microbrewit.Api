using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class UserPostDtoSocialResolver : ValueResolver<UserPostDto,IEnumerable<UserSocial>>
    {
        //readonly IUserRepository _userRepository = new UserDapperRepository();

        protected override IEnumerable<UserSocial> ResolveCore(UserPostDto source)
        {
            var userSocials = new List<UserSocial>();
            // var dbUserSocials = _userRepository.GetUserSocials(source.Username);
            // if (source.Socials == null) return userSocials;
            // foreach (var social in source.Socials)
            // {
            //     var userSocial = dbUserSocials.SingleOrDefault(s => s.Site == social.Key);
            //     if (userSocial != null)
            //     {
            //         if (userSocial.Url != social.Value)
            //             userSocial.Url = social.Value;
            //         userSocials.Add(userSocial);
            //     }
            //     else
            //     {
            //         userSocial = new UserSocial {Username = source.Username, Site = social.Key, Url = social.Value};
            //         userSocials.Add(userSocial);
            //     }
            //     
            // }
            return userSocials;
        }
    }
}
