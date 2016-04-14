using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microbrewit.Api.ElasticSearch.Interface;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Service.Interface;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Model.Database;

namespace Microbrewit.Api.Service.Component
{
    public class OtherService : IOtherService
    {
        private readonly IOtherRepository _otherRepository;
        private readonly IOtherElasticsearch _otherElasticsearch;
        public OtherService(IOtherRepository otherRepository, IOtherElasticsearch otherElasticsearch)
        {
            _otherRepository = otherRepository;
            _otherElasticsearch = otherElasticsearch;
        }


        public async Task<OtherDto> AddAsync(OtherDto otherDto)
        {
            var other = AutoMapper.Mapper.Map<OtherDto, Other>(otherDto);
            await _otherRepository.AddAsync(other);
            var result = await _otherRepository.GetSingleAsync(other.OtherId);
            var mappedResult = AutoMapper.Mapper.Map<Other, OtherDto>(result);
            await _otherElasticsearch.UpdateAsync(mappedResult);
            return mappedResult;
        }

        public async Task<OtherDto> DeleteAsync(int id)
        {
            var otherDto = await _otherElasticsearch.GetSingleAsync(id);
            if (otherDto != null) return otherDto;
            var other = await _otherRepository.GetSingleAsync(id);
            return AutoMapper.Mapper.Map<Other,OtherDto>(other);
        }

        public async Task<IEnumerable<OtherDto>> GetAllAsync(string custom)
        {
            var othersDto = await _otherElasticsearch.GetAllAsync("custom");
            if (othersDto.Any()) return othersDto;
            var others = await _otherRepository.GetAllAsync();
            return AutoMapper.Mapper.Map<IEnumerable<Other>,IEnumerable<OtherDto>>(others);
        }

        public async Task<OtherDto> GetSingleAsync(int id)
        {
            var otherDto = await _otherElasticsearch.GetSingleAsync(id);
            if (otherDto != null) return otherDto;
            var other = await _otherRepository.GetSingleAsync(id);
            return AutoMapper.Mapper.Map<Other,OtherDto>(other);
        }

        public async Task ReIndexElasticSearch()
        {
            var others = await _otherRepository.GetAllAsync();
            var otherDtos = AutoMapper.Mapper.Map<IEnumerable<Other>, IEnumerable<OtherDto>>(others);
            await _otherElasticsearch.UpdateAllAsync(otherDtos);
        }

        public async Task<IEnumerable<OtherDto>> SearchAsync(string query, int from, int size)
        {
            return await _otherElasticsearch.SearchAsync(query, from, size);
        }

        public async Task UpdateAsync(OtherDto otherDto)
        {
            var other = AutoMapper.Mapper.Map<OtherDto, Other>(otherDto);
            await _otherRepository.UpdateAsync(other);
            var result = await _otherRepository.GetSingleAsync(otherDto.Id);
            var mappedResult = AutoMapper.Mapper.Map<Other, OtherDto>(result);
            await _otherElasticsearch.UpdateAsync(mappedResult);
        }
    }
}