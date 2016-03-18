using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Service.Interface;

namespace Microbrewit.Api.Service.Component
{
    public class FermentableService : IFermentableService
    {
        private readonly IFermentableRepository _fermentableRepository;

        public FermentableService(IFermentableRepository fermentableRepository)
        {
            _fermentableRepository = fermentableRepository;
        }
        public async Task<IEnumerable<FermentableDto>> GetAllAsync(int @from, int size, string custom)
        {
            var fermentables = await _fermentableRepository.GetAllAsync(from, size);
            var fermentableDtos = AutoMapper.Mapper.Map<IEnumerable<Fermentable>, IEnumerable<FermentableDto>>(fermentables);
            return fermentableDtos;
        }

        public async Task<FermentableDto> GetSingleAsync(int id)
        {
            var fermentable = await _fermentableRepository.GetSingleAsync(id);
            var fermentableDto = AutoMapper.Mapper.Map<Fermentable, FermentableDto>(fermentable);
            return fermentableDto;
        }

        public async Task<FermentableDto> AddAsync(FermentableDto fermentableDto)
        {
            var fermantable = AutoMapper.Mapper.Map<FermentableDto, Fermentable>(fermentableDto);
            await _fermentableRepository.AddAsync(fermantable);
            var result = await _fermentableRepository.GetSingleAsync(fermantable.FermentableId);
            var mappedResult = AutoMapper.Mapper.Map<Fermentable, FermentableDto>(result);
            return mappedResult;
        }

        public async Task<FermentableDto> DeleteAsync(int id)
        {
            var fermentable = await _fermentableRepository.GetSingleAsync(id);
            if (fermentable != null) await _fermentableRepository.RemoveAsync(fermentable);
            var fermentableDto = AutoMapper.Mapper.Map<Fermentable, FermentableDto>(fermentable);
            return fermentableDto;
        }

        public async Task UpdateAsync(FermentableDto fermentableDto)
        {
            var fermentable = AutoMapper.Mapper.Map<FermentableDto, Fermentable>(fermentableDto);
            await _fermentableRepository.UpdateAsync(fermentable);
            await _fermentableRepository.GetSingleAsync(fermentableDto.Id);
        }

        public async Task<IEnumerable<FermentableDto>> SearchAsync(string query, int @from, int size)
        {
            throw new NotImplementedException();
        }

        public async Task ReIndexElasticsearch()
        {
            throw new NotImplementedException();
        }
    }
}
