using AutoMapper;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class HopPostAliasesResolver : ValueResolver<HopDto, string>
    {
        protected override string ResolveCore(HopDto hop)
        {
            if (hop.Aliases == null) return "";
            return string.Join(";", hop.Aliases);
        }
    }
}