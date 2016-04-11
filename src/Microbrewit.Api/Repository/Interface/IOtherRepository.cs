using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.Database;

namespace Microbrewit.Api.Repository.Interface
{
    public interface IOtherRepository
    {
        Task<IEnumerable<Other>> GetAllAsync();
        Task<Other> GetSingleAsync(int id);
        Task AddAsync(Other other);
        Task<int> UpdateAsync(Other other);
        Task RemoveAsync(Other other);
    }
}
