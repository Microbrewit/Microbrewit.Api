﻿using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Service.Interface;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;


namespace Microbrewit.Api.Controllers
{
    [Route("[controller]")]
    public class BeersController : Controller
    {
        private readonly ILogger<BeersController> _logger;
        private readonly IBeerService _beerService;

        public BeersController(IBeerService beerService,ILogger<BeersController> logger)
        {
            _beerService = beerService;
            _logger = logger;
        }

        /// <summary>
        /// Gets all beer
        /// </summary>
        /// <response code="200">It's all good!</response>
        /// <returns>Returns collection of all beers</returns>
        [HttpGet]
        public async Task<BeerCompleteDto> GetBeers(int from = 0, int size = 20) 
        {         
            if (size > 1000) size = 1000;
            var beers = await _beerService.GetAllAsync(from, size);
            var result = new BeerCompleteDto { Beers = beers};
            return result;
        }

        /// <summary>
        /// Gets all beer
        /// </summary>
        /// <response code="200">It's all good!</response>
        /// <returns>Returns collection of all beers</returns>
        [HttpGet("user/{username}")]
        public async Task<BeerCompleteDto> GetUserBeers(string username)
        {
            var beersDto = await _beerService.GetUserBeersAsync(username);
            var result = new BeerCompleteDto { Beers = beersDto};
            return result;
        }

        /// <summary>
        /// Gets beer by id
        /// </summary>
        /// <response code="200">Beer found and returned</response>
        /// <response code="404">Beer with that id not found</response>
        ///<param name="id">Id of the beer</param>
        /// <returns></returns>
        [HttpGet("{id}")]
       public async Task<IActionResult> GetBeer(int id)
        {
            var beerDto = await _beerService.GetSingleAsync(id);
            if (beerDto == null) return HttpNotFound();
            var result = new BeerCompleteDto() { Beers = new List<BeerDto>{beerDto}};
            return Ok(result);
        }

        /// <summary>
        /// Updates a beer
        /// </summary>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <param name="id">Id of the beer</param>
        /// <param name="beerDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBeer(int id, BeerDto beerDto)
        {
            if (!ModelState.IsValid) return HttpBadRequest(ModelState);
            if (id != beerDto.Id) return HttpBadRequest();
            await _beerService.UpdateAsync(beerDto);
            return new HttpStatusCodeResult((int) HttpStatusCode.Created);
        }

        /// <summary>
        /// Adds a beer
        /// </summary>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <param name="beerDto"></param>
        /// <returns></returns>
        [HttpPost("")]
       public async Task<IActionResult> PostBeer(BeerDto beerDto)
        {
            if (beerDto == null) return HttpBadRequest("Missing data");
            if (!ModelState.IsValid) return HttpBadRequest(ModelState);
            
            var username = HttpContext.User.Identity.Name;
            if (username == null) return HttpBadRequest("Missing user");
            var beer = await _beerService.AddAsync(beerDto, username);
            if (beer == null) return HttpBadRequest();
            return CreatedAtRoute(new { controller = "beers" }, beer);
        }

        /// <summary>
        /// Deletes a beer
        /// </summary>
        /// <response code="200">Ok</response>
        /// <resppmse code="404">Not Found</resppmse>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBeer(int id)
        {
            var beerDto = await _beerService.DeleteAsync(id);
            if (beerDto == null) return HttpNotFound();
            return Ok(beerDto);
        }
      
        /// <summary>
        /// Searches in beers.
        /// </summary>
        /// <param name="query">Query phrase</param>
        /// <param name="from">From what object</param>
        /// <param name="size">StepNumber of objects returned</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<BeerCompleteDto> GetBeerBySearch(string query, int from = 0, int size = 20)
        {
            if(size > 1000) size = 1000;
            var beerDtos = await _beerService.SearchAsync(query, from, size);
            return new BeerCompleteDto {Beers = beerDtos};
        }

        /// <summary>
        /// Get the last beers added.
        /// </summary>
        /// <param name="from">from beer</param>
        /// <param name="size">number of beer returned</param>
        /// <returns></returns>
        [HttpGet("last")]
        public async Task<BeerCompleteDto> GetLastAddedBeers(int from = 0, int size = 20)
        {
            if (size > 1000) size = 1000;
            var beerDto = await _beerService.GetLastAsync(from, size);
            return new BeerCompleteDto{ Beers = beerDto};
        }
    }
}
