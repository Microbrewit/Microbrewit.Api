using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Microbrewit.Api.Controllers
{
    [Route("suppliers")]
    public class SuppliersController : Controller
    {
        private readonly ISupplierService _supplierService;

        public SuppliersController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        /// <summary>
        /// Gets all suppliers
        /// </summary>
        /// <response code="200">OK</response>
        /// <returns></returns>
        [HttpGet]
        public async Task<SupplierCompleteDto> GetSuppliers(string custom = "false")
        {
            var suppliersDto = await _supplierService.GetAllAsync(custom);
            var result = new SupplierCompleteDto {Suppliers = suppliersDto.OrderBy(s => s.Name).ToList()};
            return result;
        }

        /// <summary>
        /// Get a supplier.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="404">Not Found</response>
        /// <param name="id">Supplier id</param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetSupplier(int id)
        {
            var supplierDto = await _supplierService.GetSingleAsync(id);
            if (supplierDto == null)
            {
                return NotFound();
            }
            var result = new SupplierCompleteDto() { Suppliers = new List<SupplierDto>() };
            result.Suppliers.Add(supplierDto);
            return Ok(result);
        }

        /// <summary>
        /// Updates a supplier.
        /// </summary>
        /// <response code="204">No Content</response>
        /// <response code="400">Bad Request</response>
        /// <param name="id">Supplier id</param>
        /// <param name="supplierDto">Supplier transfer object</param>
        /// <returns></returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutSupplier(int id, [FromBody]SupplierDto supplierDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != supplierDto.Id)
            {
                return BadRequest();
            }
            await _supplierService.UpdateAsync(supplierDto);
            return new StatusCodeResult((int) HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Adds suppliers.
        /// </summary>
        /// <param name="supplierDtos">List of supplier tranfer objects</param>
        /// <response code="201">Created</response>
        /// <response code="400">Bad Request</response>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> PostSupplier([FromBody]SupplierDto supplierDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _supplierService.AddAsync(supplierDto);
            return CreatedAtRoute(new { controller = "suppliers", }, result);
        }

        /// <summary>
        /// Delets supplier.
        /// </summary>
        /// <response code="200">OK</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var supplier = await _supplierService.DeleteAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }
            return Ok(supplier);
        }

        [HttpGet("es")]
        public async Task<IActionResult> UpdateSupplierElasticSearch()
        {
            await _supplierService.ReIndexElasticSearch();
            return Ok();
        }

        [HttpGet("search")]
        public async Task<SupplierCompleteDto> GetSuppliersBySearch(string query, int from = 0, int size = 20)
        {
            var supplierDto = await _supplierService.SearchAsync(query,from,size);
            var result = new SupplierCompleteDto {Suppliers = supplierDto.ToList()};
            return result;
        }
    }
}