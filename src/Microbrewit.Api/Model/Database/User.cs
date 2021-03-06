﻿using System.Collections.Generic;

namespace Microbrewit.Api.Model.Database
{
    public class User
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Settings { get; set; }
        public string Gravatar { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string HeaderImage { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Avatar { get; set; }
        public bool EmailConfirmed { get; set; }
        public IList<string> Roles { get; set; }

        public ICollection<UserSocial> Socials { get; set; }
        public ICollection<BreweryMember> Breweries { get; set; }
        public ICollection<UserBeer> Beers { get; set; }
    }
}
