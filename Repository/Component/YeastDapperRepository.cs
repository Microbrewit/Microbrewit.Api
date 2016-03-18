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

namespace Microbrewit.Repository.Repository
{

    public class YeastDapperRepository : IYeastRepository
    {
        private DatabaseSettings _databaseSettings;
        public YeastDapperRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings.Value;   
        }
        public async Task<IEnumerable<Yeast>> GetAllAsync()
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                var fields = "y.yeast_id AS YeastId, y.name as Name, y.temperature_low AS TemperatureLow, y.temperature_high AS TemperatureHigh," +
                             " y.flocculation, y.alcohol_tolerance AS AlcoholTolerance, y.product_code AS ProductCode, y.notes, " +
                             "y.type, y.brewery_source AS BrewerySource, y.species, y.attenution_range AS AttenutionRange, y.pitching_fermentation_notes As PitchingFermentationNotes, " +
                             "y.supplier_id AS SupplierId, y.custom, s.supplier_id AS SupplierId, s.name, s.origin_id AS OriginId ";
                string sql = $"SELECT {fields} FROM yeasts y LEFT JOIN suppliers s ON y.supplier_id = s.supplier_id ORDER BY y.name;";
                var yeasts = await connection.QueryAsync<Yeast, Supplier, Yeast>(sql, (yeast, supplier) =>
                {
                    yeast.Supplier = supplier;
                    return yeast;
                }, splitOn: "YeastId,SupplierId");
                return yeasts.ToList();
            }
        }

        public async Task<Yeast> GetSingleAsync(int id)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                var fields = "y.yeast_id AS YeastId, y.name as Name, y.temperature_low AS TemperatureLow, y.temperature_high AS TemperatureHigh," +
                             " y.flocculation, y.alcohol_tolerance AS AlcoholTolerance, y.product_code AS ProductCode, y.notes, " +
                             "y.type, y.brewery_source AS BrewerySource, y.species, y.attenution_range AS AttenutionRange, y.pitching_fermentation_notes As PitchingFermentationNotes, " +
                             "y.supplier_id AS SupplierId, y.custom, s.supplier_id AS SupplierId, s.name, s.origin_id AS OriginId ";
                var sql = $"SELECT {fields} FROM yeasts y LEFT JOIN suppliers s ON y.supplier_id = s.supplier_id WHERE y.yeast_id = @YeastId;";
                var yeast = await connection.QueryAsync<Yeast, Supplier, Yeast>(sql, (s, supplier) =>
                {
                    s.Supplier = supplier;
                    return s;
                }, new { YeastId = id }, splitOn: "YeastId,SupplierId");
                return yeast.SingleOrDefault();
            };
        }

        public async Task AddAsync(Yeast yeast)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var result = await connection.ExecuteAsync("INSERT INTO yeasts(name,temperature_high,temperature_low,flocculation,alcohol_tolerance,product_code,notes,type,brewery_source,species,attenution_range,pitching_fermentation_notes,supplier_id,custom) VALUES(@Name,@TemperatureHigh,@TemperatureLow,@Flocculation,@AlcoholTolerance,@ProductCode,@Notes,@Type,@BrewerySource,@Species,@AttenutionRange,@PitchingFermentationNotes,@SupplierId,@Custom);",                           
                                yeast,transaction);
                        
                        var yeastId = await connection.QueryAsync<int>("SELECT last_value FROM yeasts_seq;");
                        yeast.YeastId = yeastId.SingleOrDefault();
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public async Task<int> UpdateAsync(Yeast yeast)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var result = await connection.ExecuteAsync(
                                "UPDATE Yeasts set name = @Name,temperature_high = @TemperatureHigh,temperature_low = @TemperatureLow,flocculation = @Flocculation," +
                                "alcohol_tolerance = @AlcoholTolerance,product_code = @ProductCode, notes = @Notes, type = @Type, brewery_source = @BrewerySource, species = @Species," +
                                "attenution_range = @AttenutionRange, pitching_fermentation_notes = @PitchingFermentationNotes, supplier_id = @SupplierId, custom = @Custom " +
                                "WHERE yeast_id = @YeastId;",yeast, transaction);
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

        public async Task RemoveAsync(Yeast yeast)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await connection.ExecuteAsync("DELETE FROM yeasts WHERE yeast_id = @YeastId", new { yeast.YeastId }, transaction);
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
