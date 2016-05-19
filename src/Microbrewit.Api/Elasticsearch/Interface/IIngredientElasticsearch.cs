using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microbrewit.Api.Elasticsearch.Interface
{
    public interface IIngredientElasticsearch
    {
        Task<IEnumerable<dynamic>> SearchIngredientsAsync(string query, int from, int size);
        Task<bool> HasIngredientsBeenUpdated(DateTime lastUpdatedTime);
    }
}
