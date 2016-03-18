using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.ElasticSearch.Interface
{
    public interface ISupplierElasticsearch
    {
        Task UpdateAsync(SupplierDto supplierDto);

        Task<IEnumerable<SupplierDto>> GetAllAsync(string custom);
        Task<SupplierDto> GetSingleAsync(int id);
        Task<IEnumerable<SupplierDto>> SearchAsync(string query, int from, int size);
        Task UpdateAllAsync(IEnumerable<SupplierDto> supplierDtos);
        Task DeleteAsync(int id);
    }
}
