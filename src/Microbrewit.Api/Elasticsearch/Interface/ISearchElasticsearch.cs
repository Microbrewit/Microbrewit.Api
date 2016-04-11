using System.Threading.Tasks;

namespace Microbrewit.Api.ElasticSearch.Interface
{
    public interface ISearchElasticsearch
    {
        Task<string> SearchAllAsync(string query, int from, int size);
        Task<string> SearchIngredientsAsync(string query, int from, int size);
    }
}
