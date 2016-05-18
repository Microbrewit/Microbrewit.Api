using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Elasticsearch.Interface;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Service.Interface;

namespace Microbrewit.Api.Service.Component
{
    public class OriginService : IOriginService
    {
        private readonly IOriginElasticsearch _originElasticsearch;
        private readonly IOriginRespository _originRepository;
        
        public OriginService(IOriginRespository originRepository, IOriginElasticsearch originElasticsearch)
        {
            _originElasticsearch = originElasticsearch;
            _originRepository = originRepository;
        }
        public async Task<OriginDto> AddAsync(OriginDto originDto)
        {
             var origin = AutoMapper.Mapper.Map<OriginDto, Origin>(originDto);
            await _originRepository.AddAsync(origin);
            var result = await _originRepository.GetSingleAsync(origin.OriginId);
            var mappedResult = AutoMapper.Mapper.Map<Origin, OriginDto>(result);
            await _originElasticsearch.UpdateAsync(mappedResult);
            return mappedResult;
        }

        public async Task<OriginDto> DeleteAsync(int id)
        {
           var origin = await _originRepository.GetSingleAsync(id);
            var originDto = await _originElasticsearch.GetSingleAsync(id);
            if(origin != null) await _originRepository.RemoveAsync(origin);
            if (originDto != null) await _originElasticsearch.DeleteAsync(id);
            return originDto;
        }

        public async Task<IEnumerable<OriginDto>> GetAllAsync(int from, int size, string custom)
        {
             var originDtos = await _originElasticsearch.GetAllAsync(from,size,custom);
            if (originDtos.Any()) return originDtos;
            var origins = await _originRepository.GetAllAsync(from,size);
            return AutoMapper.Mapper.Map<IEnumerable<Origin>, IEnumerable<OriginDto>>(origins);
        }

        public async Task<OriginDto> GetSingleAsync(int id)
        {
            var originDto = await _originElasticsearch.GetSingleAsync(id);
            if (originDto != null) return originDto;
            var origin = await _originRepository.GetSingleAsync(id);
            return AutoMapper.Mapper.Map<Origin, OriginDto>(origin);
        }

        public async Task ReIndexElasticSearch()
        {
            var origins = await _originRepository.GetAllAsync(0,int.MaxValue);
            var originDtos =AutoMapper.Mapper.Map<IEnumerable<Origin>, IList<OriginDto>>(origins);
            await _originElasticsearch.UpdateAllAsync(originDtos);
        }

        public async Task<IEnumerable<OriginDto>> SearchAsync(string query, int from, int size)
        {
            return await _originElasticsearch.SearchAsync(query,from,size);
        }

        public async Task UpdateAsync(OriginDto originDto)
        {
               var origin = AutoMapper.Mapper.Map<OriginDto, Origin>(originDto);
            await _originRepository.UpdateAsync(origin);
            var result = await _originRepository.GetSingleAsync(originDto.Id);
            var mappedResult = AutoMapper.Mapper.Map<Origin, OriginDto>(result);
            await _originElasticsearch.UpdateAsync(mappedResult);
        }
    }
}