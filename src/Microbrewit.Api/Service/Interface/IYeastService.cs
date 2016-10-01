using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Service.Interface
{
    public interface IYeastService
    {
        Task<IEnumerable<YeastDto>> GetAllAsync();
        Task<YeastDto> GetSingleAsync(int id);
        Task<YeastDto> AddAsync(YeastDto yeastDto);
        Task<YeastDto> DeleteAsync(int id);
        Task UpdateAsync(YeastDto yeastDto);
        Task<IEnumerable<YeastDto>> SearchAsync(string query, int from, int size);
        Task ReIndexElasticSearch();
    }
}
