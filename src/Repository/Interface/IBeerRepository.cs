using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.Database;

namespace Microbrewit.Api.Repository.Interface
{
    public interface IBeerRepository
    {
        Task<IEnumerable<Beer>> GetAllAsync(int from,int size);
        Task<Beer> GetSingleAsync(int id);
        Task AddAsync(Beer beer);
        Task<int> UpdateAsync(Beer beer);
        Task RemoveAsync(Beer beer);
        Task<IEnumerable<Beer>> GetLastAsync(int from, int size);
        Task<IEnumerable<Beer>> GetAllUserBeerAsync(string username);
        Task<IEnumerable<Beer>> GetAllBreweryBeersAsync(int breweryId);

    }
}
