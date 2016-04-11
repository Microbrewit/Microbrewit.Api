using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Settings;
using Microsoft.Extensions.OptionsModel;
using Npgsql;

namespace Microbrewit.Api.Repository.Component
{

    public class SupplierDapperRepository : ISupplierRepository
    {
        private DatabaseSettings _databaseSettings;
        public SupplierDapperRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings.Value;
        }
      public async Task<IList<Supplier>> GetAllAsync()
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                var sql = @"SELECT s.supplier_id AS SupplierId, s.name, s.origin_id As OriginId, o.origin_id AS OriginId, o.name FROM Suppliers s LEFT JOIN Origins o ON s.origin_id = o.origin_id";
                var suppliers = await connection.QueryAsync<Supplier, Origin, Supplier>(sql, (supplier, origin) =>
                {
                    supplier.Origin = origin;
                    return supplier;
                }, splitOn: "OriginId");
                return suppliers.ToList();
            }
        }

        public async Task<Supplier> GetSingleAsync(int id)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                var sql = @"SELECT s.supplier_id AS SupplierId, s.name, s.origin_id As OriginId, o.origin_id AS OriginId, o.name FROM suppliers s LEFT JOIN Origins o ON s.origin_id = o.origin_id WHERE supplier_id = @SupplierId;";
                var supplier = await connection.QueryAsync<Supplier, Origin, Supplier>(sql, (s, origin) =>
                {
                    s.Origin = origin;
                    return s;
                }, new { SupplierId = id }, splitOn: "OriginId");
                return supplier.SingleOrDefault();
            };
        }

        public async Task AddAsync(Supplier supplier)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var result = await connection.ExecuteAsync("INSERT INTO Suppliers(name,origin_id) VALUES(@Name, @OriginId)",
                             new { supplier.Name, supplier.OriginId }, transaction);
                        var supplierId = await connection.QueryAsync<int>("SELECT last_value FROM suppliers_seq;");
                        supplier.SupplierId = supplierId.SingleOrDefault();
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public async Task<int> UpdateAsync(Supplier supplier)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var result = await connection.ExecuteAsync(
                                "UPDATE suppliers set name=@Name, origin_id=@OriginId WHERE supplier_id = @SupplierId;",
                                new {supplier.Name, supplier.OriginId, supplier.SupplierId}, transaction);
                        transaction.Commit();
                        return result;
                    }
                    catch (Exception exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task RemoveAsync(Supplier supplier)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await connection.ExecuteAsync("DELETE FROM Suppliers WHERE supplier_id = @SupplierId", new { supplier.SupplierId }, transaction);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
