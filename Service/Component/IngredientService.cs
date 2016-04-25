using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.ElasticSearch.Interface;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Service.Interface;
namespace Microbrewit.Api.Service.Component
{
    public class IngredientService : IIngredientService
    {
        readonly IHopService _hopService;
        readonly IFermentableService _fermentableService;
        readonly IOtherService _otherService;
        readonly IYeastService _yeastService;
        readonly ISearchElasticsearch _searchElasticsearch;

        public IngredientService(IHopService hopService, IFermentableService fermentableService,
        IOtherService otherService, IYeastService yeastService, ISearchElasticsearch searchElasticsearch)
        {
            _hopService = hopService;
            _fermentableService = fermentableService;
            _otherService = otherService;
            _yeastService = yeastService;
            _searchElasticsearch = searchElasticsearch;
        }
        public async Task<IEnumerable<IIngredientDto>> GetAllAsync(string custom)
        {
            var ingredients = new List<IIngredientDto>();
            ingredients.AddRange(await _hopService.GetAllAsync(0,100000));
            ingredients.AddRange(await _fermentableService.GetAllAsync(0, 10000, custom));
            ingredients.AddRange(await _otherService.GetAllAsync(custom));
            ingredients.AddRange(await _yeastService.GetAllAsync(custom));
            return ingredients;
            
        }

        public Task ReIndexElasticSearch()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<dynamic>> SearchAsync(string query, int from, int size)
        {
            return await _searchElasticsearch.SearchIngredientsAsync(query, from, size);
        }
    }
}