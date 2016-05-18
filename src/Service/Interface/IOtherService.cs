using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Service.Interface
{
    public interface IOtherService
    {
        Task<IEnumerable<OtherDto>> GetAllAsync(string custom);
        Task<OtherDto> GetSingleAsync(int id);
        Task<OtherDto> AddAsync(OtherDto otherDto);
        Task<OtherDto> DeleteAsync(int id);
        Task UpdateAsync(OtherDto otherDto);
        Task<IEnumerable<OtherDto>> SearchAsync(string query, int from, int size);
        Task ReIndexElasticSearch();
    }
}