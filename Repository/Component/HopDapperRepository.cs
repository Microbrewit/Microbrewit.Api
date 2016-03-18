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
using Microsoft.Extensions.OptionsModel;
using Npgsql;

namespace Microbrewit.Api.Repository.Component
{
    public class HopDapperRepository : IHopRepository
    {
       private readonly DatabaseSettings _databaseSettings;
       private readonly ILogger<HopDapperRepository> _logger;
       public HopDapperRepository(IOptions<DatabaseSettings> databaseSettings, ILogger<HopDapperRepository> logger)
       {
           _databaseSettings = databaseSettings.Value;
           _logger = logger;
       }
          
          public async Task<IEnumerable<Hop>> GetAllAsync(int from, int size)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                const string hopFields = @"hop_id AS HopId ,h.name AS Name ,aa_low as AALow, aa_high AS AAHigh, 
                    beta_low AS BetaLow, beta_high AS BetaHigh,notes AS Notes ,flavour_description AS FlavourDescription, custom AS Custom,
                    h.origin_id AS OriginId,purpose AS Purpose, aliases AS Aliases ,total_oil_low AS TotalOilLow,total_oil_high AS TotalOilHigh,
                    bpinene_low AS BpineneLow,bpinene_high AS BpineneHigh,linalool_low AS LinaloolLow,linalool_high AS LinaloolHigh,myrcene_low AS MyrceneLow,
                    myrcene_high As MyrceneHigh,caryophyllene_low AS CaryophylleneLow,caryophyllene_high AS CaryophylleneHigh ,farnesene_low AS FarneseneLow,
                    farnesene_high AS FarneseneHigh,humulene_low As HumuleneLow ,humulene_high AS HumuleneHigh,geraniol_low AS GeraniolLow,
                    geraniol_high AS GeraniolHigh,other_oil_low AS OtherOilLow,other_oil_high AS OtherOilHigh, o.origin_id AS OriginId, o.name";
                var sql = $"SELECT {hopFields} FROM hops h LEFT JOIN origins o ON h.origin_id = o.origin_id LIMIT @Size OFFSET @From";
                var hops = (await connection.QueryAsync<Hop, Origin, Hop>(sql,  (hop, origin) =>
                {
                    hop.Flavours = new List<HopFlavour>();
                    hop.AromaWheel = new List<HopFlavour>();
                    hop.Substituts = new List<Hop>();
                    hop.HopBeerStyles = new List<HopBeerStyle>();
                    hop.Origin = origin;
                    return hop;
                },new {From = from, Size = size}, splitOn: "OriginId")).ToList();

                var hopFlavours = await connection.QueryAsync<HopFlavour>("SELECT flavour_id AS FlavourId, hop_id AS HopId FROM hop_flavours WHERE hop_id = ANY(@Ids)",
                    new { Ids = hops.Select(h => h.HopId).Distinct().ToArray() });

                var flavours = await connection.QueryAsync<Flavour>("SELECT flavour_id AS FlavourId, name FROM Flavours WHERE flavour_id = ANY(@Ids)",
                    new { Ids = hopFlavours.Select(m => m.FlavourId).Distinct().ToList() });

                var substitutes = await connection.QueryAsync<Substitute>("SELECT hop_id AS HopId, substitute_id AS SubstituteId FROM substitutes WHERE hop_id = ANY(@Ids)",
                  new { Ids = hops.Select(h => h.HopId).Distinct().ToList() });

                foreach (var substitute in substitutes)
                {
                    var hop = hops.SingleOrDefault(h => h.HopId == substitute.HopId);
                    var sub = hops.SingleOrDefault(h => h.HopId == substitute.SubstituteId);
                    if (hop == null || sub == null) break;
                    if (hop.Substituts == null)
                        hop.Substituts = new List<Hop>();
                    hop.Substituts.Add(sub);
                }

                foreach (var hopFlavour in hopFlavours)
                {
                    var flavour = flavours.SingleOrDefault(f => f.FlavourId == hopFlavour.FlavourId);
                    if (flavour != null)
                        hopFlavour.Flavour = flavour;
                    var hop = hops.SingleOrDefault(h => h.HopId == hopFlavour.HopId);
                    if (hop == null) break;
                    if (hop.Flavours == null)
                        hop.Flavours = new List<HopFlavour>();
                    hop.Flavours.Add(hopFlavour);
                }

                foreach (var hop in hops)
                {
                    var beerstyleFields =
                   "h.hop_id AS HopId, h.beerstyle_id AS BeerStyleId, bs.beerstyle_id AS BeerStyleId, bs.name, bs.superstyle_id AS SuperStyleId, bs.og_low AS OGLow, " +
                   "bs.og_high AS OGHigh, bs.fg_low AS FGLow, bs.fg_high FGHigh, bs.ibu_low AS IBULow, bs.ibu_high AS IBUHigh, " +
                   "bs.srm_low AS SRMLow, bs.srm_high AS SRMHigh, bs.abv_low AS ABVLow, bs.abv_high AS ABVHigh, bs.comments ";
                    var sql3 =
                        $"SELECT {beerstyleFields} FROM hop_beerstyles h LEFT JOIN beerstyles bs ON h.beerstyle_id = bs.beerstyle_id WHERE h.hop_id = @Id";
                    var hopBeerStyles = await connection.QueryAsync<HopBeerStyle, BeerStyle, HopBeerStyle>(
                        sql3, (h, beerStyle) =>
                        {
                            h.BeerStyle = beerStyle;
                            return h;
                        }, new { Id = hop.HopId }, splitOn: "HopId,BeerStyleId");
                    if (hopBeerStyles != null)
                        hop.HopBeerStyles = hopBeerStyles.ToList();
                }

                return hops.ToList();
            }
        }

        public async Task<Hop> GetSingleAsync(int id)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                const string hopFields = @"h.hop_id AS HopId ,h.name AS Name ,h.aa_low as AALow, h.aa_high AS AAHigh," +
                    "beta_low AS BetaLow, beta_high AS BetaHigh,notes AS Notes ,flavour_description AS FlavourDescription, " +
                    "custom AS Custom,h.origin_id AS OriginId,purpose AS Purpose,aliases AS Aliases ,total_oil_low AS TotalOilLow," +
                    "total_oil_high AS TotalOilHigh,bpinene_low AS BpineneLow,bpinene_high AS BpineneHigh,linalool_low AS LinaloolLow," +
                    "linalool_high AS LinaloolHigh,myrcene_low AS MyrceneLow,myrcene_high As MyrceneHigh,caryophyllene_low AS CaryophylleneLow," +
                    "caryophyllene_high AS CaryophylleneHigh ,farnesene_low AS FarneseneLow,farnesene_high AS FarneseneHigh," +
                    "humulene_low As HumuleneLow ,humulene_high AS HumuleneHigh,geraniol_low AS GeraniolLow,geraniol_high AS GeraniolHigh," +
                    "other_oil_low AS OtherOilLow,other_oil_high AS OtherOilHigh,o.origin_id AS OriginId, o.name";
                var result = await connection.QueryAsync<Hop, Origin, Hop>($"SELECT {hopFields} FROM hops h LEFT JOIN origins o ON h.origin_id = o.origin_id WHERE h.hop_id = @Id", (h, origin) =>
                {
                    h.Origin = origin;
                    h.Flavours = new List<HopFlavour>();
                    h.Substituts = new List<Hop>();
                    h.AromaWheel = new List<HopFlavour>();
                    h.HopBeerStyles = new List<HopBeerStyle>();
                    return h;
                }, new { Id = id }, splitOn: "HopId,OriginId");

                var hop = result.SingleOrDefault();
                if (hop == null) return null;

                var mapping = await connection.QueryAsync<Substitute>("SELECT hop_id AS HopId, substitute_id AS SubstituteId FROM substitutes WHERE hop_id = @id",
                    new { id = hop.HopId });
                var substitutes = await connection.QueryAsync<Hop, Origin, Hop>($"SELECT {hopFields} FROM Hops h LEFT JOIN origins o ON h.origin_id = o.origin_id WHERE hop_id = ANY(@Ids)",
                    (h, origin) =>
                    {
                        h.Origin = origin;
                        return h;
                    },
                    new { Ids = mapping.Select(m => m.SubstituteId).Distinct().ToList() }, splitOn: "OriginId");
                hop.Substituts = substitutes.ToList();

                var hopFlavours = await connection.QueryAsync<HopFlavour>("SELECT flavour_id AS FlavourId, hop_id AS HopId FROM hop_flavours WHERE hop_id = @id",
                  new { id = hop.HopId });
                var aromaWheels = await connection.QueryAsync<HopFlavour>("SELECT flavour_id AS FlavourId, hop_id AS HopId FROM hop_aroma_wheels WHERE hop_id = @id",
                  new { id = hop.HopId });


                var flavours = (await connection.QueryAsync<Flavour>("SELECT flavour_id AS FlavourId, name FROM flavours")).ToList();
                foreach (var hopFlavour in hopFlavours)
                {
                    var flavour = flavours.SingleOrDefault(f => f.FlavourId == hopFlavour.FlavourId);
                    if (flavour != null)
                    {
                        hopFlavour.Flavour = flavour;
                        hop.Flavours.Add(hopFlavour);
                    }
                }
                foreach (var hopFlavour in aromaWheels)
                {
                    var flavour = flavours.SingleOrDefault(f => f.FlavourId == hopFlavour.FlavourId);
                    if (flavour != null)
                    {
                        hopFlavour.Flavour = flavour;
                        hop.AromaWheel.Add(hopFlavour);
                    }
                }

                var beerstyleFields =
                   "h.hop_id AS HopId, h.beerstyle_id AS BeerStyleId, bs.beerstyle_id AS BeerStyleId, bs.name, bs.superstyle_id AS SuperStyleId, bs.og_low AS OGLow, " +
                   "bs.og_high AS OGHigh, bs.fg_low AS FGLow, bs.fg_high FGHigh, bs.ibu_low AS IBULow, bs.ibu_high AS IBUHigh, " +
                   "bs.srm_low AS SRMLow, bs.srm_high AS SRMHigh, bs.abv_low AS ABVLow, bs.abv_high AS ABVHigh, bs.comments ";

                var hopBeerStyles = await connection.QueryAsync<HopBeerStyle, BeerStyle, HopBeerStyle>(
                    $"SELECT {beerstyleFields} FROM hop_beerstyles h LEFT JOIN beerstyles bs ON h.beerstyle_id = bs.beerstyle_id WHERE h.hop_id = @Id", (h, beerStyle) =>
                {
                    h.BeerStyle = beerStyle;
                    return h;
                }, new { Id = id }, splitOn: "HopId,BeerStyleId");
                if (hopBeerStyles != null)
                    hop.HopBeerStyles = hopBeerStyles.ToList();

                return hop;
            }
        }

        public async Task AddAsync(Hop hop)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        const string sql = @"INSERT INTO hops(name, aa_low, aa_high, beta_low, beta_high, notes, flavour_description, custom, 
                            origin_id, purpose, aliases, total_oil_low, total_oil_high, bpinene_low, bpinene_high, linalool_low, linalool_high, myrcene_low, 
                            myrcene_high, caryophyllene_low, caryophyllene_high, farnesene_low, farnesene_high, humulene_low, humulene_high, geraniol_low, geraniol_high, 
                            other_oil_low, other_oil_high) 
                            VALUES(@Name,@AALow,@AAHigh,@BetaLow,@BetaHigh,@Notes,@FlavourDescription,@Custom,@OriginId,
                            @Purpose,@Aliases,@TotalOilLow,@TotalOilHigh,@BPineneLow,@BPineneHigh,@LinaloolLow,@LinaloolHigh,@MyrceneLow,@MyrceneHigh,@CaryophylleneLow,
                            @CaryophylleneHigh,@FarneseneLow,@FarneseneHigh,@HumuleneLow,@HumuleneHigh,@GeraniolLow,@GeraniolHigh,@OtherOilLow,@OtherOilHigh);";

                        var result = await connection.ExecuteAsync(sql, hop, transaction);
                        var hopId = await connection.QueryAsync<int>("SELECT last_value FROM hops_seq;");
                        hop.HopId = hopId.SingleOrDefault();

                        if (hop.Flavours != null)
                        {
                            await connection.ExecuteAsync(
                                @"INSERT INTO hop_flavours(flavour_id, hop_id) VALUES(@FlavourId,@HopId);",
                                hop.Flavours.Select(h => new { h.FlavourId, hop.HopId }),
                                transaction);
                        }
                        if (hop.AromaWheel != null)
                        {
                            await connection.ExecuteAsync(
                                @"INSERT INTO hop_aroma_wheels(flavour_id, hop_id) VALUES(@FlavourId,@HopId);",
                                hop.AromaWheel.Select(h => new { h.FlavourId, hop.HopId }),
                                transaction);
                        }

                        if (hop.Substituts != null)
                        {
                            await connection.ExecuteAsync(
                                @"INSERT INTO substitutes(hop_id,substitute_id) VALUES(@HopId,@SubstituteId);",
                                hop.Substituts.Select(s => new { hop.HopId, SubstituteId = s.HopId }),
                                transaction);
                        }
                        if (hop.HopBeerStyles != null && hop.HopBeerStyles.Any())
                        {
                            await connection.ExecuteAsync(
                                @"INSERT INTO hop_beerstyles(hop_id,beerstyle_id) VALUES(@HopId,@BeerStyleId);",
                                hop.HopBeerStyles.Select(h => new { hop.HopId, h.BeerStyleId }), transaction);
                        }
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

        public async Task<int> UpdateAsync(Hop hop)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        _logger.LogInformation($"Hop name:{hop.Name}");
                        var sql =
                            @"Update hops set name = @Name,aa_low = @AALow,aa_high = @AAHigh, beta_low = @BetaLow,beta_high = @BetaHigh, 
                            notes = @Notes,flavour_description = @FlavourDescription, custom = @custom,origin_id = @OriginId,  
                            purpose = @purpose, aliases = @aliases, total_oil_high = @TotalOilHigh, bpinene_high = @BPineneHigh, linalool_high = @LinaloolHigh,
                            myrcene_high = @myrceneHigh,caryophyllene_high = @CaryophylleneHigh,farnesene_high = @FarneseneHigh,humulene_high = @HumuleneHigh,
                            geraniol_high = @GeraniolHigh,other_oil_high = @OtherOilHigh,total_oil_low = @TotalOilLow,bpinene_low = @BPineneLow,linalool_low = @LinaloolLow,
                            myrcene_low = @myrceneLow,caryophyllene_low = @CaryophylleneLow,farnesene_low = @FarneseneLow,humulene_low = @humuleneLow,geraniol_low = @GeraniolLow,
                            other_oil_low = @OtherOilLow WHERE hop_id = @HopId;";
                        var result = await connection.ExecuteAsync(sql, hop, transaction);
                        await UpdateHopFlavourAsync(connection, transaction, hop);
                        await UpdateHopSubstituteAsync(connection, transaction, hop);
                        await UpdateAromaWheelAsync(connection, transaction, hop);
                        await UpdateHopBeerStyles(connection, transaction, hop);
                        transaction.Commit();
                        return result;
                    }
                    catch (Exception e)
                    {
                        _logger.LogError(e.ToString());
                        throw;
                    }
                }
            }
        }

        private async Task UpdateHopBeerStyles(DbConnection context, DbTransaction transaction, Hop hop)
        {
            var hopBeerStyles = (await context.QueryAsync<HopBeerStyle>(@"SELECT hop_id AS HopId, beerstyle_id AS BeerStyleId FROM hop_beerStyles WHERE hop_id = @HopId", new { hop.HopId },
               transaction)).ToList();

            var toDelete = hopBeerStyles.Where(h => hop.HopBeerStyles.All(f => f.BeerStyleId != h.BeerStyleId));
            await context.ExecuteAsync("DELETE FROM hop_beerStyles WHERE hop_id = @HopId and beerstyle_id = @BeerStyleId;",
                toDelete.Select(h => new { h.HopId, h.BeerStyleId }), transaction);

            var toAdd = hop.HopBeerStyles.Where(h => hopBeerStyles.All(f => f.BeerStyleId != h.BeerStyleId));
            await context.ExecuteAsync(@"INSERT INTO hop_beerStyles(beerstyle_id, hop_id) VALUES(@BeerStyleId,@HopId);", toAdd.Select(h => new { h.HopId, h.BeerStyleId }), transaction);
        }

        private async Task UpdateHopSubstituteAsync(DbConnection connection, DbTransaction transaction, Hop hop)
        {
            //var hopSubstitutes = (await connection.QueryAsync<Substitute>(@"SELECT hop_id AS HopId, substitute_id AS SubstituteId FROM substitutes WHERE hop_id = @HopId",
            //    new { hop.HopId }, transaction)).ToList();

            //var toDelete = hopSubstitutes.Where(h => hop.Substituts.All(s => s.HopId != h.HopId && h.SubstituteId != s.HopId));
            await connection.ExecuteAsync("DELETE FROM substitutes WHERE hop_id = @HopId", hop, transaction);

            //var toAdd = hop.Substituts.Where(h => hopSubstitutes.All(s => s.HopId != h.HopId && h.HopId != s.SubstituteId)).Select(c => new Substitute { HopId = hop.HopId, SubstituteId = c.HopId });

            var add = hop.Substituts.Select(s => new Substitute { SubstituteId = s.HopId, HopId = hop.HopId }).ToList();
            await connection.ExecuteAsync(@"INSERT INTO substitutes(substitute_id, hop_id) VALUES(@SubstituteId,@HopId);", add, transaction);
        }

        private async Task UpdateHopFlavourAsync(DbConnection connection, DbTransaction transaction, Hop hop)
        {
            var hopFlavours = (await connection.QueryAsync<HopFlavour>(@"SELECT flavour_id AS FlavourId, hop_id AS HopId FROM hop_flavours WHERE hop_id = @HopId", new { hop.HopId },
                transaction)).ToList();

            var toDelete = hopFlavours.Where(h => hop.Flavours.All(f => f.FlavourId != h.FlavourId));
            await connection.ExecuteAsync("DELETE FROM hop_flavours WHERE hop_id = @HopId and flavour_id = @FlavourId;",
                toDelete.Select(h => new { h.HopId, h.FlavourId }), transaction);

            var toAdd = hop.Flavours.Where(h => hopFlavours.All(f => f.FlavourId != h.FlavourId));
            await connection.ExecuteAsync(@"INSERT INTO hop_flavours(flavour_id, hop_id) VALUES(@FlavourId,@HopId);", toAdd.Select(h => new { h.HopId, h.FlavourId }), transaction);

        }

        private async Task UpdateAromaWheelAsync(DbConnection connection, DbTransaction transaction, Hop hop)
        {
            var aromaWheels = (await connection.QueryAsync<HopFlavour>(@"SELECT flavour_id AS FlavourId, hop_id AS HopId FROM hop_aroma_wheels WHERE hop_id = @HopId", new { hop.HopId },
                transaction)).ToList();

            var toDelete = aromaWheels.Where(h => hop.AromaWheel.All(f => f.FlavourId != h.FlavourId));
            await connection.ExecuteAsync("DELETE FROM hop_aroma_wheels WHERE hop_id = @HopId and flavour_id = @FlavourId;",
                toDelete.Select(h => new { h.HopId, h.FlavourId }), transaction);

            var toAdd = hop.AromaWheel.Where(h => aromaWheels.All(f => f.FlavourId != h.FlavourId));
            await connection.ExecuteAsync(@"INSERT INTO hop_aroma_wheels(FlavourId, HopId) VALUES(@FlavourId,@HopId);", toAdd.Select(h => new { h.HopId, h.FlavourId }), transaction);

        }

        public async Task RemoveAsync(Hop hop)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var result = await connection.ExecuteAsync("DELETE FROM hops WHERE hop_id = @HopId", new { hop.HopId }, transaction);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public async Task<Flavour> AddFlavourAsync(string name)
        {
           using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var result = await connection.ExecuteAsync(@"INSERT INTO flavours(name) VALUES(@Name)", new {Name = name }, transaction);
                    var flavour = (await connection.QueryAsync<Flavour>("SELECT flavour_id As FlavourId, name FROM flavours WHERE flavour_id = (SELECT last_value FROM flavours_seq);", transaction: transaction)).SingleOrDefault();
                   
                    transaction.Commit();
                    return flavour;

                }
            }
        }

        public async Task<HopForm> GetFormAsync(int id)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                var hopForm = await connection.QueryAsync<HopForm>(@"SELECT hop_form_id AS HopFormId, name FROM hop_forms WHERE hop_form_id = @Id", new { Id = id });
                return hopForm.SingleOrDefault();
            }
        }

        public async Task<IEnumerable<HopForm>> GetHopFormsAsync()
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                return await connection.QueryAsync<HopForm>("SELECT hop_form_id AS HopFormId, name FROM hop_forms");
            }
        }

        public async Task<IEnumerable<Flavour>> GetFlavoursAsync()
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                return await connection.QueryAsync<Flavour>("SELECT flavour_id AS FlavourId, name FROM flavours;");
            }
        }                      
    }
}
