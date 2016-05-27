using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.DTOs;

namespace Microbrewit.Api.Elasticsearch.Interface
{
    public interface IHopElasticsearch
    {
        Task UpdateAsync(HopDto hopDto);
        Task<IEnumerable<HopDto>> GetAllAsync(int from, int size);
        Task<HopDto> GetSingleAsync(int id);
        Task<IEnumerable<HopDto>> SearchAsync(string query, int from, int size);
        Task UpdateAllAsync(IEnumerable<HopDto> hops);
        Task DeleteAsync(int id);
        HopDto GetSingle(int hopId);
        IEnumerable<HopDto> Search(string query, int from, int size);
        Task UpdateAromaWheelAsync(AromaWheelDto aromaWheelDto);
        Task UpdateAllAromaWheelAsync(IEnumerable<AromaWheelDto> aromaWheels);
        Task<IEnumerable<AromaWheelDto>> GetAromaWheelsAsync();
    }
}
