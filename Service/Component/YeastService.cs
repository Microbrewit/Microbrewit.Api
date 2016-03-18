using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Service.Interface;
namespace Microbrewit.Api.Service.Component
{
    public class YeastService : IYeastService
    {
        private IYeastRepository _yeastRepository;
        
        public YeastService(IYeastRepository yeastRepository)
        {
            _yeastRepository = yeastRepository;
        }
        public async Task<YeastDto> AddAsync(YeastDto yeastDto)
        {
            var yeast = AutoMapper.Mapper.Map<YeastDto,Yeast>(yeastDto);
            await _yeastRepository.AddAsync(yeast);
            return AutoMapper.Mapper.Map<Yeast,YeastDto>(yeast);
        }

        public async Task<YeastDto> DeleteAsync(int id)
        {
            var origin = await _yeastRepository.GetSingleAsync(id);
            await _yeastRepository.RemoveAsync(origin);
            return AutoMapper.Mapper.Map<Yeast,YeastDto>(origin);
        }


        public async Task<IEnumerable<YeastDto>> GetAllAsync(string custom)
        {
            var yeasts = await _yeastRepository.GetAllAsync();
            return AutoMapper.Mapper.Map<IEnumerable<Yeast>,IEnumerable<YeastDto>>(yeasts);
        }

        public async Task<YeastDto> GetSingleAsync(int id)
        {
            var yeast = await _yeastRepository.GetSingleAsync(id);
            return AutoMapper.Mapper.Map<Yeast,YeastDto>(yeast);
        }

        public Task ReIndexElasticSearch()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OriginDto>> SearchAsync(string query, int from, int size)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(YeastDto yeastDto)
        {
            var yeast = AutoMapper.Mapper.Map<YeastDto,Yeast>(yeastDto);
            await _yeastRepository.UpdateAsync(yeast);
        }

        Task<IEnumerable<YeastDto>> IYeastService.SearchAsync(string query, int from, int size)
        {
            throw new NotImplementedException();
        }
    }
}