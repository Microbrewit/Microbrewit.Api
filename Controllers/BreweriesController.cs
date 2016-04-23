using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Service.Interface;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;


namespace Microbrewit.Api.Controllers
{
    [Route("[controller]")]
    public class BreweriesController : Controller
    {
        private readonly IBreweryService _breweryService;

        public BreweriesController(IBreweryService breweryService)
        {
            _breweryService = breweryService;
        }

        /// <summary>
        /// Get all breweries.
        /// <response code="200">OK</response>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<BreweryCompleteDto> GetBreweries(int from = 0, int size = 20, bool? isCommercial = null, string origin = null)
        {
            if (size > 1000) size = 1000;
            var breweriesDto = await _breweryService.GetAllAsync(from, size,isCommercial,origin);
            var result = new BreweryCompleteDto { Breweries = breweriesDto};
            return result;
        }


        /// <summary>
        /// Get brewery by id.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Beerstyle id</param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
      public async Task<IActionResult> GetBrewery(int id)
        {
            var breweryDto = await _breweryService.GetSingleAsync(id);
            if (breweryDto == null) return HttpNotFound();
            var breweryCompleteDto = new BreweryCompleteDto() {Breweries = new List<BreweryDto> {breweryDto}};
            return Ok(breweryCompleteDto);
        }

        /// <summary>
        /// Updates a brewery member.
        /// </summary>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <param name="id">Brewery id</param>
        /// <param name="breweryDto">Brewery object</param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("{id:int}")]
       public async Task<IActionResult> PutBrewery(int id,[FromBody] BreweryDto breweryDto)
        {
            // Checks if login user is allowed to change brewery.
            if (!ModelState.IsValid)
            {
                return HttpBadRequest(ModelState);
            }

            if (id != breweryDto.Id)
            {
                return HttpBadRequest();
            }
            await _breweryService.UpdateAsync(breweryDto);
            return new HttpStatusCodeResult((int) HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Add brewery.
        /// </summary>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <param name="brewery">List of brewery objects</param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostBrewery([FromBody]BreweryDto brewery)
        {
            if (brewery == null) return HttpBadRequest();
            if (!ModelState.IsValid) return HttpBadRequest(ModelState);
            var result = await _breweryService.AddAsync(brewery);
            return CreatedAtRoute(new { controller = "breweries" }, result);
        }



        /// <summary>
        /// Deletes brewery by id.
        /// </summary>
        /// <response code="404">Node Found</response>
        /// <param name="id">Brewery id.</param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBrewery(int id)
        {
            var brewery = await _breweryService.DeleteAsync(id);
            if (brewery == null) return HttpNotFound();
            return Ok(brewery);
        }

        /// <summary>
        /// Deletes a brewery member from a brewery.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Brewery id</param>
        /// <param name="username">Username for member</param>
        /// <returns>Deleted brewery member</returns>
        [HttpDelete("{id:int}/members/{username}")]
       public async Task<IActionResult> DeleteBreweryMember(int id, string username)
        {
            var breweryMember = await _breweryService.DeleteMember(id, username);
            if (breweryMember == null) return HttpNotFound();
            return Ok(breweryMember);
        }

        /// <summary>
        /// Gets a brewery member of a brewery
        /// </summary>
        /// <response code="200">Ok</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Brewery id</param>
        /// <param name="username">Member username</param>
        /// <returns>Returns brewery member</returns>
        [HttpGet("{id:int}/members/{username}")]
        public async Task<IActionResult> GetBreweryMember(int id, string username)
        {
            var breweryMember = await _breweryService.GetBreweryMember(id, username);
            if (breweryMember == null) return HttpNotFound();
            return Ok(breweryMember);
        }

        /// <summary>
        /// Gets all brewery members for a brewery.
        /// </summary>
        /// <response code="200">Ok</response>
        /// <param name="id">Brewery id</param>
        /// <returns>Returns list of brewery members</returns>
        [HttpGet("{id:int}/members")]
         public async Task<IActionResult> GetBreweryMembers(int id)
        {
            var breweryMembers = await _breweryService.GetAllMembers(id);
            return Ok(breweryMembers);
        }

        /// <summary>
        /// Updates a brewery member for a brewery.
        /// </summary>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <param name="id">Brewery id</param>
        /// <param name="username">Brewery admin users username</param>
        /// <param name="breweryMember">Member to be added</param>
        /// <returns></returns>
        [HttpPut("{id:int}/members/{username}")]
       public async Task<IActionResult> PutBreweryMember(int id, string username, [FromBody]BreweryMemberDto breweryMember)
        {
            if (!ModelState.IsValid) return HttpBadRequest(ModelState);
            if (username != breweryMember.UserId) return HttpBadRequest();
            await _breweryService.UpdateBreweryMember(id, breweryMember);
            return new HttpStatusCodeResult((int) HttpStatusCode.Created);
        }

        [HttpPost("{id:int}/members")]
         public async Task<IActionResult> PostBreweryMember(int id, [FromBody]BreweryMemberDto breweryMember)
        {
            if (!ModelState.IsValid) return HttpBadRequest(ModelState);
            var result = await _breweryService.AddBreweryMember(id, breweryMember);
            return Ok(result);
        }

        /// <summary>
        /// Updates elasticsearch with database data.
        /// </summary>
        /// <returns></returns>
        [HttpGet("es")]
        public async Task<IActionResult> UpdateBreweryElasticSearch()
        {
            await _breweryService.ReIndexElasticSearch();
            return Ok();
        }


        /// <summary>
        /// Searches in breweries.
        /// </summary>
        /// <param name="query">The pharse you want to match.</param>
        /// <param name="from">Start point of the search.</param>
        /// <param name="size">Number of results returned.</param>
        /// <returns></returns>
        [HttpGet("search")]
        public async Task<BreweryCompleteDto> GetBreweriesBySearch(string query, int from = 0, int size = 20)
        {
            if (size > 1000) size = 1000;
            var breweriesDto = await _breweryService.SearchAsync(query, from, size);
            return new BreweryCompleteDto { Breweries = breweriesDto };
        }
    }
}