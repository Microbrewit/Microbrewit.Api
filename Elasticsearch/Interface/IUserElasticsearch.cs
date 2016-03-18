using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;
using Nest;

namespace Microbrewit.Api.ElasticSearch.Interface
{
    public interface IUserElasticsearch
    {
        Task<IIndexResponse> UpdateAsync(UserDto userDto);
        Task<IEnumerable<UserDto>> GetAllAsync(int from , int size);
        Task<UserDto> GetSingleAsync(string username);
        Task<IEnumerable<UserDto>> SearchAsync(string query, int from, int size);
        Task<IBulkResponse> UpdateAllAsync(IEnumerable<UserDto> users);
        Task<IDeleteResponse> DeleteAsync(string username);
    }
}
