using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Microbrewit.Api.Controllers
{
    [Route("users")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
       

        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService,ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        // GET api/User
        [Route("")]
        public async Task<UserCompleteDto> GetUsers(int from = 0, int size = 20)
        {
            if (size > 1000) size = 1000;
            var users = await _userService.GetAllAsync(from,size);
            var result = new UserCompleteDto {Users = users.ToList()};
            return result;
        }

        // GET api/User/5
        [HttpGet("{username}")]
        public async Task<IActionResult> GetUser(string username)
        {
            var user = await _userService.GetSingleByUsernameAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            var result = new UserCompleteDto() { Users = new List<UserDto>() };
            result.Users.Add(user);
            return Ok(result);
        }

        /// <summary>
        /// Searches in users.
        /// </summary>
        /// <param name="query">The pharse you want to match.</param>
        /// <param name="from">Start point of the search.</param>
        /// <param name="size">Number of results returned.</param>
        /// <returns></returns>
        [HttpGet("search")]
        public async Task<UserCompleteDto> GetUsersBySearch(string query, int from = 0, int size = 20)
        {
            var usersDto = await _userService.SearchAsync(query, from, size);
            return new UserCompleteDto {Users = usersDto.ToList()};
        }

        /// <summary>
        /// Updates elasticsearch with data from the database.
        /// </summary>
        /// <returns>200 OK</returns>
        [HttpGet("es")]
        public async Task<IActionResult> UpdateUsersElasticSearch()
        {
            await _userService.ReIndexElasticSearch();
            return Ok();
        }
        [HttpGet("{username}/notifications")]
        public async Task<IActionResult> GetUserNotifications(string username)
        {
            var notifications = await _userService.GetUserNotificationsAsync(username);
            return Ok(notifications);
        }

        [HttpGet("{username}/notifications/{id}")]
        public async Task<IActionResult> GetUserNotifications(string username,int id)
        {
            var notifications = await _userService.GetUserNotificationsAsync(username);
            return Ok(notifications);
        }

        [HttpPost("{username}/notifications")]
        public async Task<IActionResult> UpdateUserNoTifications(string username, NotificationDto notificationDto)
        {

            var changed = await _userService.UpdateNotification(username, notificationDto);
            if (!changed) return BadRequest();
            return Ok();
        }
    }
}