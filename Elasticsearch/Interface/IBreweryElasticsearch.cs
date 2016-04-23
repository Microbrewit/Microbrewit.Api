using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.ElasticSearch.Interface
{
    public interface IBreweryElasticsearch
    {
        Task UpdateAsync(BreweryDto breweryDto);

        Task<IEnumerable<BreweryDto>> GetAllAsync(int from, int size, bool? isCommerical,string origin,bool? hasBeers);
        Task<BreweryDto> GetSingleAsync(int id);
        Task<IEnumerable<BreweryDto>> SearchAsync(string query, int from, int size);
        Task UpdateAllAsync(IEnumerable<BreweryDto> breweryDtos);
        Task DeleteAsync(int id);
        Task<IEnumerable<BreweryMemberDto>> GetAllMembersAsync(int breweryId);
        Task<BreweryMemberDto> GetSingleMemberAsync(int breweryId, string username);
        IEnumerable<BreweryMemberDto> GetMemberships(string userId);
    }
}
