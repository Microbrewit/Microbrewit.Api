using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;


namespace Microbrewit.Api.Controllers
{
    [Route("yeasts")]
    public class YeastsController : Controller
    {
        private readonly IYeastService _yeastService;

        public YeastsController(IYeastService yeastService)
        {
            _yeastService = yeastService;
        }


        /// <summary>
        /// Gets all yeasts.
        /// </summary>
        /// <returns>200 OK</returns>
        [HttpGet]
        public async Task<YeastCompleteDto> GetYeasts(string custom = "false")
        {
            var yeasts = await _yeastService.GetAllAsync(custom);
            return new YeastCompleteDto {Yeasts = yeasts.ToList()};
        }

        /// <summary>
        /// Gets single yeast.
        /// api.microbrew.it/yeasts/:id
        /// </summary>
        /// <param name="id">Yeast Id</param>
        /// <returns>200 OK Single yeast</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetYeast(int id)
        {
            var yeastDto = await _yeastService.GetSingleAsync(id);
            if (yeastDto == null)
                return NotFound();
            var result = new YeastCompleteDto() { Yeasts = new List<YeastDto>() };
            result.Yeasts.Add(yeastDto);
            return Ok(result);
        }

        /// <summary>
        /// Updates a yeast.
        /// </summary>
        /// <param name="id">Yeast Id</param>
        /// <param name="yeastDto">Json of the YeastDto object</param>
        /// <returns>No Content 204</returns>
        [Authorize(Roles=("Admin"))]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutYeast(int id, [FromBody] YeastDto yeastDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != yeastDto.Id)
                return BadRequest();
            await _yeastService.UpdateAsync(yeastDto);
            return new StatusCodeResult((int) HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Inserts new yeast.
        /// </summary>
        /// <param name="yeastDto">Takes a list of YeastDto objects in form of json</param>
        /// <returns>201 Created</returns>
        [Authorize(Roles=("Admin"))]
        [HttpPost]
        public async Task<IActionResult> PostYeast([FromBody]YeastDto yeastDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _yeastService.AddAsync(yeastDto);
            return CreatedAtRoute(new { controller = "yeasts", }, result);
        }

        /// <summary>
        /// Deletes a yeast
        /// </summary>
        /// <param name="id">Yeast id</param>
        /// <returns>200 OK</returns>
        [Authorize(Roles=("Admin"))]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteYeast(int id)
        {
            var yeast = await _yeastService.DeleteAsync(id);
            if (yeast == null)
            {
                return NotFound();
            }
            return Ok(yeast);
        }

        [Authorize(Roles=("Admin"))]
        [ApiExplorerSettings(IgnoreApi=true)]
        [HttpGet("es")]
        public async Task<IActionResult> UpdateElasticSearchYeast()
        {
            await _yeastService.ReIndexElasticSearch();
            return Ok();
        }
        /// <summary>
        /// Searches in yeasts
        /// </summary>
        /// <param name="query">The pharse you want to match.</param>
        /// <param name="from">Start point of the search.</param>
        /// <param name="size">Number of results returned.</param>
        /// <returns></returns>
        [HttpGet("search")]
        public async Task<YeastCompleteDto> GetYeastsBySearch(string query, int from = 0, int size = 20)
        {
            var yeastsDto = await _yeastService.SearchAsync(query, from, size);
            return new YeastCompleteDto {Yeasts = yeastsDto.ToList()};
        }
    }
}