using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Service.Interface;
namespace Microbrewit.Api.Service.Component
{
    public class GlassService : IGlassService 
        {
        private IGlassRepository _glassRepository;
        
        public GlassService(IGlassRepository GlassRepository)
        {
            _glassRepository = GlassRepository;
        }
        public async Task<GlassDto> AddAsync(GlassDto GlassDto)
        {
            var glass = AutoMapper.Mapper.Map<GlassDto,Glass>(GlassDto);
            await _glassRepository.AddAsync(glass);
            return AutoMapper.Mapper.Map<Glass,GlassDto>(glass);
        }

        public async Task<GlassDto> DeleteAsync(int id)
        {
            var glass = await _glassRepository.GetSingleAsync(id);
            await _glassRepository.RemoveAsync(glass);
            return AutoMapper.Mapper.Map<Glass,GlassDto>(glass);
        }

        public async Task<IEnumerable<GlassDto>> GetAllAsync()
        {
            var glasses = await _glassRepository.GetAllAsync();
            return AutoMapper.Mapper.Map<IEnumerable<Glass>,IEnumerable<GlassDto>>(glasses);
        }

        public async Task<GlassDto> GetSingleAsync(int id)
        {
            var glass = await _glassRepository.GetSingleAsync(id);
            return AutoMapper.Mapper.Map<Glass,GlassDto>(glass);
        }

        public Task ReIndexElasticSearch()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GlassDto>> SearchAsync(string query, int from, int size)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(GlassDto GlassDto)
        {
            var Glass = AutoMapper.Mapper.Map<GlassDto,Glass>(GlassDto);
            await _glassRepository.UpdateAsync(Glass);
        }
    }
}