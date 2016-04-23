using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Service.Interface
{
    public interface IBreweryService
    {
        Task<IEnumerable<BreweryDto>> GetAllAsync(int from, int size,bool? isCommerical,string origin, bool? hasBeers);
        Task<BreweryDto> GetSingleAsync(int id);
        Task<BreweryDto> AddAsync(BreweryDto breweryDto);
        Task<BreweryDto> DeleteAsync(int id);
        Task UpdateAsync(BreweryDto breweryDto);
        Task<IEnumerable<BreweryDto>> SearchAsync(string query, int from, int size);
        Task ReIndexElasticSearch();
        Task ReIndexBeerRelationElasticSearch(BeerDto beerDto);
        Task ReIndexUserRelationElasticSearch(UserDto userDto);


        Task<BreweryMemberDto> GetBreweryMember(int breweryId, string username);
        Task<IEnumerable<BreweryMemberDto>> GetAllMembers(int breweryId);
        Task<BreweryMemberDto> DeleteMember(int breweryId, string username);
        Task UpdateBreweryMember(int breweryId, BreweryMemberDto breweryMember);
        Task<BreweryMemberDto> AddBreweryMember(int breweryId, BreweryMemberDto breweryMember);
        Task<IEnumerable<BreweryMember>> GetMembershipsAsync(string username);
    }
}
