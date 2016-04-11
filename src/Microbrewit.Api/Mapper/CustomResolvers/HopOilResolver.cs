using AutoMapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Mapper.CustomResolvers
{
    public class HopOilResolver : ValueResolver<Hop, OilDto>
    {
        protected override OilDto ResolveCore(Hop hop)
        {
            var oil = new OilDto
            {
                TotalOilDto = new TotalOilDto
                {
                    Low   = hop.TotalOilLow,
                    High = hop.TotalOilHigh,
                },
                BPineneDto = new BPineneDto
                {
                    High = hop.BPineneHigh,
                    Low = hop.BPineneLow,
                },
                HumuleneDto = new HumuleneDto
                {
                    High = hop.HumuleneHigh,
                    Low = hop.HumuleneLow
                },
                CaryophylleneDto =  new CaryophylleneDto
                {
                    High = hop.CaryophylleneHigh,
                    Low = hop.CaryophylleneLow
                },
                FarneseneDto = new FarneseneDto
                {
                    High = hop.FarneseneHigh,
                    Low = hop.FarneseneLow
                },
                GeraniolDto = new GeraniolDto
                {
                    High = hop.GeraniolHigh,
                    Low = hop.GeraniolLow,
                },
                LinalLoolDto = new LinalLoolDto
                {
                    High = hop.LinaloolHigh,
                    Low = hop.LinaloolLow
                },
                MyrceneDto = new MyrceneDto
                {
                    High = hop.MyrceneHigh,
                    Low = hop.MyrceneLow
                },
                OtherOilDto = new OtherOilDto
                {
                    High = hop.OtherOilHigh,
                    Low = hop.OtherOilLow
                }
            };
            return oil;
        }
    }
}