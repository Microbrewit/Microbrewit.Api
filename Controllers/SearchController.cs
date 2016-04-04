
using System.Threading.Tasks;
using Microbrewit.Api.ElasticSearch.Interface;
using Newtonsoft.Json.Linq;
using Microsoft.AspNet.Mvc;

namespace Microbrewit.Api.Controllers
{
    [Route("[controller]")]
    public class SearchController : Controller
    {
        private readonly ISearchElasticsearch _searchElasticsearch;
        
        public SearchController(ISearchElasticsearch searchElasticsearch)
        {
            _searchElasticsearch = searchElasticsearch;
        }

        /// <summary>
        /// Searches in all things Microbrew.it.
        /// </summary>
        /// <param name="query">The thing you want found.</param>
        /// <param name="from">Start of result returns.</param>
        /// <param name="size">Size of return result.</param>
        /// <returns>All things.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll(string query, int from = 0, int size = 20)
        {
            if (size > 1000) size = 1000;
            var result = await _searchElasticsearch.SearchAllAsync(query, from, size);
            return Ok(JObject.Parse(result));
        }
    }
}
