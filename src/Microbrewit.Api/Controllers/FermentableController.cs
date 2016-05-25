using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
namespace Microbrewit.Api.Controllers
{
    /// <summary>
    /// Fermentable Controller
    /// </summary>
    [Route("[controller]")]
    public class FermentablesController : Controller
    {
        private readonly IFermentableService _fermentableService;
        private readonly ILogger<FermentablesController> _logger;

        public FermentablesController(IFermentableService fermentableService, ILogger<FermentablesController> logger)
        {
            _fermentableService = fermentableService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all fermentables.
        /// </summary>
        /// <response code="200">OK</response>
        /// <returns></returns>
        [HttpGet]
        public async Task<FermentablesCompleteDto> GetFermentables(int from = 0, int size = 20 ,string custom = "false")
        {
            if (size > 1000) size = 1000;
            var fermentables = await _fermentableService.GetAllAsync(from,size,custom);
            return new FermentablesCompleteDto { Fermentables = fermentables.ToList() };
        }

        /// <summary>
        /// Get a fermentable by its id.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Fermentable </param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetFermentable(int id)
        {
            var fermentableDto = await _fermentableService.GetSingleAsync(id);
            if (fermentableDto == null) return NotFound();
            var result = new FermentablesCompleteDto() { Fermentables = new List<FermentableDto>() };
            result.Fermentables.Add(fermentableDto);
            return Ok(result);
        }

        /// <summary>
        /// Updates a fermentable.
        /// </summary>
        /// <param name="id">Fermentable id</param>
        /// <param name="fermentableDto">Fermentable data transfer object</param>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <returns></returns>
        [Authorize(Roles=("Admin"))]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutFermentable(int id, [FromBody]FermentableDto fermentableDto)
        {
            if (fermentableDto == null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != fermentableDto.Id)
                return BadRequest();
            await _fermentableService.UpdateAsync(fermentableDto);
            return new StatusCodeResult((int)HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds fermentables.
        /// </summary>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <param name="fermentableDto">List of fermentable transfer objects</param>
        /// <returns></returns>
        [Authorize(Roles=("Admin"))]
        [HttpPost]
        public async Task<IActionResult> PostFermentable([FromBody]FermentableDto fermentableDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _fermentableService.AddAsync(fermentableDto);
            _logger.LogInformation(fermentableDto.Name);
            //return CreatedAtRoute(new { controller = "fermentables", }, result);
            return Created("fermentables",result);
        }

        /// <summary>
        /// Deletes a fermentable by id.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Fermentable id</param>
        /// <returns></returns>
        [Authorize(Roles=("Admin"))]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteFermentable(int id)
        {
            var fermentable = await _fermentableService.DeleteAsync(id);
            if (fermentable == null)
                return NotFound();
            return Ok(fermentable);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [Authorize(Roles=("Admin"))]
        [HttpGet("es")]
        public async Task<IActionResult> UpdateFermentableElasticSearch()
        {
            await _fermentableService.ReIndexElasticsearch();
            return Ok();
        }

        [HttpGet("search")]
        public async Task<FermentablesCompleteDto> GetFermentablesBySearch(string query, int from = 0, int size = 20)
        {
            var fermentablesDto = await _fermentableService.SearchAsync(query, from, size);
            return new FermentablesCompleteDto { Fermentables = fermentablesDto.ToList() };
        }
    }
}