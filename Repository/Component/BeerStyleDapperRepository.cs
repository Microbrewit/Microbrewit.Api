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
    public class BeerStyleDapperRepository : IBeerStyleRepository
    {
        private DatabaseSettings _databaseSettings; 
        public BeerStyleDapperRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings.Value;
        }
         public async Task<IList<BeerStyle>> GetAllAsync(int from, int size)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                var fields =
                    " bs.beerstyle_id AS BeerStyleId, bs.name, bs.superstyle_id AS SuperStyleId, bs.og_low AS OGLow , " +
                    "bs.og_high AS OGHigh, bs.fg_low AS FGLow, bs.fg_high FGHigh, bs.ibu_low AS IBULow, bs.ibu_high AS IBUHigh, " +
                    "bs.srm_low AS SRMLow, bs.srm_high AS SRMHigh, bs.abv_low AS ABVLow, bs.abv_high AS ABVHigh, bs.comments ";
                var sql = $"SELECT {fields} FROM beerstyles bs  LIMIT @Size OFFSET @From;";
                var beerStyles = await connection.QueryAsync<BeerStyle>(sql, new {From = from, Size = size});
                foreach (var beerStyle in beerStyles)
                {
                    if (beerStyle.SuperStyleId != null)
                    {
                        var superStyle =
                            await connection.QueryAsync<BeerStyle>($"SELECT {fields} FROM beerstyles bs WHERE beerstyle_id = @SuperStyleId;",
                                new { beerStyle.SuperStyleId });
                        beerStyle.SuperStyle = superStyle.SingleOrDefault();
                    }
                    var subStyles =
                        await connection.QueryAsync<BeerStyle>($"SELECT {fields} FROM BeerStyles bs WHERE superstyle_id = @BeerStyleId;",
                            new { beerStyle.BeerStyleId });
                    beerStyle.SubStyles = subStyles.ToList();
                    const string hopFields = @"hbs.hop_id AS HopId,hbs.beerstyle_id, h.hop_id AS HopId, h.name AS Name ,aa_low as AALow, aa_high AS AAHigh, beta_low AS BetaLow, beta_high AS BetaHigh," +
                                       "notes AS Notes ,flavour_description AS FlavourDescription, custom AS Custom,h.origin_id AS OriginId,purpose AS Purpose," +
                                        "aliases AS Aliases ,total_oil_low AS TotalOilLow,total_oil_high AS TotalOilHigh,bpinene_low AS BpineneLow,bpinene_high AS BpineneHigh," +
                                        "linalool_low AS LinaloolLow,linalool_high AS LinaloolHigh,myrcene_low AS MyrceneLow,myrcene_high As MyrceneHigh," +
                                        "caryophyllene_low AS CaryophylleneLow,caryophyllene_high AS CaryophylleneHigh ,farnesene_low AS FarneseneLow," +
                                        "farnesene_high AS FarneseneHigh,humulene_low As HumuleneLow ,humulene_high AS HumuleneHigh,geraniol_low AS GeraniolLow," +
                                        "geraniol_high AS GeraniolHigh,other_oil_low AS OtherOilLow,other_oil_high AS OtherOilHigh";
                    var hopBeerStyleSql =
                        $"SELECT {hopFields} FROM hop_beerstyles hbs LEFT JOIN Hops h ON hbs.hop_Id = h.hop_id WHERE hbs.beerstyle_id = @Id";
                    var hopBeerStyles = await connection.QueryAsync<HopBeerStyle, Hop, HopBeerStyle>(
                      hopBeerStyleSql, (hb, hop) =>
                      {
                          hb.Hop = hop;
                          return hb;
                      }, new { Id = beerStyle.BeerStyleId }, splitOn: "HopId");
                        if (hopBeerStyles != null)
                            beerStyle.HopBeerStyles = hopBeerStyles.ToList();
                }
                return beerStyles.ToList();
            }
        }

        public async Task<BeerStyle> GetSingleAsync(int id)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                var fields =
                       " bs.beerstyle_id AS BeerStyleId, bs.name, bs.superstyle_id AS SuperStyleId, bs.og_low AS OGLow , " +
                       "bs.og_high AS OGHigh, bs.fg_low AS FGLow, bs.fg_high FGHigh, bs.ibu_low AS IBULow, bs.ibu_high AS IBUHigh, " +
                       "bs.srm_low AS SRMLow, bs.srm_high AS SRMHigh, bs.abv_low AS ABVLow, bs.abv_high AS ABVHigh, bs.comments ";
                var beerStyles = await connection.QueryAsync<BeerStyle>($"SELECT {fields} FROM BeerStyles bs WHERE beerstyle_id = @BeerStyleId;", new { BeerStyleId = id });
                var beerStyle = beerStyles.SingleOrDefault();
                if (beerStyle == null) return null;
                if (beerStyle.SuperStyleId != null)
                {
                    var superStyle =
                        await connection.QueryAsync<BeerStyle>($"SELECT {fields} FROM beerstyles bs WHERE beerstyle_id = @SuperStyleId;",
                            new { beerStyle.SuperStyleId });
                    beerStyle.SuperStyle = superStyle.SingleOrDefault();
                }
                var subStyles =
                    await connection.QueryAsync<BeerStyle>($"SELECT {fields} FROM beerstyles bs WHERE superstyle_id = @BeerStyleId;",
                        new { beerStyle.BeerStyleId });
                beerStyle.SubStyles = subStyles.ToList();
                const string hopFields = @"hbs.hop_id AS HopId,hbs.beerstyle_id, h.hop_id AS HopId, h.name AS Name ,aa_low as AALow, aa_high AS AAHigh, beta_low AS BetaLow, beta_high AS BetaHigh," +
                                        "notes AS Notes ,flavour_description AS FlavourDescription, custom AS Custom,h.origin_id AS OriginId,purpose AS Purpose," +
                                         "aliases AS Aliases ,total_oil_low AS TotalOilLow,total_oil_high AS TotalOilHigh,bpinene_low AS BpineneLow,bpinene_high AS BpineneHigh," +
                                         "linalool_low AS LinaloolLow,linalool_high AS LinaloolHigh,myrcene_low AS MyrceneLow,myrcene_high As MyrceneHigh," +
                                         "caryophyllene_low AS CaryophylleneLow,caryophyllene_high AS CaryophylleneHigh ,farnesene_low AS FarneseneLow," +
                                         "farnesene_high AS FarneseneHigh,humulene_low As HumuleneLow ,humulene_high AS HumuleneHigh,geraniol_low AS GeraniolLow," +
                                         "geraniol_high AS GeraniolHigh,other_oil_low AS OtherOilLow,other_oil_high AS OtherOilHigh";
                var hopBeerStyles = await connection.QueryAsync<HopBeerStyle, Hop, HopBeerStyle>(
                   $"SELECT {hopFields} FROM hop_beerstyles hbs LEFT JOIN hops h ON hbs.hop_id = h.hop_id WHERE hbs.beerStyle_id = @Id", (hb, hop) =>
                   {
                       hb.Hop = hop;
                       return hb;
                   }, new { Id = beerStyle.BeerStyleId }, splitOn: "HopId");
                if (hopBeerStyles != null)
                    beerStyle.HopBeerStyles = hopBeerStyles.ToList();
                return beerStyle;
            }
        }

        public async Task AddAsync(BeerStyle beerStyle)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var result =
                            await
                                connection.ExecuteAsync(
                                    "INSERT INTO beerstyles(name,superstyle_id,og_low,og_high,fg_low,fg_high,ibu_low,ibu_high,srm_low,srm_high,abv_low,abv_high,comments) " +
                                    "VALUES(@Name,@SuperStyleId,@OGLow,@OGHigh,@FGLow,@FGHigh,@IBULow,@IBUHigh,@SRMLow,@SRMHigh,@ABVLow,@ABVHigh,@Comments);",
                                    beerStyle, transaction);
                        var beerStyleId = await connection.QueryAsync<int>("SELECT last_value FROM beerstyles_seq;");
                        beerStyle.BeerStyleId = beerStyleId.SingleOrDefault();
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

        public async Task<int> UpdateAsync(BeerStyle beerStyle)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var result = await connection.ExecuteAsync("UPDATE BeerStyles set Name = @Name,superstyle_id = @SuperStyleId,og_low = @OGLow,og_high = @OGHigh," +
                                        "fg_low = @FGLow,fg_high = @FGHigh,ibu_low = @IBULow,ibu_high = @IBUHigh, srm_low = @SRMLow, srm_high = @SRMHigh," +
                                        "abv_low = @ABVLow, abv_high = @ABVHigh, comments = @Comments " +
                                        "WHERE beerstyle_id = @BeerStyleId",
                                        beerStyle, transaction);
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

        public async Task RemoveAsync(BeerStyle beerStyle)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await connection.ExecuteAsync("DELETE FROM beerStyles WHERE beerStyle_id = @BeerStyleId",
                            new { beerStyle.BeerStyleId }, transaction);
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
