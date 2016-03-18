using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Service.Interface
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierDto>> GetAllAsync(string custom);
        Task<SupplierDto> GetSingleAsync(int id);
        Task<SupplierDto> AddAsync(SupplierDto supplierDto);
        Task<SupplierDto> DeleteAsync(int id);
        Task UpdateAsync(SupplierDto supplierDto);
        Task<IEnumerable<SupplierDto>> SearchAsync(string query, int from, int size);
        Task ReIndexElasticSearch();
    }
}
