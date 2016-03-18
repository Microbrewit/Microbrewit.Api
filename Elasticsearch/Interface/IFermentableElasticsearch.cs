using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.ElasticSearch.Interface
{
    public interface IFermentableElasticsearch
    {
        Task UpdateAsync(FermentableDto fermentableDto);
        Task<IEnumerable<FermentableDto>> GetAllAsync(int from, int size,string custom);
        Task<FermentableDto> GetSingleAsync(int id);
        Task<IEnumerable<FermentableDto>> SearchAsync(string query, int from, int size);
        Task UpdateAllAsync(IEnumerable<FermentableDto> fermentableDtos);
        Task DeleteAsync(int id);

        FermentableDto GetSingle(int id);
        IEnumerable<FermentableDto> GetAll(string custom);
        IEnumerable<FermentableDto> Search(string query, int from, int size);
    }
}
