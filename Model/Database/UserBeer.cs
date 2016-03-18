﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Api.Model.Database
{

    public class UserBeer
    {
        public string Username { get; set; }
        public int BeerId { get; set; }
        public bool Confirmed { get; set; }
        public User User { get; set; }
        public Beer Beer { get; set; }

    }
}
