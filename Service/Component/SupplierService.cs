using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Model.DTOs;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Service.Interface;
namespace Microbrewit.Api.Service.Component
{
    public class SupplierService : ISupplierService
    {
        private ISupplierRepository _supplierRepository;
        
        public SupplierService(ISupplierRepository SupplierRepository)
        {
            _supplierRepository = SupplierRepository;
        }
        public async Task<SupplierDto> AddAsync(SupplierDto SupplierDto)
        {
            var supplier = AutoMapper.Mapper.Map<SupplierDto,Supplier>(SupplierDto);
            await _supplierRepository.AddAsync(supplier);
            return AutoMapper.Mapper.Map<Supplier,SupplierDto>(supplier);
        }

        public async Task<SupplierDto> DeleteAsync(int id)
        {
            var supplier = await _supplierRepository.GetSingleAsync(id);
            await _supplierRepository.RemoveAsync(supplier);
            return AutoMapper.Mapper.Map<Supplier,SupplierDto>(supplier);
        }


        public async Task<IEnumerable<SupplierDto>> GetAllAsync(string custom)
        {
            var suppliers = await _supplierRepository.GetAllAsync();
            return AutoMapper.Mapper.Map<IEnumerable<Supplier>,IEnumerable<SupplierDto>>(suppliers);
        }

        public async Task<SupplierDto> GetSingleAsync(int id)
        {
            var supplier = await _supplierRepository.GetSingleAsync(id);
            return AutoMapper.Mapper.Map<Supplier,SupplierDto>(supplier);
        }

        public Task ReIndexElasticSearch()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<OriginDto>> SearchAsync(string query, int from, int size)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(SupplierDto SupplierDto)
        {
            var supplier = AutoMapper.Mapper.Map<SupplierDto,Supplier>(SupplierDto);
            await _supplierRepository.UpdateAsync(supplier);
        }

        Task<IEnumerable<SupplierDto>> ISupplierService.SearchAsync(string query, int from, int size)
        {
            throw new NotImplementedException();
        }
    }
}