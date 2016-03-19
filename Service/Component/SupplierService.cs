using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.ElasticSearch.Interface;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Service.Interface;
using System.Linq;

namespace Microbrewit.Api.Service.Component
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly ISupplierElasticsearch _supplierElasticsearch;
        
        public SupplierService(ISupplierRepository supplierRepository, ISupplierElasticsearch supplierElasticsearch)
        {
            _supplierRepository = supplierRepository;
            _supplierElasticsearch = supplierElasticsearch;
        }
        public async Task<SupplierDto> AddAsync(SupplierDto supplierDto)
        {
             var supplier = AutoMapper.Mapper.Map<SupplierDto, Supplier>(supplierDto);
            await _supplierRepository.AddAsync(supplier);
            var result = await _supplierRepository.GetSingleAsync(supplier.SupplierId);
            var mappedResult = AutoMapper.Mapper.Map<Supplier, SupplierDto>(result);
            await _supplierElasticsearch.UpdateAsync(mappedResult);
            return mappedResult;
        }

        public async Task<SupplierDto> DeleteAsync(int id)
        {
            var supplier = await _supplierRepository.GetSingleAsync(id);
            var supplierDto = await _supplierElasticsearch.GetSingleAsync(id);
            if (supplier != null) await _supplierRepository.RemoveAsync(supplier);
            if (supplierDto != null) await _supplierElasticsearch.DeleteAsync(id);
            return supplierDto ?? AutoMapper.Mapper.Map<Supplier, SupplierDto>(supplier);
        }


        public async Task<IEnumerable<SupplierDto>> GetAllAsync(string custom)
        {
             var supplierDtos = await _supplierElasticsearch.GetAllAsync(custom);
            if (supplierDtos.Any()) return supplierDtos;
            var suppliers = await _supplierRepository.GetAllAsync();
            supplierDtos = AutoMapper.Mapper.Map<IEnumerable<Supplier>, IEnumerable<SupplierDto>>(suppliers);
            return supplierDtos;
        }

        public async Task<SupplierDto> GetSingleAsync(int id)
        {
            var supplierDto = await _supplierElasticsearch.GetSingleAsync(id);
            if (supplierDto != null) return supplierDto;
            var supplier = await _supplierRepository.GetSingleAsync(id);
            supplierDto = AutoMapper.Mapper.Map<Supplier, SupplierDto>(supplier);
            return supplierDto;
        }

        public async Task ReIndexElasticSearch()
        {
            var suppliers = await _supplierRepository.GetAllAsync();
            var supplierDtos = AutoMapper.Mapper.Map<IEnumerable<Supplier>,IEnumerable<SupplierDto>>(suppliers);
            await _supplierElasticsearch.UpdateAllAsync(supplierDtos);
        }

        public async Task<IEnumerable<SupplierDto>> SearchAsync(string query, int from, int size)
        {
             return await _supplierElasticsearch.SearchAsync(query, from, size);
        }

        public async Task UpdateAsync(SupplierDto supplierDto)
        {
              var supplier = AutoMapper.Mapper.Map<SupplierDto, Supplier>(supplierDto);
            await _supplierRepository.UpdateAsync(supplier);
            var result = await _supplierRepository.GetSingleAsync(supplier.SupplierId);
            var mapperResult = AutoMapper.Mapper.Map<Supplier, SupplierDto>(result);
            await _supplierElasticsearch.UpdateAsync(mapperResult);
        }
    }
}