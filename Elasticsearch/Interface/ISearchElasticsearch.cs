using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microbrewit.Api.Elasticsearch.Interface
{
    public interface ISearchElasticsearch
    {
        Task<IEnumerable<dynamic>> SearchAllAsync(string query, int from, int size);
        
    }
}
