using System.Threading.Tasks;
using Microbrewit.Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;


namespace Microbrewit.Api.Controllers
{
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("reset")]
        public async Task<IActionResult> ResetPassword([FromBody]string email)
        {
            await _userService.ResetPassword(email);
            return Ok();
        }
    }
}
