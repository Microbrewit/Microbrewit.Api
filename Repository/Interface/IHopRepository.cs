using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.Database;

namespace Microbrewit.Api.Repository.Interface
{
    public interface IHopRepository
    {
        Task<IEnumerable<Hop>> GetAllAsync(int from, int size);
        Task<Hop> GetSingleAsync(int id);
        Task AddAsync(Hop item);
        Task<int> UpdateAsync(Hop item);
        Task RemoveAsync(Hop item);

        Task<Flavour> AddFlavourAsync(string name);
        Task<HopForm> GetFormAsync(int id);
        Task<IEnumerable<HopForm>> GetHopFormsAsync();
        Task<IEnumerable<Flavour>> GetFlavoursAsync();
    }
}
