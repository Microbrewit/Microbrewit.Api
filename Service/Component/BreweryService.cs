using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microbrewit.Api.ElasticSearch.Interface;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Service.Interface;

namespace Microbrewit.Api.Service.Component
{
    public class BreweryService : IBreweryService
    {
        private readonly IBreweryRepository _breweryRepository;
        private static IBreweryElasticsearch _breweryElasticsearch;


        public BreweryService(IBreweryRepository breweryRepository, IBreweryElasticsearch breweryElasticsearch)
        {
            _breweryElasticsearch = breweryElasticsearch;
            _breweryRepository = breweryRepository;
        }

        public async Task<IEnumerable<BreweryDto>> GetAllAsync(int @from, int size)
        {
             var brewerysDto = await _breweryElasticsearch.GetAllAsync(from,size);
            if (brewerysDto .Any()) return brewerysDto ;
            var brewerys = await _breweryRepository.GetAllAsync(from, size);
            brewerysDto = AutoMapper.Mapper.Map<IEnumerable<Brewery>, IEnumerable<BreweryDto>>(brewerys);
            return brewerysDto;
        }

        public async Task<BreweryDto> GetSingleAsync(int id)
        {
           var breweryDto = await _breweryElasticsearch.GetSingleAsync(id);
            if (breweryDto != null) return breweryDto;
            var brewery = await _breweryRepository.GetSingleAsync(id);
            breweryDto = AutoMapper.Mapper.Map<Brewery, BreweryDto>(brewery);
            return breweryDto;
        }

        public async Task<BreweryDto> AddAsync(BreweryDto breweryDto)
        {
             var brewery = AutoMapper.Mapper.Map<BreweryDto, Brewery>(breweryDto);
            await _breweryRepository.AddAsync(brewery);
            var result = await _breweryRepository.GetSingleAsync(brewery.BreweryId);
            var mappedResult = AutoMapper.Mapper.Map<Brewery,BreweryDto>(result);
            await _breweryElasticsearch.UpdateAsync(mappedResult);
            return mappedResult;
        }

        public async Task<BreweryDto> DeleteAsync(int id)
        {
            var brewery = await _breweryRepository.GetSingleAsync(id);
            var breweryDto = await _breweryElasticsearch.GetSingleAsync(id);
            if (brewery != null) await _breweryRepository.RemoveAsync(brewery);
            if (breweryDto == null) return breweryDto;
            await _breweryElasticsearch.DeleteAsync(id);
            //if(breweryDto.Members.Any()) await _userService.ReIndexBreweryRelationElasticSearch(breweryDto);
            return breweryDto ?? AutoMapper.Mapper.Map<Brewery,BreweryDto>(brewery);
        }

        public async Task UpdateAsync(BreweryDto breweryDto)
        {
            var brewery = AutoMapper.Mapper.Map<BreweryDto, Brewery>(breweryDto);
            await _breweryRepository.UpdateAsync(brewery);
            var result = await _breweryRepository.GetSingleAsync(breweryDto.Id);
            var mappedResult = AutoMapper.Mapper.Map<Brewery, BreweryDto>(result);
            //if (brewery.Members.Any()) await _userService.ReIndexBreweryRelationElasticSearch(mappedResult);
            await _breweryElasticsearch.UpdateAsync(mappedResult);
        }

        public async Task<IEnumerable<BreweryDto>> SearchAsync(string query, int @from, int size)
        {
            return await _breweryElasticsearch.SearchAsync(query, from, size);
        }

        public async Task ReIndexElasticSearch()
        {
             var brewerys = await _breweryRepository.GetAllAsync(0,int.MaxValue);
            var brewerysDto = AutoMapper.Mapper.Map<IEnumerable<Brewery>, IEnumerable<BreweryDto>>(brewerys);
            await _breweryElasticsearch.UpdateAllAsync(brewerysDto);
        }

        public async Task ReIndexBeerRelationElasticSearch(BeerDto beerDto)
        {
            foreach (var dtoBrewery in beerDto.Breweries)
            {
                var brewery = dtoBrewery;
                var result = await _breweryRepository.GetSingleAsync(brewery.Id);
                var mappedResult = AutoMapper.Mapper.Map<Brewery, BreweryDto>(result);
                await _breweryElasticsearch.UpdateAsync(mappedResult);
            }
        }

        public async Task ReIndexUserRelationElasticSearch(UserDto userDto)
        {
             foreach (var breweryDto in userDto.Breweries)
            {
                var brewery = breweryDto;
                var result = await _breweryRepository.GetSingleAsync(brewery.Id);
                var mappedResult = AutoMapper.Mapper.Map<Brewery, BreweryDto>(result);
                await _breweryElasticsearch.UpdateAsync(mappedResult);
            }
        }

        public async Task<BreweryMemberDto> GetBreweryMember(int breweryId, string username)
        {
            var breweryMember = await _breweryRepository.GetSingleMemberAsync(breweryId, username);
            return AutoMapper.Mapper.Map<BreweryMember, BreweryMemberDto>(breweryMember);
        }

        public async Task<IEnumerable<BreweryMemberDto>> GetAllMembers(int breweryId)
        {
             var breweryMembersDto = await _breweryElasticsearch.GetAllMembersAsync(breweryId);
            if (breweryMembersDto.Any()) return breweryMembersDto;
            var breweryMembers = await _breweryRepository.GetAllMembersAsync(breweryId);
            return AutoMapper.Mapper.Map<IEnumerable<BreweryMember>, IEnumerable<BreweryMemberDto>>(breweryMembers);
        }

        public async Task<BreweryMemberDto> DeleteMember(int breweryId, string username)
        {
            var breweryMemberDto = await _breweryElasticsearch.GetSingleMemberAsync(breweryId, username);
            await _breweryRepository.DeleteMemberAsync(breweryId,username);
            var brewery = await _breweryRepository.GetSingleAsync(breweryId);
            var breweryDto = AutoMapper.Mapper.Map<Brewery, BreweryDto>(brewery);
            await _breweryElasticsearch.UpdateAsync(breweryDto);
            //await _userService.ReIndexUserElasticSearch(username);
            return breweryMemberDto;
        }

        public async Task UpdateBreweryMember(int breweryId, BreweryMemberDto breweryMemberDto)
        {
            var breweryMember = AutoMapper.Mapper.Map<BreweryMemberDto, BreweryMember>(breweryMemberDto);
            breweryMember.BreweryId = breweryId;
            await _breweryRepository.UpdateMemberAsync(breweryMember);
            var brewery = await _breweryRepository.GetSingleAsync(breweryId);
            var breweryDto = AutoMapper.Mapper.Map<Brewery, BreweryDto>(brewery);
            await _breweryElasticsearch.UpdateAsync(breweryDto);
            //await _userService.ReIndexUserElasticSearch(breweryMemberDto.Username);
        }

        public async Task<BreweryMemberDto> AddBreweryMember(int breweryId, BreweryMemberDto breweryMemberDto)
        {
             var breweryMember = AutoMapper.Mapper.Map<BreweryMemberDto, BreweryMember>(breweryMemberDto);
            breweryMember.BreweryId = breweryId;
            await _breweryRepository.AddMemberAsync(breweryMember);
            var brewery = await _breweryRepository.GetSingleAsync(breweryId);
            var breweryDto = AutoMapper.Mapper.Map<Brewery, BreweryDto>(brewery);
            await _breweryElasticsearch.UpdateAsync(breweryDto);
            //await _userService.ReIndexUserElasticSearch(breweryMemberDto.Username);
            return breweryDto.Members.SingleOrDefault(b => b.Username.Equals(breweryMemberDto.Username));
        }

        public async Task<IEnumerable<BreweryMember>> GetMembershipsAsync(string username)
        {
            var breweryMembers = await _breweryRepository.GetMembershipsAsync(username);
            return breweryMembers;
        }
    }
}
