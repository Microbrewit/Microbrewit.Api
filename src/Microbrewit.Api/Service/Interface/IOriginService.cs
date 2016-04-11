using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Service.Interface
{
    public interface IOriginService
    {
        Task<IEnumerable<OriginDto>> GetAllAsync(int from, int size,string custom);
        Task<OriginDto> GetSingleAsync(int id);
        Task<OriginDto> AddAsync(OriginDto originDto);
        Task<OriginDto> DeleteAsync(int id);
        Task UpdateAsync(OriginDto originDto);
        Task<IEnumerable<OriginDto>> SearchAsync(string query, int from, int size);
        Task ReIndexElasticSearch();
    }
}
