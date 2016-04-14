using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.Database;

namespace Microbrewit.Api.Repository.Interface
{
    public interface IUserRepository
    {
        Task<IList<User>> GetAllAsync();
        Task<User> GetSingleAsync(string user_id);
        Task AddAsync(User user);
        Task<int> UpdateAsync(User user);
        Task RemoveAsync(User user);        
        IEnumerable<UserSocial> GetUserSocials(string user_id);
        Task<IEnumerable<UserBeer>> GetAllUserBeersAsync(string user_id);
     
    }
}
