using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Microbrewit.Repository.Component
{

    public class YeastDapperRepository : IYeastRepository
    {
        private const string _yeastFields = "y.yeast_id AS YeastId, y.name as Name, y.temperature_low AS TemperatureLow, y.temperature_high AS TemperatureHigh," +
                             " y.flocculation, y.alcohol_tolerance AS AlcoholTolerance, y.product_code AS ProductCode, y.notes, " +
                             "y.type, y.brewery_source AS BrewerySource, y.species, y.attenution_range AS AttenuationRange, y.pitching_fermentation_notes As PitchingFermentationNotes, " +
                             "y.flocculation_low as FlocculationLow, y.flocculation_high As FlocculationHigh, y.attenution_low as AttenuationLow, y.attenution_high as AttenuationHigh, " +
                             "y.alcohol_tolerance_low as AlcoholToleranceLow, y.alcohol_tolerance_high as AlcoholToleranceHigh, " +
                             "y.supplier_id AS SupplierId, y.custom, s.supplier_id AS SupplierId, s.name, s.origin_id AS OriginId ";
        private readonly DatabaseSettings _databaseSettings;
        private readonly ILogger<YeastDapperRepository> _logger;
        public YeastDapperRepository(IOptions<DatabaseSettings> databaseSettings, ILogger<YeastDapperRepository> logger)
        {
            _databaseSettings = databaseSettings.Value;
            _logger = logger;   
        }
        public async Task<IEnumerable<Yeast>> GetAllAsync()
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();

                string sql = $"SELECT {_yeastFields} FROM yeasts y LEFT JOIN suppliers s ON y.supplier_id = s.supplier_id ORDER BY y.name;";
                var yeasts = await connection.QueryAsync<Yeast, Supplier, Yeast>(sql, (yeast, supplier) =>
                {
                    yeast.Supplier = supplier;
                    return yeast;
                }, splitOn: "YeastId,SupplierId");
                foreach (var yeast in yeasts)
                {
                    yeast.Sources = await GetYeastSources(yeast.YeastId, connection);
                    yeast.Flavours = await GetYeastFlavours(yeast.YeastId,connection);
                }
                return yeasts.ToList();
            }
        }

        public async Task<Yeast> GetSingleAsync(int id)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                var sql = $"SELECT {_yeastFields} FROM yeasts y LEFT JOIN suppliers s ON y.supplier_id = s.supplier_id WHERE y.yeast_id = @YeastId;";
                var yeast = (await connection.QueryAsync<Yeast, Supplier, Yeast>(sql, (s, supplier) =>
                {
                    s.Supplier = supplier;
                    return s;
                }, new { YeastId = id }, splitOn: "YeastId,SupplierId")).SingleOrDefault();
                if(yeast == null) return yeast;
                yeast.Sources = await GetYeastSources(yeast.YeastId, connection);
                yeast.Flavours = await GetYeastFlavours(yeast.YeastId,connection);
                return yeast;
            };
        }

        private async Task<IEnumerable<YeastFlavour>> GetYeastFlavours(int yeastId, DbConnection connection)
        {
            var sql = "SELECT yf.flavour_id as FlavourId, yf.yeast_id as YeastId, f.flavour_id as FlavourId, f.name " +
                      " FROM public.yeast_flavours yf INNER JOIN flavours f ON f.flavour_id = yf.flavour_id WHERE yf.yeast_id = @YeastId;";
            var result = await connection.QueryAsync<YeastFlavour,Flavour,YeastFlavour>(sql,(yeastFlavour, flavour) => 
            {
                yeastFlavour.Flavour = flavour;
                return yeastFlavour;
            }, new {YeastId = yeastId}, splitOn: "FlavourId");
            return result;
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
                        var result = await connection.ExecuteAsync("INSERT INTO yeasts(name,temperature_high,temperature_low,flocculation,alcohol_tolerance,product_code,notes,type,brewery_source,species,attenution_range,pitching_fermentation_notes,supplier_id,custom, flocculation_low,flocculation_high,attenution_low,attenution_high,alcohol_tolerance_low,alcohol_tolerance_high) VALUES (@Name,@TemperatureHigh,@TemperatureLow,@Flocculation,@AlcoholTolerance,@ProductCode,@Notes,@Type,@BrewerySource,@Species,@AttenuationRange,@PitchingFermentationNotes, @SupplierId, @Custom, @FlocculationLow,@FlocculationHigh,@AttenuationLow,@AttenuationHigh,@AlcoholToleranceLow,@AlcoholToleranceHigh);",                           
                                yeast,transaction);
                        
                        var yeastId = await connection.QueryAsync<int>("SELECT last_value FROM yeasts_seq;");
                        yeast.YeastId = yeastId.SingleOrDefault();
                        await InsertYeastSources(yeast, connection, transaction);
                        await InsertYeastFlavours(yeast,connection,transaction);
                        transaction.Commit();

                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.ToString());
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
                                "attenution_range = @AttenuationRange, pitching_fermentation_notes = @PitchingFermentationNotes, supplier_id = @SupplierId, custom = @Custom, " +
                                "flocculation_low = @FlocculationLow, flocculation_high = @FlocculationHigh, attenution_low = @AttenuationLow, attenution_high = @AttenuationHigh, alcohol_tolerance_low = @AlcoholToleranceLow, alcohol_tolerance_high = @AlcoholToleranceHigh " +
                                "WHERE yeast_id = @YeastId;", yeast, transaction);
                        await UpdateYeastSources(yeast,connection,transaction);
                        await InsertYeastFlavours(yeast,connection,transaction);
                        transaction.Commit();
                        return result;
                    }
                    catch (Exception exception)
                    {
                        _logger.LogError(exception.ToString());
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
                        await DeleteYeastSources(yeast.YeastId,connection,transaction);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.ToString());
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private async Task<IEnumerable<Source>> GetYeastSources(int yeastId, DbConnection connection)
        {
            var sql ="SELECT yeast_id AS Id, social_id AS SocialId, site, url FROM yeast_sources WHERE yeast_id = @YeastId;";
            return await connection.QueryAsync<Source>(sql,new{YeastId = yeastId});
        }

        private async Task InsertYeastSources(Yeast yeast, DbConnection connection, DbTransaction transaction)
        {
            foreach (var source in yeast?.Sources)
            {
                   await connection.ExecuteAsync("INSERT INTO yeast_sources (yeast_id, social_id, site, url) VALUES(@YeastId,@SocialId,@Site,@Url);",new {yeast.YeastId, source.SocialId, source.Site,source.Url},transaction);
            }
        }

        private async Task UpdateYeastSources(Yeast yeast, DbConnection connection, DbTransaction transaction)
        {
            foreach (var source in yeast?.Sources)
            {
                await connection.ExecuteAsync("UPDATE yeast_sources SET site = @Site, url = @Url WHERE yeast_id = @YeastId AND social_id = @SocialId;",new {yeast.YeastId, source.SocialId, source.Site,source.Url},transaction);
            }
        }

        private async Task DeleteYeastSources(int yeastId, DbConnection connection, DbTransaction transaction)
        {
            await connection.ExecuteAsync("DELETE FROM yeast_sources WHERE yeast_id = @YeastId",
                            new { YeastId = yeastId}, transaction);
        }

        private async Task InsertYeastFlavours(Yeast yeast, DbConnection connection, DbTransaction transaction)
        {
            await DeleteYeastFlavours(yeast.YeastId, connection, transaction);
            var flavours = await connection.QueryAsync<Flavour>("SELECT flavour_id as FlavourId, name FROM flavours f");
            foreach (var flavour in yeast?.Flavours)
            {
                var dbFlavour = flavours.FirstOrDefault(f => f.Name == flavour.Flavour.Name);
                if(dbFlavour == null)
                    dbFlavour = await InsertFlavour(flavour.Flavour.Name,connection,transaction);
                await connection.ExecuteAsync("INSERT INTO yeast_flavours (yeast_id, flavour_id) VALUES(@YeastId,@FlavourId);",new {yeast.YeastId, dbFlavour.FlavourId},transaction);
            }
        }

        private async Task<Flavour> InsertFlavour(string name, DbConnection connection, DbTransaction transaction)
        {
            await connection.ExecuteAsync("INSERT INTO flavours (name) VALUES(@Name);", new {Name = name},transaction);
            var result = await connection.QueryAsync<Flavour>("SELECT flavour_id as FlavourId, name FROM flavours f WHERE name = @Name", new {Name = name});
            return result.SingleOrDefault();
        }

        private async Task DeleteYeastFlavours(int yeastId, DbConnection connection, DbTransaction transaction)
        {
            await connection.ExecuteAsync("DELETE FROM yeast_flavours WHERE yeast_id = @YeastId",
                            new { YeastId = yeastId}, transaction);
        }
    }
}
