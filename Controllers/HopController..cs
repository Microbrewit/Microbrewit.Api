using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Service.Interface;
using Microsoft.Extensions.Logging;
using System.Net;
using Microbrewit.Api.Configuration;

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
        
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetHop(int id)
        {
            var hopDto = await _hopService.GetSingleAsync(id);
            if (hopDto == null)
                return HttpNotFound();
            var result = new HopCompleteDto() { Hops = new List<HopDto> {hopDto} };
            return Ok(result);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutHop(int id, [FromBody]HopDto hopDto)
        {
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState);

            if (id != hopDto.Id)
                return HttpBadRequest();
            await _hopService.UpdateHopAsync(hopDto);
            return new HttpStatusCodeResult((int)HttpStatusCode.NoContent);
        }

        [HttpPost]
        public async Task<IActionResult> PostHop([FromBody] HopDto hopDto)
        {
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState);
            var result = await _hopService.AddAsync(hopDto);
            return CreatedAtRoute(new { controller = "hops", }, result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteHop(int id)
        {
            var hop = await _hopService.DeleteAsync(id);
            if (hop == null)
                return HttpNotFound();
            return Ok(hop);
        }

        [HttpGet("forms")]
        public async Task<IEnumerable<DTO>> GetHopForm()
        {
            return await _hopService.GetHopFromsAsync();
        }

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
