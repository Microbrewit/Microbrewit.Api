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

namespace Microbrewit.Api.Repository.Component
{
    public class FermentableDapperRepository : IFermentableRepository
    {
       private readonly DatabaseSettings _databaseSettings;
       private readonly ILogger<FermentableDapperRepository> _logger;
       public FermentableDapperRepository(IOptions<DatabaseSettings> databaseSettings, ILogger<FermentableDapperRepository> logger)
       {
           _databaseSettings = databaseSettings.Value;
           _logger = logger;

       }
        public async Task<IList<Fermentable>> GetAllAsync(int from, int size)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                var fermentables = await connection.QueryAsync<Fermentable, Supplier, Origin, Fermentable>(
                    "SELECT fermentable_id AS FermentableId, f.name, ebc, lovibond, ppg, note as Note, must_mash AS MustMash, max_in_batch AS MaxInBatch," +
                    "protein, diastatic_power AS DiastaticPower, add_after_boil AS AddAfterBoil, moisture, f.supplier_id AS SupplierId, type," +
                    "custom, super_fermentable_id AS SuperFermentableId,f.created_date AS CreatedDate, updated_date As UpdatedDate, " +
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
                    var flavours = await connection.QueryAsync<Flavour>("SELECT f.flavour_id AS FlavourId, f.name FROM flavours f INNER JOIN fermentable_flavours ff ON f.flavour_id = ff.flavour_id WHERE ff.fermentable_id = @FermentableId",
                    new {fermentable.FermentableId});
                    if(flavours != null)
                        fermentable.Flavours = flavours;
                    
                    fermentable.Sources = await GetFermentableSource(fermentable.FermentableId);
                }
                return fermentables.ToList();
            }
        }

        public async Task<Fermentable> GetSingleAsync(int id)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                var fermentableSql = "SELECT fermentable_id AS FermentableId, f.name, ebc, lovibond, ppg,note AS Note, must_mash AS MustMash, max_in_batch AS MaxInBatch," +
                    "protein, diastatic_power AS DiastaticPower, add_after_boil as AddAfterBoil,moisture, f.supplier_id AS SupplierId, type," +
                    "custom, super_fermentable_id AS SuperFermentableId,f.created_date AS CreatedDate, updated_date As UpdatedDate, " +
                    "s.supplier_id AS SupplierId, s.name, s.origin_id AS OriginId, o.origin_id AS OriginId, o.name " +
                    "FROM fermentables f " +
                    "LEFT JOIN suppliers s ON f.supplier_id = s.supplier_id " +
                    "LEFT JOIN origins o ON s.origin_id = o.origin_id " + 
                    "WHERE f.fermentable_id = @FermentableId;";
                    
                var fermentables = await connection.QueryAsync<Fermentable, Supplier, Origin, Fermentable>(
                    fermentableSql,
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
                _logger.LogInformation(select + subWhere);
                var flavours = await connection.QueryAsync<Flavour>("SELECT f.flavour_id AS FlavourId, f.name FROM flavours f INNER JOIN fermentable_flavours ff ON f.flavour_id = ff.flavour_id WHERE ff.fermentable_id = @FermentableId",
                    new {fermentable.FermentableId});
                    if(flavours != null)
                        fermentable.Flavours = flavours;
                fermentable.Sources = await GetFermentableSource(fermentable.FermentableId);
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
                       fermentable.UpdatedDate = DateTime.Now;
                       fermentable.CreatedDate = DateTime.Now;                       
                       var result =
                            await connection.ExecuteAsync("INSERT INTO fermentables(name,super_fermentable_id,EBC,Lovibond,PPG,supplier_id,Type,Custom,created_date,updated_date, note, must_mash, max_in_batch, protein, diastatic_power, add_after_boil, moisture) " +
                                               "VALUES(@Name,@SuperFermentableId,@EBC,@Lovibond,@PPG,@SupplierId,@Type,@Custom,@CreatedDate,@UpdatedDate,@Note,@MustMash,@MaxInBatch, @Protein, @DiastaticPower, @AddAfterBoil, @Moisture);", fermentable, transaction);
                        var fermentableId = await connection.QueryAsync<int>("SELECT last_value FROM fermentables_seq;");
                        fermentable.FermentableId = fermentableId.SingleOrDefault();
                        await AddFermentableFlavours(fermentable,connection,transaction);
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

        private async Task AddFermentableFlavours(Fermentable fermentable, DbConnection connection, DbTransaction transaction)
        {
            await SetCorrectFlavourId(fermentable.Flavours, connection, transaction);
            await InsertFermentableFlavours(fermentable, connection, transaction);
            await AddFermentableSources(fermentable,connection,transaction);
        }

        private static async Task InsertFermentableFlavours(Fermentable fermentable, DbConnection connection, DbTransaction transaction)
        {
            await connection.ExecuteAsync("INSERT INTO fermentable_flavours (flavour_id,fermentable_id) VALUES(@FlavourId,@FermentableId)",
                                fermentable.Flavours.Select(f => new { f.FlavourId, fermentable.FermentableId }), transaction);
        }

        private async Task SetCorrectFlavourId(IEnumerable<Flavour> flavours, DbConnection connection, DbTransaction transaction)
        {
            foreach (var flavour in flavours)
            {                       
                var dbFlavour = await connection.QueryAsync<Flavour>("SELECT f.flavour_id AS FlavourId, f.name FROM flavours f WHERE f.name = @Name;",new {flavour.Name},transaction);
                if(dbFlavour.FirstOrDefault() == null)
                {
                       var id = await connection.QueryAsync<int>("INSERT INTO flavours(name) VALUES(@Name); SELECT last_value FROM flavours_seq; ",new {flavour.Name},transaction);
                       flavour.FlavourId = id.Single();
                }  
                else 
                    flavour.FlavourId = dbFlavour.First().FlavourId; 
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
                        fermentable.UpdatedDate = DateTime.Now;
                        var result = await connection.ExecuteAsync(
                                        "Update Fermentables set Name = @Name,super_fermentable_id = @SuperFermentableId,EBC = @EBC," + 
                                        "Lovibond = @Lovibond,PPG= @PPG,supplier_id = @SupplierId,Type = @Type, Custom = @Custom, " +
                                        "updated_date = @UpdatedDate, note = @Note, must_mash = @MustMash, max_in_batch = @MaxInBatch" +
                                        "protein = @Protein, diastatic_power = @DiastaticPower, add_after_boil = @AddAfterBoil, moisture = @Moisture " +
                                        "WHERE fermentable_id = @FermentableId;",
                                              fermentable, transaction);
                        await UpdateFermantableFlavours(fermentable,connection,transaction);
                        await UpdateFermentableSources(fermentable,connection,transaction);
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

        private async Task UpdateFermantableFlavours(Fermentable fermentable, DbConnection connection, DbTransaction transaction)
        {
            await connection.ExecuteAsync("DELETE FROM fermentable_flavours WHERE fermentable_id = @FermentableId", new {fermentable.FermentableId});          
            await SetCorrectFlavourId(fermentable.Flavours,connection,transaction);                
            await InsertFermentableFlavours(fermentable,connection,transaction);
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
                            await DeleteFermentableSources(fermentable.FermentableId, connection,transaction);
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

        private async Task<IEnumerable<Source>> GetFermentableSource(int fermentableId)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                var sql ="SELECT fermentable_id AS Id, social_id AS SocialId, site, url FROM fermentable_sources WHERE fermentable_id = @FermentableId;";
                return await connection.QueryAsync<Source>(sql,new{FermentableId = fermentableId});   
            }
        }

          private async Task AddFermentableSources(Fermentable fermentable, DbConnection connection, DbTransaction transaction)
        {
            foreach (var source in fermentable?.Sources)
            {
                   await connection.ExecuteAsync("INSERT INTO fermentable_sources (fermentable_id, social_id, site, url) VALUE(@FermentableId,@SocialId,@Site,@Url);",new {fermentable.FermentableId, source.SocialId, source.Site,source.Url},transaction);
            }
        }

        private async Task UpdateFermentableSources(Fermentable fermentable, DbConnection connection, DbTransaction transaction)
        {
            foreach (var source in fermentable?.Sources)
            {
                await connection.ExecuteAsync("UPDATE fermentable_sources SET site = @Site, url = @Url WHERE fermentable_id = @FermentableId AND social_id = @SocialId;",new {fermentable.FermentableId, source.SocialId, source.Site,source.Url},transaction);
            }
        }

        private async Task DeleteFermentableSources(int fermentableId, DbConnection connection, DbTransaction transaction)
        {
            await connection.ExecuteAsync("DELETE FROM fermentable_sources WHERE fermentable_id = @FermentableId",
                            new { FermentableId = fermentableId}, transaction);
        }
    }
}
