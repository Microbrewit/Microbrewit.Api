using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Service.Interface
{
    public interface IHopService
    {
        Task<IEnumerable<HopDto>> GetAllAsync(int from, int size);
        Task<HopDto> GetSingleAsync(int id);
        Task<HopDto> AddAsync(HopDto hopDto);
        Task<HopDto> DeleteAsync(int id);
        Task UpdateHopAsync(HopDto hopDto);
        Task<IEnumerable<HopDto>> SearchHop(string query, int from, int size);
        Task ReIndexHopsElasticSearch();
        Task<IEnumerable<DTO>> GetHopFromsAsync();
    }
}
