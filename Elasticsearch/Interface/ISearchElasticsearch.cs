using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microbrewit.Api.ElasticSearch.Interface
{
    public interface ISearchElasticsearch
    {
        Task<IEnumerable<dynamic>> SearchAllAsync(string query, int from, int size);
        Task<IEnumerable<dynamic>> SearchIngredientsAsync(string query, int from, int size);
    }
}
