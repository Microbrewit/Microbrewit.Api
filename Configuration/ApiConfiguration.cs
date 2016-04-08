using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.OptionsModel;

namespace Microbrewit.Api.Settings
{
    public static class ApiConfiguration
    {
        public static ApiSettings ApiSettings { get; set; }

    }
}
