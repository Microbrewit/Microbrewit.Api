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
    public class BeerStylesController : Controller
    {
        private readonly IBeerStyleService _beerStyleService;

        public BeerStylesController(IBeerStyleService beerStyleService)
        {
            _beerStyleService = beerStyleService;
        }

        /// <summary>
        /// Get all beerstyles.
        /// <response code="200">OK</response>
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<BeerStyleCompleteDto> GetBeerStyles(int from = 0, int size = 20)
        {
            if (size > 1000) size = 1000;
            var beerStylesDto = await _beerStyleService.GetAllAsync(from, size);
            var response = new BeerStyleCompleteDto() { BeerStyles = beerStylesDto.ToList()};
            return response;
        }


        /// <summary>
        /// Get beerstyle by id.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Beerstyle id</param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetBeerStyle(int id)
        {
            var beerStyleDto = await _beerStyleService.GetSingleAsync(id);
            if (beerStyleDto == null) return NotFound();
            var response = new BeerStyleCompleteDto() { BeerStyles = new List<BeerStyleDto>() };
            response.BeerStyles.Add(beerStyleDto);
            return Ok(response);
        }

        /// <summary>
        /// Updates a beerstyle.
        /// </summary>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <param name="id">Beerstyle id</param>
        /// <param name="beerstyle">BeerStyle object</param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutBeerStyle(int id, [FromBody]BeerStyleDto beerstyle)
        {
            if (!ModelState.IsValid)return BadRequest(ModelState);
            if (id != beerstyle.Id) return BadRequest();
            await _beerStyleService.UpdateAsync(beerstyle);
            return new StatusCodeResult((int)HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Add beerstyle.
        /// </summary>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <param name="beerStylesDto">Beerstyle object.</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostBeerStyle([FromBody]BeerStyleDto beerStylesDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = await _beerStyleService.AddAsync(beerStylesDto);
            return CreatedAtRoute(new { controller = "beerstyles" }, result);
        }

        /// <summary>
        /// Deletes beerstyle by id.
        /// </summary>
        /// <response code="404">Node Found</response>
        /// <param name="id">Beerstyle id.</param>
        /// <returns></returns>
        [Route("{id}")]
        public async Task<IActionResult> DeleteBeerStyle(int id)
        {
            var beerstyle = await _beerStyleService.DeleteAsync(id);
            if (beerstyle == null) return NotFound();
            return Ok(beerstyle);
        }
        /// <summary>
        /// Searches in beer styles.
        /// </summary>
        /// <param name="query">Query phrase</param>
        /// <param name="from">From what object</param>
        /// <param name="size">Number of objects returned</param>
        /// <returns></returns>
        [HttpGet]
        [Route("search")]
        public async Task<BeerStyleCompleteDto> GetBeerBySearch(string query, int from = 0, int size = 20)
        {
            var result = await _beerStyleService.SearchAsync(query, from, size);
            return new BeerStyleCompleteDto {BeerStyles = result.ToList()};
        }


        [HttpGet]
        [Route("es")]
        public async Task<IActionResult> UpdateBeerStyleElasticSearch()
        {
            await _beerStyleService.ReIndexElasticSearch();
            return Ok();
        }
    }
}