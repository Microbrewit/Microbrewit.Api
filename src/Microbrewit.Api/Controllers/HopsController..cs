using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Service.Interface;
using Microsoft.Extensions.Logging;
using System.Net;
using Microbrewit.Api.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace Microbrewit.Api.Controllers
{
    [Route("[controller]")]
    public class HopsController : Controller
    {
        
        private readonly IHopService _hopService;
        private readonly ILogger<HopsController> _logger;
        
        public HopsController(IHopService hopService, ILogger<HopsController> logger)
        {
            _hopService = hopService;
            _logger = logger;
        }
        
        [HttpGet]
        public async Task<HopCompleteDto> GetHops(int from = 0, int size = 20)
        {
            _logger.LogDebug("Api Url:" + ApiConfiguration.ApiSettings.Url);
            if(size > 1000) size = 1000;
            var hops = await _hopService.GetAllAsync(from,size);
            return new HopCompleteDto { Hops = hops};
        }

        [HttpGet("aromawheels/{aromawheel}")]
         public async Task<HopCompleteDto> GetHopsByAromaWheel(string aromawheel)
        {
            _logger.LogDebug("Api Url:" + ApiConfiguration.ApiSettings.Url);
            var hops = await _hopService.GetHopsByAromaWheel(aromawheel);
            return new HopCompleteDto { Hops = hops};
        }
        
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetHop(int id)
        {
            var hopDto = await _hopService.GetSingleAsync(id);
            if (hopDto == null)
                return NotFound();
            var result = new HopCompleteDto() { Hops = new List<HopDto> {hopDto} };
            return Ok(result);
        }

        [Authorize(Roles=("Admin"))]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutHop(int id, [FromBody]HopDto hopDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != hopDto.Id)
                return BadRequest();
            await _hopService.UpdateHopAsync(hopDto);
            return new StatusCodeResult((int)HttpStatusCode.NoContent);
        }
        
        [Authorize(Roles=("Admin"))]
        [HttpPost]
        public async Task<IActionResult> PostHop([FromBody] HopDto hopDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _hopService.AddAsync(hopDto);
            return CreatedAtRoute(new { controller = "hops", }, result);
        }

        [Authorize(Roles=("Admin"))]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteHop(int id)
        {
            var hop = await _hopService.DeleteAsync(id);
            if (hop == null)
                return NotFound();
            return Ok(hop);
        }

        [HttpGet("forms")]
        public async Task<IEnumerable<DTO>> GetHopForm()
        {
            return await _hopService.GetHopFromsAsync();
        }
        
        [HttpGet("aromawheels")]
        public async Task<IEnumerable<AromaWheelDto>> GetHopAromaWheel()
        {
            return await _hopService.GetAromaWheelsAsync();
        }

        [Authorize(Roles=("Admin"))]
        [ApiExplorerSettings(IgnoreApi = true)]
        [Route("es")]
        [HttpGet]
        public async Task<IActionResult> UpdateHopsElasticSearch()
        {
            await _hopService.ReIndexHopsElasticSearch();
            return Ok();
        }

        [HttpGet("search")]
        public async Task<HopCompleteDto> GeHopsBySearch(string query, int from = 0, int size = 20)
        {
            var hops = await _hopService.SearchHop(query, from, size);
            return new HopCompleteDto { Hops = hops };
        }


    }
}
