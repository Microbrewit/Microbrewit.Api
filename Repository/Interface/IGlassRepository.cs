using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.Database;

namespace Microbrewit.Api.Repository.Interface
{
    public interface IGlassRepository
    {
        Task<IEnumerable<Glass>> GetAllAsync();
        Task<Glass> GetSingleAsync(int id);
        Task AddAsync(Glass glass);
        Task<int> UpdateAsync(Glass glass);
        Task RemoveAsync(Glass glass);
    }
}
