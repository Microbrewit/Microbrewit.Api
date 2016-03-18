using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Service.Interface;

namespace Microbrewit.Api.Service.Component
{
    public class BreweryService : IBreweryService
    {
        private readonly IBreweryRepository _breweryRepository;


        public BreweryService(IBreweryRepository breweryRepository)
        {
            _breweryRepository = breweryRepository;
        }

        public async Task<IEnumerable<BreweryDto>> GetAllAsync(int @from, int size)
        {
            var brewerys = await _breweryRepository.GetAllAsync(from, size);
            var brewerysDto = AutoMapper.Mapper.Map<IEnumerable<Brewery>, IEnumerable<BreweryDto>>(brewerys);
            return brewerysDto;
        }

        public async Task<BreweryDto> GetSingleAsync(int id)
        {
            var brewery = await _breweryRepository.GetSingleAsync(id);
            var breweryDto = AutoMapper.Mapper.Map<Brewery, BreweryDto>(brewery);
            return breweryDto;
        }

        public async Task<BreweryDto> AddAsync(BreweryDto breweryDto)
        {
            var brewery = AutoMapper.Mapper.Map<BreweryDto, Brewery>(breweryDto);
            await _breweryRepository.AddAsync(brewery);
            var result = await _breweryRepository.GetSingleAsync(brewery.BreweryId);
            var mappedResult = AutoMapper.Mapper.Map<Brewery, BreweryDto>(result);
            return mappedResult;
        }

        public async Task<BreweryDto> DeleteAsync(int id)
        {
            var brewery = await _breweryRepository.GetSingleAsync(id);
            if (brewery != null) await _breweryRepository.RemoveAsync(brewery);
            return AutoMapper.Mapper.Map<Brewery, BreweryDto>(brewery);
        }

        public async Task UpdateAsync(BreweryDto breweryDto)
        {
            var brewery = AutoMapper.Mapper.Map<BreweryDto, Brewery>(breweryDto);
            await _breweryRepository.UpdateAsync(brewery);
            await _breweryRepository.GetSingleAsync(breweryDto.Id);
        }

        public async Task<IEnumerable<BreweryDto>> SearchAsync(string query, int @from, int size)
        {
            throw new NotImplementedException();
        }

        public async Task ReIndexElasticSearch()
        {
            throw new NotImplementedException();
        }

        public async Task ReIndexBeerRelationElasticSearch(BeerDto beerDto)
        {
            throw new NotImplementedException();
        }

        public async Task ReIndexUserRelationElasticSearch(UserDto userDto)
        {
            throw new NotImplementedException();
        }

        public async Task<BreweryMemberDto> GetBreweryMember(int breweryId, string username)
        {
            var breweryMember = await _breweryRepository.GetSingleMemberAsync(breweryId, username);
            return AutoMapper.Mapper.Map<BreweryMember, BreweryMemberDto>(breweryMember);
        }

        public async Task<IEnumerable<BreweryMemberDto>> GetAllMembers(int breweryId)
        {
            var breweryMembers = await _breweryRepository.GetAllMembersAsync(breweryId);
            return AutoMapper.Mapper.Map<IEnumerable<BreweryMember>, IEnumerable<BreweryMemberDto>>(breweryMembers);
        }

        public async Task<BreweryMemberDto> DeleteMember(int breweryId, string username)
        {
            var brewyMember = await _breweryRepository.GetSingleMemberAsync(breweryId, username);
            await _breweryRepository.DeleteMemberAsync(breweryId, username);
            return AutoMapper.Mapper.Map<BreweryMember,BreweryMemberDto>(brewyMember);
        }

        public async Task UpdateBreweryMember(int breweryId, BreweryMemberDto breweryMemberDto)
        {
            var breweryMember = AutoMapper.Mapper.Map<BreweryMemberDto, BreweryMember>(breweryMemberDto);
            breweryMember.BreweryId = breweryId;
            await _breweryRepository.UpdateMemberAsync(breweryMember);
        }

        public async Task<BreweryMemberDto> AddBreweryMember(int breweryId, BreweryMemberDto breweryMemberDto)
        {
            var breweryMember = AutoMapper.Mapper.Map<BreweryMemberDto, BreweryMember>(breweryMemberDto);
            breweryMember.BreweryId = breweryId;
            await _breweryRepository.AddMemberAsync(breweryMember);
            var brewery = await _breweryRepository.GetSingleAsync(breweryId);
            var breweryDto = AutoMapper.Mapper.Map<Brewery, BreweryDto>(brewery);
            return breweryDto.Members.SingleOrDefault(b => b.Username.Equals(breweryMemberDto.Username));
        }

        public async Task<IEnumerable<BreweryMember>> GetMembershipsAsync(string username)
        {
            return await _breweryRepository.GetMembershipsAsync(username);
        }
    }
}
