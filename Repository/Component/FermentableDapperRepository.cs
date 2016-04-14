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
    public class FermentableDapperRepository : IFermentableRepository
    {
       private readonly DatabaseSettings _databaseSettings;
       public FermentableDapperRepository(IOptions<DatabaseSettings> databaseSettings)
       {
           _databaseSettings = databaseSettings.Value;

       }
        public async Task<IList<Fermentable>> GetAllAsync(int from, int size)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                var fermentables = await connection.QueryAsync<Fermentable, Supplier, Origin, Fermentable>(
                    "SELECT fermentable_id AS FermentableId, f.name, ebc, lovibond, ppg, f.supplier_id AS SupplierId, type," +
                    "custom, super_fermentable_id AS SuperFermentableId, " +
                    "s.supplier_id AS SupplierId, s.name, s.origin_id AS OriginId, o.origin_id AS OriginId, o.name " +
                    "FROM fermentables f " +
                    "LEFT JOIN suppliers s ON f.supplier_id = s.supplier_id " +
                    "LEFT JOIN origins o ON s.origin_id = o.origin_id;",
                    (f, supplier, origin) =>
                    {
                        if (supplier != null)
                            supplier.Origin = origin;
                        f.Supplier = supplier;
                        return f;
                    }, splitOn: "SupplierId,OriginId");

                foreach (var fermentable in fermentables)
                {
                    var select =
                         "SELECT fermentable_id AS FermentableId, f.name, ebc, lovibond, ppg, f.supplier_id AS SupplierId, type, " +
                         "custom, super_fermentable_id AS SuperFermentableId FROM Fermentables f ";
                    if (fermentable.SuperFermentableId != null)
                    {

                        var superWhere = " WHERE fermentable_id = @SuperFermentableId;";
                        fermentable.SuperFermentable = (await
                                 connection.QueryAsync<Fermentable>(select + superWhere,
                                    new { fermentable.SuperFermentableId })).SingleOrDefault();
                    }
                    var subWhere = " WHERE super_fermentable_id = @FermentableId;";
                    fermentable.SubFermentables = (await
                        connection.QueryAsync<Fermentable>(select + subWhere,
                            new { fermentable.FermentableId })).ToList();
                }
                return fermentables.ToList();
            }
        }

        public async Task<Fermentable> GetSingleAsync(int id)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                var fermentables = await connection.QueryAsync<Fermentable, Supplier, Origin, Fermentable>(
                    "SELECT fermentable_id AS FermentableId, f.name, ebc, lovibond, ppg, f.supplier_id AS SupplierId, type," +
                    "custom, super_fermentable_id AS SuperFermentableId, " +
                    "s.supplier_id AS SupplierId, s.name, s.origin_id AS OriginId, o.origin_id AS OriginId, o.name " +
                    "FROM fermentables f " +
                    "LEFT JOIN suppliers s ON f.supplier_id = s.supplier_id " +
                    "LEFT JOIN origins o ON s.origin_id = o.origin_id " + 
                    "WHERE f.fermentable_id = @FermentableId;",
                    (f, supplier, origin) =>
                    {
                        if (supplier != null)
                            supplier.Origin = origin;
                        f.Supplier = supplier;
                        return f;
                    }, new { FermentableId = id }, splitOn: "SupplierId,OriginId");
                var fermentable = fermentables.SingleOrDefault();
                if (fermentable == null) return null;
                var select =
                         "SELECT fermentable_id AS FermentableId, f.name, ebc, lovibond, ppg, f.supplier_id AS SupplierId, type, " +
                         "custom, super_fermentable_id AS SuperFermentableId FROM Fermentables f ";
                if (fermentable.SuperFermentableId != null)
                {

                    var superWhere = " WHERE fermentable_id = @SuperFermentableId;";
                    fermentable.SuperFermentable = (await
                             connection.QueryAsync<Fermentable>(select + superWhere,
                                new { fermentable.SuperFermentableId })).SingleOrDefault();
                }
                var subWhere = " WHERE super_fermentable_id = @FermentableId;";
                fermentable.SubFermentables = (await
                    connection.QueryAsync<Fermentable>(select + subWhere,
                        new { fermentable.FermentableId })).ToList();
                return fermentable;
            }
        }

        public async Task AddAsync(Fermentable fermentable)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                       var result =
                            await connection.ExecuteAsync("INSERT INTO fermentables(name,super_fermentable_id,EBC,Lovibond,PPG,supplier_id,Type,Custom) " +
                                               "VALUES(@Name,@SuperFermentableId,@EBC,@Lovibond,@PPG,@SupplierId,@Type,@Custom);", fermentable, transaction);
                        var fermentableId = await connection.QueryAsync<int>("SELECT last_value FROM fermentables_seq;");
                        fermentable.FermentableId = fermentableId.SingleOrDefault();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }

                }
            }
        }

        public async Task<int> UpdateAsync(Fermentable fermentable)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var result = await connection.ExecuteAsync("Update Fermentables set Name = @Name,super_fermentable_id = @SuperFermentableId,EBC = @EBC,Lovibond = @Lovibond,PPG= @PPG,supplier_id = @SupplierId,Type = @Type, Custom = @Custom " +
                                        "WHERE fermentable_id = @FermentableId;",
                                              fermentable, transaction);
                        transaction.Commit();
                        return result;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task RemoveAsync(Fermentable fermentable)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await connection.ExecuteAsync("DELETE FROM fermentables WHERE fermentable_id = @FermentableId",
                            new { fermentable.FermentableId }, transaction);
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
