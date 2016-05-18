using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.Database;

namespace Microbrewit.Api.Repository.Interface
{
    public interface IFermentableRepository 
    {
        Task<IList<Fermentable>> GetAllAsync(int from, int size);
        Task<Fermentable> GetSingleAsync(int id);
        Task AddAsync(Fermentable fermentable);
        Task<int> UpdateAsync(Fermentable fermentable);
        Task RemoveAsync(Fermentable fermentable);
    }
}
