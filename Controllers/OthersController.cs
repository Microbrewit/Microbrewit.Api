using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Service.Interface;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;


namespace Microbrewit.Api.Controllers
{
    
    [Route("[controller]")]
    public class OthersController : Controller
    {
        private readonly IOtherService _otherService;

        public OthersController(IOtherService otherService)
        {
            _otherService = otherService;
        }

        /// <summary>
        /// Gets a collection of others
        /// </summary>
        /// <returns>Ok 200 on success</returns>
        /// <errorCode code="400"></errorCode>
        [Authorize]
        [HttpGet]
        public async Task<OtherCompleteDto> GetOthers(string custom = "false")
        {
            var other = await _otherService.GetAllAsync(custom);
            var result = new OtherCompleteDto {Others = other.ToList()};
            return result;
        }


        /// <summary>
        /// Get other.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="400">Not Found</response>
        /// <param name="id">Other id</param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOther(int id)
        {
            var otherDto = await _otherService.GetSingleAsync(id);
            if (otherDto == null)
                return HttpNotFound();
            var response = new OtherCompleteDto() { Others = new List<OtherDto>{otherDto} };
            return Ok(response);
        }

        /// <summary>
        /// Updates a other.
        /// </summary>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <param name="id"></param>
        /// <param name="otherDto"></param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutOther(int id, [FromBody]OtherDto otherDto)
        {
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState);
            if (id != otherDto.Id)
                return HttpBadRequest();
            await _otherService.UpdateAsync(otherDto);
            return new HttpStatusCodeResult((int) HttpStatusCode.NoContent);
        }
        /// <summary>
        /// Add new other.
        /// </summary>
        /// <param name="otherDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostOther([FromBody]OtherDto otherDto)
       {
            if (!ModelState.IsValid)
                return HttpBadRequest(ModelState);
            var result = await _otherService.AddAsync(otherDto);
            return CreatedAtRoute(new { controller = "others", }, result);
        }

        
        /// <summary>
        /// Delets other by id.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Other id</param>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi=true)]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteOther(int id)
        {
            var other = await _otherService.GetSingleAsync(id);
            if (other == null)
                return HttpNotFound();
            return Ok(other);
        }
        [HttpGet("es")]
        public async Task<IActionResult> UpdateOtherElasticSearch()
        {
            await _otherService.ReIndexElasticSearch();   
            return Ok();
        }
        
        [HttpGet("search")]
        public async Task<OtherCompleteDto> GetOthersBySearch(string query, int from = 0, int size = 20)
        {
            var othersDto = await _otherService.SearchAsync(query, from, size);
            return new OtherCompleteDto {Others = othersDto.ToList()};
        }
    }
}