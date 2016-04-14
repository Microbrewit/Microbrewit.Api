using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Service.Interface;
namespace Microbrewit.Api.Service.Component
{
    public class IngredientService : IIngredientService
    {
        IHopService _hopService;
        IFermentableService _fermentableService;
        IOtherService _otherService;
        IYeastService _yeastService;
        public IngredientService(IHopService hopService, IFermentableService fermentableService,
        IOtherService otherService, IYeastService yeastService)
        {
            _hopService = hopService;
            _fermentableService = fermentableService;
            _otherService = otherService;
            _yeastService = yeastService;
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

        public Task<IEnumerable<IIngredientDto>> SearchAsync(string query, int from, int size)
        {
            throw new NotImplementedException();
        }
    }
}