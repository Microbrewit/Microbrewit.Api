using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microbrewit.Api.ElasticSearch.Interface;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Service.Interface;

namespace Microbrewit.Api.Service.Component
{
    public class FermentableService : IFermentableService
    {
        private readonly IFermentableElasticsearch _fermentableElasticsearch;
        private readonly IFermentableRepository _fermentableRepository;

        public FermentableService(IFermentableRepository fermentableRepository,IFermentableElasticsearch fermentableElasticsearch)
        {
            _fermentableElasticsearch = fermentableElasticsearch;
            _fermentableRepository = fermentableRepository;
        }
        public async Task<IEnumerable<FermentableDto>> GetAllAsync(int @from, int size, string custom)
        {
            var fermentableDtos = await _fermentableElasticsearch.GetAllAsync(from,size,custom);
            if (fermentableDtos .Any()) return fermentableDtos ;
            var fermentables = await _fermentableRepository.GetAllAsync(from,size);
            fermentableDtos = AutoMapper.Mapper.Map<IEnumerable<Fermentable>, IEnumerable<FermentableDto>>(fermentables);
            return fermentableDtos;
        }

        public async Task<FermentableDto> GetSingleAsync(int id)
        {
            var fermentableDto = await _fermentableElasticsearch.GetSingleAsync(id);
            if (fermentableDto != null) return fermentableDto;
            var fermentable = await _fermentableRepository.GetSingleAsync(id);
            fermentableDto = AutoMapper.Mapper.Map<Fermentable, FermentableDto>(fermentable);
            return fermentableDto;
        }

        public async Task<FermentableDto> AddAsync(FermentableDto fermentableDto)
        {
           var fermantable = AutoMapper.Mapper.Map<FermentableDto, Fermentable>(fermentableDto);
            await _fermentableRepository.AddAsync(fermantable);
            var result = await _fermentableRepository.GetSingleAsync(fermantable.FermentableId);
            var mappedResult = AutoMapper.Mapper.Map<Fermentable,FermentableDto>(result);
            await _fermentableElasticsearch.UpdateAsync(mappedResult);
            return mappedResult;
        }

        public async Task<FermentableDto> DeleteAsync(int id)
        {
            var fermentable = await _fermentableRepository.GetSingleAsync(id);
            var fermentableDto = await _fermentableElasticsearch.GetSingleAsync(id);
            if (fermentable != null) await _fermentableRepository.RemoveAsync(fermentable);
            if (fermentableDto != null) await _fermentableElasticsearch.DeleteAsync(id);
            return fermentableDto ?? AutoMapper.Mapper.Map<Fermentable,FermentableDto>(fermentable);
        }

        public async Task UpdateAsync(FermentableDto fermentableDto)
        {
            var fermentable = AutoMapper.Mapper.Map<FermentableDto, Fermentable>(fermentableDto);
            await _fermentableRepository.UpdateAsync(fermentable);
            var result = await _fermentableRepository.GetSingleAsync(fermentableDto.Id);
            var mappedResult = AutoMapper.Mapper.Map<Fermentable, FermentableDto>(result);
            await _fermentableElasticsearch.UpdateAsync(mappedResult);
        }

        public async Task<IEnumerable<FermentableDto>> SearchAsync(string query, int @from, int size)
        {
            return await _fermentableElasticsearch.SearchAsync(query, from, size);
        }

        public async Task ReIndexElasticsearch()
        {
              var fermentables = await _fermentableRepository.GetAllAsync(0,int.MaxValue);
            var fermentableDtos = AutoMapper.Mapper.Map<IEnumerable<Fermentable>, IEnumerable<FermentableDto>>(fermentables);
            await _fermentableElasticsearch.UpdateAllAsync(fermentableDtos);
        }
    }
}
