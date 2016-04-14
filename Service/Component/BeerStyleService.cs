using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microbrewit.Api.ElasticSearch.Interface;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Service.Interface;
using Microsoft.Extensions.Logging;

namespace Microbrewit.Api.Service.Component
{
    public class BeerStyleService : IBeerStyleService
    {
        private readonly IBeerStyleRepository _beerStyleRepository;
        private readonly IBeerStyleElasticsearch _beerStyleElasticsearch;
        private readonly IHopElasticsearch _hopElasticsearch;
        private readonly IHopRepository _hopRepository;
        private readonly ILogger<BeerStyleService> _logger;
        public BeerStyleService(IBeerStyleElasticsearch beerStyleElasticsearch, 
        IBeerStyleRepository beerStyleRepository,IHopElasticsearch hopElasticsearch, IHopRepository hopRepository,
         ILogger<BeerStyleService> logger)
        {
            _beerStyleElasticsearch = beerStyleElasticsearch;
            _beerStyleRepository = beerStyleRepository;
            _hopElasticsearch = hopElasticsearch;
            _hopRepository = hopRepository;
            _logger = logger;
        }
        public async Task<IEnumerable<BeerStyleDto>> GetAllAsync(int @from, int size)
        {
            var beerStyleDtos = await _beerStyleElasticsearch.GetAllAsync(from,size);
            if (beerStyleDtos.Any()) return beerStyleDtos;
            var beerStyles = await _beerStyleRepository.GetAllAsync(from,size);
            return AutoMapper.Mapper.Map<IEnumerable<BeerStyle>, IEnumerable<BeerStyleDto>>(beerStyles);
        }

        public async Task<BeerStyleDto> GetSingleAsync(int id)
        {
             var beerStyleDto = await _beerStyleElasticsearch.GetSingleAsync(id);
            if (beerStyleDto != null) return beerStyleDto;
            var beerStyle = await _beerStyleRepository.GetSingleAsync(id);
            return AutoMapper.Mapper.Map<BeerStyle, BeerStyleDto>(beerStyle);
        }

        public async Task<BeerStyleDto> AddAsync(BeerStyleDto beerStyleDto)
        {
             var beerStyle = AutoMapper.Mapper.Map<BeerStyleDto, BeerStyle>(beerStyleDto);
            await _beerStyleRepository.AddAsync(beerStyle);
            var result = await _beerStyleRepository.GetSingleAsync(beerStyle.BeerStyleId);           
            var mappedResult = AutoMapper.Mapper.Map<BeerStyle, BeerStyleDto>(result);
            _logger.LogInformation(mappedResult.Name);
            await _beerStyleElasticsearch.UpdateAsync(mappedResult);
            await IndexHopAsync(beerStyle);
            return mappedResult;
        }

        public async Task<BeerStyleDto> DeleteAsync(int id)
        {
            var beerStyle = await _beerStyleRepository.GetSingleAsync(id);
            var beerStyleDto = await _beerStyleElasticsearch.GetSingleAsync(id);
            if(beerStyle != null) await _beerStyleRepository.RemoveAsync(beerStyle);
            if (beerStyleDto != null) await _beerStyleElasticsearch.DeleteAsync(id);
            await IndexHopAsync(beerStyle);
            return beerStyleDto ?? AutoMapper.Mapper.Map<BeerStyle, BeerStyleDto>(beerStyle);
        }

        public async Task UpdateAsync(BeerStyleDto beerStyleDto)
        {
             var beerStyle = AutoMapper.Mapper.Map<BeerStyleDto, BeerStyle>(beerStyleDto);
            await _beerStyleRepository.UpdateAsync(beerStyle);
            var result = await _beerStyleRepository.GetSingleAsync(beerStyleDto.Id);
            var mappedResult = AutoMapper.Mapper.Map<BeerStyle, BeerStyleDto>(result);
            await _beerStyleElasticsearch.UpdateAsync(mappedResult);
            await IndexHopAsync(beerStyle);
        }

        public async Task<IEnumerable<BeerStyleDto>> SearchAsync(string query, int @from, int size)
        {
            return await _beerStyleElasticsearch.SearchAsync(query,from,size);
        }

        public async Task ReIndexElasticSearch()
        {
            var beerStyles = await _beerStyleRepository.GetAllAsync(0,int.MaxValue);
            var beerStyleDtos = AutoMapper.Mapper.Map<IList<BeerStyle>, IList<BeerStyleDto>>(beerStyles);
            await _beerStyleElasticsearch.UpdateAllAsync(beerStyleDtos);
        }
        
          private async Task IndexHopAsync(BeerStyle beerStyle)
        {
            if(beerStyle.HopBeerStyles == null) return;
            foreach (var hopBeerStyle in beerStyle.HopBeerStyles)
            {
                var hop = await _hopRepository.GetSingleAsync(hopBeerStyle.HopId);
                var hopDto = AutoMapper.Mapper.Map<Hop, HopDto>(hop);
                await _hopElasticsearch.UpdateAsync(hopDto);
            }
        }
    }
}
