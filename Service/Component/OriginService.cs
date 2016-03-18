using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Service.Interface;
namespace Microbrewit.Api.Service.Component
{
    public class OriginService : IOriginService
    {
        private IOriginRespository _originRepository;
        
        public OriginService(IOriginRespository originRepository)
        {
            _originRepository = originRepository;
        }
        public async Task<OriginDto> AddAsync(OriginDto originDto)
        {
            var origin = AutoMapper.Mapper.Map<OriginDto,Origin>(originDto);
            await _originRepository.AddAsync(origin);
            return AutoMapper.Mapper.Map<Origin,OriginDto>(origin);
        }

        public async Task<OriginDto> DeleteAsync(int id)
        {
            var origin = await _originRepository.GetSingleAsync(id);
            await _originRepository.RemoveAsync(origin);
            return AutoMapper.Mapper.Map<Origin,OriginDto>(origin);
        }

        public async Task<IEnumerable<OriginDto>> GetAllAsync(int from, int size, string custom)
        {
            var origins = await _originRepository.GetAllAsync(from,size);
            return AutoMapper.Mapper.Map<IEnumerable<Origin>,IEnumerable<OriginDto>>(origins);
        }

        public async Task<OriginDto> GetSingleAsync(int id)
        {
            var origin = await _originRepository.GetSingleAsync(id);
            return AutoMapper.Mapper.Map<Origin,OriginDto>(origin);
        }

        public Task ReIndexElasticSearch()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OriginDto>> SearchAsync(string query, int from, int size)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(OriginDto originDto)
        {
            var origin = AutoMapper.Mapper.Map<OriginDto,Origin>(originDto);
            await _originRepository.UpdateAsync(origin);
        }
    }
}