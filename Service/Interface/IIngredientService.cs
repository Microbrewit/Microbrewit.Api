using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Service.Interface
{
    public interface IIngredientService
    {
        Task<IEnumerable<IIngredientDto>> GetAllAsync(string custom);
        Task<IEnumerable<dynamic>> SearchAsync(string query, int from, int size);
        Task ReIndexElasticSearch();
    }
}