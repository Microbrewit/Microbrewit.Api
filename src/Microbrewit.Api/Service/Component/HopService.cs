using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.ElasticSearch.Interface;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Service.Interface;
namespace Microbrewit.Api.Service.Component
{
    public class HopService : IHopService
    {
        private readonly IHopElasticsearch _hopElasticsearch;
        private readonly IHopRepository _hopRepository;
        private readonly IBeerStyleRepository _beerStyleRepository;
        private readonly IBeerStyleElasticsearch _beerStyleElasticsearch;
        
        public HopService(IHopRepository hopRepository,IHopElasticsearch hopElasticsearch,IBeerStyleRepository beerStyleRespository, 
        IBeerStyleElasticsearch beerStyleElasticsearch)
        {
            _beerStyleElasticsearch = beerStyleElasticsearch;
            _beerStyleRepository = beerStyleRespository;
            _hopElasticsearch = hopElasticsearch;
            _hopRepository = hopRepository;
        }

        public async Task<HopDto> AddAsync(HopDto hopDto)
        {
             var hop = AutoMapper.Mapper.Map<HopDto,Hop>(hopDto);
            await _hopRepository.AddAsync(hop);
            var result = await _hopRepository.GetSingleAsync(hop.HopId);
            var mappedResult = AutoMapper.Mapper.Map<Hop, HopDto>(result);
            await _hopElasticsearch.UpdateAsync(mappedResult);
            await IndexBeerStylesAsync(hop);
            return mappedResult;
        }

        public async Task<HopDto> DeleteAsync(int id)
        {
            var hop = await _hopRepository.GetSingleAsync(id);
            var hopDto = await _hopElasticsearch.GetSingleAsync(id);
            if(hop != null) await _hopRepository.RemoveAsync(hop);
            if (hopDto != null) await _hopElasticsearch.DeleteAsync(id);
            await IndexBeerStylesAsync(hop);
            return hopDto;
        }

        public async Task<IEnumerable<HopDto>> GetAllAsync(int from, int size)
        {
            var hopsDto = await _hopElasticsearch.GetAllAsync(from, size);
            if (hopsDto.Any()) return hopsDto;
            var hops = await _hopRepository.GetAllAsync(from, size);
            hopsDto = AutoMapper.Mapper.Map<IEnumerable<Hop>, IList<HopDto>>(hops);
            return hopsDto;
        }

        public async Task<IEnumerable<DTO>> GetHopFromsAsync()
        {
            var hopForms = await _hopRepository.GetHopFormsAsync();
            return AutoMapper.Mapper.Map<IEnumerable<HopForm>,IEnumerable<DTO>>(hopForms);
        }

        public async Task<HopDto> GetSingleAsync(int id)
        {
             var hopDto = await _hopElasticsearch.GetSingleAsync(id);
            if (hopDto != null) return hopDto;
            var hop = await _hopRepository.GetSingleAsync(id);
            hopDto = AutoMapper.Mapper.Map<Hop, HopDto>(hop);
            return hopDto;
        }

        public async Task ReIndexHopsElasticSearch()
        {
            var hops = await _hopRepository.GetAllAsync(0,10000);
            var hopsDto = AutoMapper.Mapper.Map<IEnumerable<Hop>, IList<HopDto>>(hops);
            await _hopElasticsearch.UpdateAllAsync(hopsDto);
        }

        public async Task<IEnumerable<HopDto>> SearchHop(string query, int from, int size)
        {
             return await _hopElasticsearch.SearchAsync(query, from, size);
        }

        public async Task UpdateHopAsync(HopDto hopDto)
        {
            var hop = AutoMapper.Mapper.Map<HopDto, Hop>(hopDto);
            await _hopRepository.UpdateAsync(hop);
            var result = await _hopRepository.GetSingleAsync(hopDto.Id);
            var mappedResult = AutoMapper.Mapper.Map<Hop, HopDto>(result);
            await _hopElasticsearch.UpdateAsync(mappedResult);
            await IndexBeerStylesAsync(hop);
        }
        
          private async Task IndexBeerStylesAsync(Hop hop)
        {
            foreach (var hopBeerStyle in hop.HopBeerStyles)
            {
                var beerStyle = await _beerStyleRepository.GetSingleAsync(hopBeerStyle.BeerStyleId);
                await _beerStyleElasticsearch.UpdateAsync(AutoMapper.Mapper.Map<BeerStyle, BeerStyleDto>(beerStyle));
            }
        }
    }
}