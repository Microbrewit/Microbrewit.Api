using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.Database;

namespace Microbrewit.Api.Repository.Interface
{
    public interface IBreweryRepository
    {

        Task<IEnumerable<Brewery>> GetAllAsync(int from, int size);
        Task<Brewery> GetSingleAsync(int id);
        Task AddAsync(Brewery brewery);
        Task<int> UpdateAsync(Brewery brewery);
        Task RemoveAsync(Brewery brewery);

        Task<BreweryMember> GetSingleMemberAsync(int breweryId,string userId);
        Task<IEnumerable<BreweryMember>> GetAllMembersAsync(int breweryId);
        Task DeleteMemberAsync(int breweryId, string username);
        Task UpdateMemberAsync(BreweryMember breweryMember);
        Task AddMemberAsync(BreweryMember breweryMember);
        Task<IEnumerable<BreweryMember>> GetMembershipsAsync(string userId);
        Task<IEnumerable<BrewerySocial>> GetBrewerySocialAsync(int breweryId);



    }
}
