using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.Database;

namespace Microbrewit.Api.Repository.Interface
{
    public interface IOriginRespository
    {
        Task<IEnumerable<Origin>> GetAllAsync(int from, int size);
        Task<Origin> GetSingleAsync(int id, params string[] navigtionProperties);
        Task AddAsync(Origin origin);
        Task<int> UpdateAsync(Origin origin);
        Task RemoveAsync(Origin origin);
    }
}
