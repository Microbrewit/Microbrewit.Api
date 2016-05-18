
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Microbrewit.Api.Controllers
{
    [Route("[controller]")]
    public class GlassesController : Controller
    {
        private readonly IGlassService _glassService;

        public GlassesController(IGlassService glassService)
        {
            _glassService = glassService;
        }

        /// <summary>
        /// Gets all glassware from the database
        /// </summary>
        /// <returns>200 ok</returns>
        [HttpGet]
        public async Task<GlassCompleteDto> GetGlasses()
        {
            var glassesDto = await _glassService.GetAllAsync();
            var result = new GlassCompleteDto {Glasses = glassesDto.OrderBy(g => g.Name).ToList()};
            return result;
        }

        /// <summary>
        /// Get single glass
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="400">Not Found</response>
        /// <param name="id">Glass id</param>
        /// <returns>Glass</returns>
        [HttpGet("{id:int}")]
          public async Task<IActionResult> GetGlass(int id)
        {
            var glassDto = await _glassService.GetSingleAsync(id);
            var result = new GlassCompleteDto() { Glasses = new List<GlassDto>() };
            result.Glasses.Add(glassDto);
            return Ok(result);
        }

        /// <summary>
        /// Updates a glass.
        /// </summary>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <param name="id"></param>
        /// <param name="glassDto"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutGlass(int id,[FromBody]GlassDto glassDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (id != glassDto.Id)
                return BadRequest();
            await _glassService.UpdateAsync(glassDto);
            return new StatusCodeResult((int) HttpStatusCode.NoContent);
        }
        /// <summary>
        /// Add new glass.
        /// </summary>
        /// <param name="glassDtos"></param>
        /// <returns></returns>
        [HttpPost] 
        public async Task<IActionResult> PostGlass([FromBody]GlassDto glassDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            await _glassService.AddAsync(glassDto);
            return CreatedAtRoute("DefaultApi", new { controller = "glasses", }, glassDto);
        }

        /// <summary>
        /// Delets glass by id.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Glass id</param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpDelete("{id:int}")]
       public async Task<IActionResult> DeleteGlass(int id)
        {
            var glass = await _glassService.DeleteAsync(id);
            if (glass == null)
                return NotFound();
            return Ok(glass);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("es")]
        public async Task<IActionResult> UpdateGlassElasticSearch()
        {
            await _glassService.ReIndexElasticSearch();
            return Ok();
        }


        [HttpGet("search")]
        public async Task<GlassCompleteDto> GetGlassBySearch(string query, int from = 0, int size = 20)
        {
            var glassesDto = await _glassService.SearchAsync(query, from, size);
            var result = new GlassCompleteDto {Glasses = glassesDto.ToList()};
            return result;
        }
    }
}
