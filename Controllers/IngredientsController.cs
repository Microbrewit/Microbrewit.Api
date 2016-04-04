
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Microsoft.AspNet.Mvc;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Service.Interface;
using System.Collections.Generic;

namespace Microbrewit.Api.Controllers
{
    [Route("[controller]")]
    public class IngredientsController : Controller
    {
        private readonly IIngredientService _ingredientsService;
        
        public IngredientsController(IIngredientService ingredientService)
        {
            _ingredientsService = ingredientService;
        }

        [HttpGet]
        public async Task<IEnumerable<IIngredientDto>> GetIngredients(string custom)
        {
            return await _ingredientsService.GetAllAsync(custom);
        }

        /// <summary>
        /// Search in all ingedients in Microberew.it store
        /// </summary>
        /// <param name="query">The thing you want found.</param>
        /// <param name="from">Start of result returns.</param>
        /// <param name="size">Size of return result.</param>
        /// <returns>List of ingredients with score.</returns>
        [HttpGet("search")]
        public async Task<IActionResult> GetAllIngredients(string query, int from = 0, int size = 20)
        {
            if (size > 1000) size = 1000; 
            var result = await _searchElasticsearch.SearchIngredientsAsync(query, from, size);
            return Ok(JObject.Parse(result));
            return Ok();
        }
    }
}
