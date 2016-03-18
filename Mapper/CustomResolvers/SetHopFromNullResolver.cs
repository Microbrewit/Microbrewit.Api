using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class SetHopFromNullResolver : ValueResolver<HopStepDto,HopForm>
    {
        protected override HopForm ResolveCore(HopStepDto source)
        {
            return null;
        }
    }
}