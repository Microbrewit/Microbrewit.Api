using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Service.Interface;
using Microbrewit.Api.ElasticSearch.Interface;
using Microsoft.Extensions.Logging;

namespace Microbrewit.Api.Service.Component
{
    public class YeastService : IYeastService
    {
        private ILogger<YeastService> _logger;
        private readonly IYeastRepository _yeastRepository;
        private readonly IYeastElasticsearch _yeastElasticsearch;
        
        public YeastService(IYeastRepository yeastRepository, IYeastElasticsearch yeastElasticsearch, ILogger<YeastService> logger)
        {
            _logger = logger;
            _yeastRepository = yeastRepository;
            _yeastElasticsearch = yeastElasticsearch;
        }
        public async Task<YeastDto> AddAsync(YeastDto yeastDto)
        {
            var yeast = AutoMapper.Mapper.Map<YeastDto, Yeast>(yeastDto);
            await _yeastRepository.AddAsync(yeast);
            _logger.LogInformation($"YeastId: {yeast.YeastId}");
            var result = await _yeastRepository.GetSingleAsync(yeast.YeastId);
            var mappedResult = AutoMapper.Mapper.Map<Yeast,YeastDto>(result);
            await _yeastElasticsearch.UpdateAsync(mappedResult);
            return mappedResult;
        }

        public async Task<YeastDto> DeleteAsync(int id)
        {
            var yeast = await _yeastRepository.GetSingleAsync(id);
            var yeastDto = await _yeastElasticsearch.GetSingleAsync(id);
            if (yeast != null) await _yeastRepository.RemoveAsync(yeast);
            if (yeastDto != null) await _yeastElasticsearch.DeleteAsync(id);
            return yeastDto ?? AutoMapper.Mapper.Map<Yeast,YeastDto>(yeast);
        }


        public async Task<IEnumerable<YeastDto>> GetAllAsync(string custom)
        {
            var yeastsDto = await _yeastElasticsearch.GetAllAsync(custom);
            //if (yeastsDto.Any()) 
            return yeastsDto ;
            //var yeasts = await _yeastRepository.GetAllAsync();
            //return AutoMapper.Mapper.Map<IEnumerable<Yeast>,IEnumerable<YeastDto>>(yeasts);
        }

        public async Task<YeastDto> GetSingleAsync(int id)
        {
            var yeastDto = await _yeastElasticsearch.GetSingleAsync(id);
            if (yeastDto != null) return yeastDto;
            var yeast = await _yeastRepository.GetSingleAsync(id);
            return AutoMapper.Mapper.Map<Yeast,YeastDto>(yeast);
        }

        public async Task ReIndexElasticSearch()
        {
            var yeasts = await _yeastRepository.GetAllAsync();
            var yeastsDto = AutoMapper.Mapper.Map<IEnumerable<Yeast>, IEnumerable<YeastDto>>(yeasts);
            await _yeastElasticsearch.UpdateAllAsync(yeastsDto);
        }

        public async Task UpdateAsync(YeastDto yeastDto)
        {
             var yeast = AutoMapper.Mapper.Map<YeastDto, Yeast>(yeastDto);
            await _yeastRepository.UpdateAsync(yeast);
            var result = await _yeastRepository.GetSingleAsync(yeastDto.Id);
            var mappedResult = AutoMapper.Mapper.Map<Yeast, YeastDto>(result);
            await _yeastElasticsearch.UpdateAsync(mappedResult);
        }

        public async Task<IEnumerable<YeastDto>> SearchAsync(string query, int from, int size)
        {
             return await _yeastElasticsearch.SearchAsync(query, from, size);
        }
    }
}