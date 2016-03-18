using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Service.Interface;

namespace Microbrewit.Api.Service.Component
{
    public class BeerStyleService : IBeerStyleService
    {
        private readonly IBeerStyleRepository _beerStyleRepository;

        public BeerStyleService(IBeerStyleRepository beerStyleRepository)
        {
            _beerStyleRepository = beerStyleRepository;
        }
        public async Task<IEnumerable<BeerStyleDto>> GetAllAsync(int @from, int size)
        {
            var beerStyles = await _beerStyleRepository.GetAllAsync(from, size);
            return AutoMapper.Mapper.Map<IEnumerable<BeerStyle>, IEnumerable<BeerStyleDto>>(beerStyles);
        }

        public async Task<BeerStyleDto> GetSingleAsync(int id)
        {
            var beerStyle = await _beerStyleRepository.GetSingleAsync(id);
            return AutoMapper.Mapper.Map<BeerStyle, BeerStyleDto>(beerStyle);
        }

        public async Task<BeerStyleDto> AddAsync(BeerStyleDto beerStyleDto)
        {
            var beerStyle = AutoMapper.Mapper.Map<BeerStyleDto, BeerStyle>(beerStyleDto);
            await _beerStyleRepository.AddAsync(beerStyle);
            var result = await _beerStyleRepository.GetSingleAsync(beerStyle.BeerStyleId);
            return AutoMapper.Mapper.Map<BeerStyle, BeerStyleDto>(result);
        }

        public async Task<BeerStyleDto> DeleteAsync(int id)
        {
            var beerStyle = await _beerStyleRepository.GetSingleAsync(id);
            if (beerStyle != null) await _beerStyleRepository.RemoveAsync(beerStyle);
            return AutoMapper.Mapper.Map<BeerStyle,BeerStyleDto>(beerStyle);
        }

        public async Task UpdateAsync(BeerStyleDto beerStyleDto)
        {
            var beerStyle = AutoMapper.Mapper.Map<BeerStyleDto, BeerStyle>(beerStyleDto);
            await _beerStyleRepository.UpdateAsync(beerStyle);
            await _beerStyleRepository.GetSingleAsync(beerStyleDto.Id);
        }

        public async Task<IEnumerable<BeerStyleDto>> SearchAsync(string query, int @from, int size)
        {
            throw new NotImplementedException();
        }

        public async Task ReIndexElasticSearch()
        {
            throw new NotImplementedException();
        }
    }
}
