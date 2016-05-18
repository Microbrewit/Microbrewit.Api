using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class HopAcidResolver : ValueResolver<Hop, AcidDto>
    {
        protected override AcidDto ResolveCore(Hop hop)
        {
            var acid = new AcidDto
            {
                AlphaAcid = new AlphaAcidDto
                {
                    Low = hop.AALow,
                    High = hop.AAHigh
                },
                BetaAcid = new BetaAcidDto
                {
                    High = hop.BetaHigh,
                    Low = hop.BetaLow,
                }
            };
            return acid;
        }
    }
}