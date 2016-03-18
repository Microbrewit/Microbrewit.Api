using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Service.Interface;
namespace Microbrewit.Api.Service.Component
{
    public class HopService : IHopService
    {
        private readonly IHopRepository _hopRepository;
        
        public HopService(IHopRepository hopRepository)
        {
            _hopRepository = hopRepository;
        }

        public async Task<HopDto> AddAsync(HopDto hopDto)
        {
            var hop = AutoMapper.Mapper.Map<HopDto, Hop>(hopDto);
            await _hopRepository.AddAsync(hop);
            return AutoMapper.Mapper.Map<Hop,HopDto>(await _hopRepository.GetSingleAsync(hop.HopId));
        }

        public async Task<HopDto> AddHopAsync(HopDto hopDto)
        {
            var hop =  AutoMapper.Mapper.Map<HopDto,Hop>(hopDto);
            await _hopRepository.AddAsync(hop);
            return AutoMapper.Mapper.Map<Hop,HopDto>(await _hopRepository.GetSingleAsync(hop.HopId));
        }

        public async Task<HopDto> DeleteAsync(int id)
        {
            var hop = await _hopRepository.GetSingleAsync(id);
            if (hop == null) return null;
            await _hopRepository.RemoveAsync(hop);
            return AutoMapper.Mapper.Map<Hop, HopDto>(hop);
        }

        public async Task<HopDto> DeleteHopAsync(int id)
        {
            var hop = await _hopRepository.GetSingleAsync(id);
            await _hopRepository.RemoveAsync(hop);
            return AutoMapper.Mapper.Map<Hop,HopDto>(hop);
        }

        public async Task<IEnumerable<HopDto>> GetAllAsync(int from, int size)
        {
            var hops = await _hopRepository.GetAllAsync(from, size);
            return AutoMapper.Mapper.Map<IEnumerable<Hop>,IEnumerable<HopDto>>(hops);
        }

        public async Task<IEnumerable<DTO>> GetHopFromsAsync()
        {
            var hopForms = await _hopRepository.GetHopFormsAsync();
            return AutoMapper.Mapper.Map<IEnumerable<HopForm>,IEnumerable<DTO>>(hopForms);
        }

        public async Task<HopDto> GetSingleAsync(int id)
        {
            var hop = await _hopRepository.GetSingleAsync(id);
            return AutoMapper.Mapper.Map<Hop,HopDto>(hop);
        }

        public Task ReIndexHopsElasticSearch()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<HopDto>> SearchHop(string query, int from, int size)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateHopAsync(HopDto hopDto)
        {
            var hop = AutoMapper.Mapper.Map<HopDto,Hop>(hopDto);
            await _hopRepository.UpdateAsync(hop);
        }
    }
}