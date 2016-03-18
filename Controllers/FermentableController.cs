using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Service.Interface;
using Microsoft.AspNet.Mvc;


namespace Microbrewit.Api.Controllers
{
    /// <summary>
    /// Fermentable Controller
    /// </summary>
    [Route("[controller]")]
    public class FermentablesController : Controller
    {
        private readonly IFermentableService _fermentableService;

        public FermentablesController(IFermentableService fermentableService)
        {
            _fermentableService = fermentableService;
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
            if (fermentableDto == null) return HttpNotFound();
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
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutFermentable(int id, [FromBody]FermentableDto fermentableDto)
        {
            if (fermentableDto == null)
                return HttpBadRequest();
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState);
            if (id != fermentableDto.Id)
                return HttpBadRequest();
            await _fermentableService.UpdateAsync(fermentableDto);
            return new HttpStatusCodeResult((int)HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds fermentables.
        /// </summary>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <param name="fermentableDto">List of fermentable transfer objects</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostFermentable([FromBody]FermentableDto fermentableDto)
        {
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState);
            var result = await _fermentableService.AddAsync(fermentableDto);
            return CreatedAtRoute(new { controller = "fermentables", }, result);
        }

        /// <summary>
        /// Deletes a fermentable by id.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Fermentable id</param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteFermentable(int id)
        {
            var fermentable = await _fermentableService.DeleteAsync(id);
            if (fermentable == null)
                return HttpNotFound();
            return Ok(fermentable);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("es")]
        public async Task<IActionResult> UpdateFermentableElasticSearch()
        {
            await _fermentableService.ReIndexElasticsearch();
            return Ok();
        }

        [HttpGet]
        [Route("")]
        public async Task<FermentablesCompleteDto> GetFermentablesBySearch(string query, int from = 0, int size = 20)
        {
            var fermentablesDto = await _fermentableService.SearchAsync(query, from, size);
            return new FermentablesCompleteDto { Fermentables = fermentablesDto.ToList() };
        }
    }
}