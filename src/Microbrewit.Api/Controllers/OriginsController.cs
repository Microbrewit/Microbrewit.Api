using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microbrewit.Api.Controllers
{
    [Route("[controller]")]
    public class OriginsController : Controller
    {
        private readonly IOriginService _originService;

        public OriginsController(IOriginService originService)
        {
            _originService = originService;
        }
        /// <summary>
        /// Get all origins.
        /// </summary>
        /// <response code="200">OK</response>
        /// <returns></returns>
        [HttpGet]
        public async Task<OriginCompleteDto> Get(int from = 0,int size = 20,string custom = "false")
        {
            var originDto = await _originService.GetAllAsync(from, size, custom);
            var originsComplete = new OriginCompleteDto { Origins = originDto};
            return originsComplete;
        }

        /// <summary>
        /// Get origin by id.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code=""404>Not Found</response>
        /// <param name="id">Origin id</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var originDto = await _originService.GetSingleAsync(id);
            if (originDto == null)
                return NotFound();
            var originsComplete = new OriginCompleteDto { Origins = new List<OriginDto>{originDto}};
            return  Ok(originsComplete);
        }

        /// <summary>
        /// Updates a origin
        /// </summary>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <param name="id">Origin id</param>
        /// <param name="origin"></param>
        /// <returns></returns>
        [Authorize(Roles=("Admin"))]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([FromRoute]int id, [FromBody]OriginDto origin)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != origin.Id)
                return BadRequest();
            await _originService.UpdateAsync(origin);
            return new StatusCodeResult((int) HttpStatusCode.NoContent);        }

        /// <summary>
        /// Adds origins.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        /// <param name="origin"></param>
        /// <returns></returns>
        [Authorize(Roles=("Admin"))]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]OriginDto origin)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _originService.AddAsync(origin);
            return CreatedAtRoute(new { controller = "origins", }, result);
        }

        /// <summary>
        /// Deletes a origin.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Origin id</param>
        /// <returns></returns>
        [Authorize(Roles=("Admin"))]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id)
        {

            var origin = await _originService.DeleteAsync(id);
            if (origin == null)
                return NotFound();
            return Ok(origin);
        }
        /// <summary>
        /// Updates elasticsearch with data from the database.
        /// </summary>
        /// <returns>200 OK</returns>
        [Authorize(Roles=("Admin"))]
        [HttpGet("es")]
        public async Task<IActionResult> UpdateOriginElasticSearch()
        {
             await _originService.ReIndexElasticSearch();
             return Ok();
        }
        /// <summary>
        /// Searches in origin.
        /// </summary>
        /// <param name="query">Query phrase</param>
        /// <param name="from">From what object</param>
        /// <param name="size">Number of objects returned</param>
        /// <returns></returns>
        [HttpGet("search")]
        public async Task<OriginCompleteDto> GetOriginBySearch(string query, int from = 0, int size = 20)
        {
           var result = await _originService.SearchAsync(query, from, size);
            return new OriginCompleteDto {Origins = result};
        }
     
    }
}