using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.Database;

namespace Microbrewit.Api.Repository.Interface
{
    public interface IBeerStyleRepository
    {

        Task<IList<BeerStyle>> GetAllAsync(int from, int size);
        Task<BeerStyle> GetSingleAsync(int id);
        Task AddAsync(BeerStyle beerStyle);
        Task<int> UpdateAsync(BeerStyle beerStyle);
        Task RemoveAsync(BeerStyle beerStyle);
    }
}
