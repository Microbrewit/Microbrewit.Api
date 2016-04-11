using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.ElasticSearch.Interface;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using System.Linq;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Service.Interface;
namespace Microbrewit.Api.Service.Component
{
    public class GlassService : IGlassService 
        {
        private readonly IGlassElasticsearch _glassElasticsearch;
        private readonly IGlassRepository _glassRepository;
        
        public GlassService(IGlassRepository glassRepository,IGlassElasticsearch glassElasticsearch)
        {
            _glassElasticsearch = glassElasticsearch;
            _glassRepository = glassRepository;
        }
        public async Task<GlassDto> AddAsync(GlassDto glassDto)
        {
            var glass = AutoMapper.Mapper.Map<GlassDto, Glass>(glassDto);
            await _glassRepository.AddAsync(glass);
            var result = await _glassRepository.GetSingleAsync(glass.GlassId);
            var mappedResult = AutoMapper.Mapper.Map<Glass, GlassDto>(result);
            await _glassElasticsearch.UpdateAsync(mappedResult);
            return mappedResult;
        }

        public async Task<GlassDto> DeleteAsync(int id)
        {
            var glass = await _glassRepository.GetSingleAsync(id);
            var glassDto = await _glassElasticsearch.GetSingleAsync(id);
            if(glass != null) await _glassRepository.RemoveAsync(glass);
            if (glassDto != null) await _glassElasticsearch.DeleteAsync(id);
            return glassDto ?? AutoMapper.Mapper.Map<Glass,GlassDto>(glass);
        }

        public async Task<IEnumerable<GlassDto>> GetAllAsync()
        {
            var glassDtos = await _glassElasticsearch.GetAllAsync();
            if (glassDtos.Any()) return glassDtos;
            var glasss = await _glassRepository.GetAllAsync();
            return AutoMapper.Mapper.Map<IEnumerable<Glass>, IEnumerable<GlassDto>>(glasss);
        }

        public async Task<GlassDto> GetSingleAsync(int id)
        {
            var glassDto = await _glassElasticsearch.GetSingleAsync(id);
            if (glassDto != null) return glassDto;
            var glass = await _glassRepository.GetSingleAsync(id);
            return AutoMapper.Mapper.Map<Glass, GlassDto>(glass);
        }

        public async Task ReIndexElasticSearch()
        {
            var glasss = await _glassRepository.GetAllAsync();
            var glassDtos = AutoMapper.Mapper.Map<IEnumerable<Glass>, IList<GlassDto>>(glasss);
            await _glassElasticsearch.UpdateAllAsync(glassDtos);
        }

        public async Task<IEnumerable<GlassDto>> SearchAsync(string query, int from, int size)
        {
            return await _glassElasticsearch.SearchAsync(query,from,size);
        }

        public async Task UpdateAsync(GlassDto glassDto)
        {
             var glass =AutoMapper.Mapper.Map<GlassDto, Glass>(glassDto);
            await _glassRepository.UpdateAsync(glass);
            var result = await _glassRepository.GetSingleAsync(glassDto.Id);
            var mappedResult = AutoMapper.Mapper.Map<Glass, GlassDto>(result);
            await _glassElasticsearch.UpdateAsync(mappedResult);
        }
    }
}