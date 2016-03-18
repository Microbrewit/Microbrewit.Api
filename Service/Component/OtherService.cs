using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Service.Interface;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Model.Database;

namespace Microbrewit.Api.Service.Component
{
    public class OtherService : IOtherService
    {
        private IOtherRepository _otherRepository;
        public OtherService(IOtherRepository otherRepository)
        {
            _otherRepository = otherRepository;    
        }


        public async Task<OtherDto> AddAsync(OtherDto otherDto)
        {
            var other = AutoMapper.Mapper.Map<OtherDto,Other>(otherDto);
            await _otherRepository.AddAsync(other);
            return AutoMapper.Mapper.Map<Other,OtherDto>(other);
        }

        public async Task<OtherDto> DeleteAsync(int id)
        {
            var other = await _otherRepository.GetSingleAsync(id);
            await _otherRepository.RemoveAsync(other);
            return AutoMapper.Mapper.Map<Other,OtherDto>(other);
        }

        public async Task<IEnumerable<OtherDto>> GetAllAsync(string custom)
        {
            var others = await _otherRepository.GetAllAsync();
            return AutoMapper.Mapper.Map<IEnumerable<Other>,IEnumerable<OtherDto>>(others);
        }

        public async Task<OtherDto> GetSingleAsync(int id)
        {
            var other = await _otherRepository.GetSingleAsync(id);
            return AutoMapper.Mapper.Map<Other,OtherDto>(other);
        }

        public Task ReIndexElasticSearch()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OtherDto>> SearchAsync(string query, int from, int size)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(OtherDto otherDto)
        {
            var other = AutoMapper.Mapper.Map<OtherDto,Other>(otherDto);
            await _otherRepository.UpdateAsync(other);
        }
    }
}