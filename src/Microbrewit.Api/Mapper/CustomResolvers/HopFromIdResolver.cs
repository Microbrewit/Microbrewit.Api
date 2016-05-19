using AutoMapper;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class HopFromIdResolver : ValueResolver<HopStepDto,int>
    {
        //private IHopRepository _hopRepository = new HopDapperRepository();
        protected override int ResolveCore(HopStepDto hopStepDto)
        {
            
            //var hopForms = _hopRepository.GetHopForms();
            //var hopForm = hopForms.FirstOrDefault(h => string.Equals(h.Name, hopStepDto.SubType, StringComparison.OrdinalIgnoreCase)) ?? hopForms.First();
            return 0;//hopForm.Id;
        }
    }
}
