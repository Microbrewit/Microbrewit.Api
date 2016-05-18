using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.Database;

namespace Microbrewit.Api.Repository.Interface
{
    public interface ISupplierRepository
    {
        Task<IList<Supplier>> GetAllAsync();
        Task<Supplier> GetSingleAsync(int id);
        Task AddAsync(Supplier supplier);
        Task<int> UpdateAsync(Supplier supplier);
        Task RemoveAsync(Supplier supplier);
    }
}
