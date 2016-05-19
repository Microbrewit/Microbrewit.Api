using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.Database;

namespace Microbrewit.Api.Repository.Interface
{
    public interface IYeastRepository
    {
        Task<IEnumerable<Yeast>> GetAllAsync();
        Task<Yeast> GetSingleAsync(int id);
        Task AddAsync(Yeast yeast);
        Task<int> UpdateAsync(Yeast yeast);
        Task RemoveAsync(Yeast yeast);
    }
}
