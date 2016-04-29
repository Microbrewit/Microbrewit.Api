using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Service.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync(int from, int size);
        Task<UserDto> GetSingleByUserIdAsync(string userId);
        Task<UserDto> GetSingleByUsernameAsync(string username);
        Task<UserDto> AddAsync(User user);
        Task<UserDto> DeleteAsync(string username);
        Task UpdateAsync(UserDto userDto);
        Task<IEnumerable<UserDto>> SearchAsync(string query, int from, int size);
        Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(string username);         
        
        Task ReIndexElasticSearch();
        Task ReIndexBeerRelationElasticSearch(BeerDto beerDto);
        Task ReIndexBreweryRelationElasticSearch(BreweryDto breweryDto);
        Task ReIndexUserElasticSearch(string username);
        Task<bool> UpdateNotification(string username, NotificationDto notificationDto);
    }
}
