using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Service.Interface
{
    public interface IFermentableService
    {
        Task<IEnumerable<FermentableDto>> GetAllAsync(int from, int size,string custom);
        Task<FermentableDto> GetSingleAsync(int id);
        Task<FermentableDto> AddAsync(FermentableDto fermentableDto);
        Task<FermentableDto> DeleteAsync(int id);
        Task UpdateAsync(FermentableDto fermentableDto);
        Task<IEnumerable<FermentableDto>> SearchAsync(string query, int from, int size);
        Task ReIndexElasticsearch();

        Task ReIndexBySupplier(int supplierId);
    }
}
