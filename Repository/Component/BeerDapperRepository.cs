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
    public class BeerDapperRepository : IBeerRepository
    {
        private DatabaseSettings _databaseSettings;
        const string BeerFields =  "b.beer_id AS BeerId, b.name, b.beerstyle_id AS BeerStyleId, b.created_date AS CreatedDate, b.updated_date AS UpdatedDate,b.is_commercial AS IsCommercial, b.fork_of_id AS ForkOfId," +
                   "bs.beerstyle_id AS BeerStyleId, bs.name, bs.superstyle_id AS SuperStyleId, bs.og_low AS OGLow , bs.og_high AS OGHigh, bs.fg_low AS FGLow, bs.fg_high FGHigh," +
                   "bs.ibu_low AS IBULow, bs.ibu_high AS IBUHigh, bs.srm_low AS SRMLow, bs.srm_high AS SRMHig, bs.abv_low AS ABVLow, bs.abv_high AS ABVHigh, bs.comments, " +
                   "r.recipe_id AS RecipeId, r.volume, r.notes, r.og, r.fg, r.efficiency, r.total_boil_time AS TotalBoilTime," +
                   "s.srm_id AS SrmId, s.standard, s.mosher, s.daniels, s.morey, " +
                   "a.abv_id AS AbvId, a.standard, a.miller, a.advanced, a.advanced_alternative AS AdvancedAlternative, a.simple, a.simple_alternative AS SimpleAlternative," +
                   "i.ibu_id AS IbuId, i.standard, i.tinseth, i.rager ";
        public BeerDapperRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings.Value;
        }
        
      
      public async Task<IEnumerable<Beer>> GetAllAsync(int from, int size)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();             
                var sql = $"SELECT {BeerFields} FROM Beers AS b " +
                          "LEFT JOIN beerstyles bs ON bs.beerstyle_id = b.beerstyle_id " +
                          "LEFT JOIN recipes r ON r.recipe_Id = b.beer_id " +
                          "LEFT JOIN srms s ON s.srm_id = b.beer_id " +
                          "LEFT JOIN abvs a ON a.abv_id = b.beer_id " +
                          "LEFT JOIN ibus i ON i.ibu_id = b.beer_id " +
                          "ORDER BY created_date DESC LIMIT @Size OFFSET @From";
                var beers = await connection.QueryAsync<Beer, BeerStyle, Recipe, SRM, ABV, IBU, Beer>(
                     sql, (beer, beerStyle, recipe, srm, abv, ibu) =>
                     {
                         if (beerStyle != null)
                             beer.BeerStyle = beerStyle;
                         if (recipe != null)
                             beer.Recipe = recipe;
                         if (srm != null)
                             beer.SRM = srm;
                         if (abv != null)
                             beer.ABV = abv;
                         if (ibu != null)
                             beer.IBU = ibu;
                         return beer;
                     }, new { From = from, Size = size },
                     splitOn: "BeerStyleId,RecipeId,SrmId,AbvId,IbuId"
                     );

                foreach (var beer in beers)
                {
                    await GetForkOf(connection, beer);
                    await GetForks(connection, beer);
                    await GetBreweries(connection, beer);
                    await GetBrewers(connection, beer);
                    if (beer.Recipe != null)
                    {
                        await GetRecipeStepsAsync(connection, beer.Recipe);
                    }
                }

                return beers.ToList();
            }
        }

        public async Task<Beer> GetSingleAsync(int id)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
               
                var beers = await connection.QueryAsync<Beer, BeerStyle, Recipe, SRM, ABV, IBU, Beer>(
                    $"SELECT {BeerFields} FROM Beers AS b " +
                          "LEFT JOIN beerstyles bs ON bs.beerstyle_id = b.beerstyle_id " +
                          "LEFT JOIN recipes r ON r.recipe_Id = b.beer_id " +
                          "LEFT JOIN srms s ON s.srm_id = b.beer_id " +
                          "LEFT JOIN abvs a ON a.abv_id = b.beer_id " +
                          "LEFT JOIN ibus i ON i.ibu_id = b.beer_id " +
                    "WHERE beer_id = @BeerId;"
                    , (b, beerStyle, recipe, srm, abv, ibu) =>
                    {
                        if (beerStyle != null)
                            b.BeerStyle = beerStyle;
                        if (recipe != null)
                            b.Recipe = recipe;
                        if (srm != null)
                            b.SRM = srm;
                        if (abv != null)
                            b.ABV = abv;
                        if (ibu != null)
                            b.IBU = ibu;
                        return b;
                    },
                    new { BeerId = id },
                    splitOn: "BeerStyleId,RecipeId,SrmId,AbvId,IbuId"
                    );
                var beer = beers.SingleOrDefault();
                if (beer == null) return null;
                await GetForkOf(connection, beer);
                await GetForks(connection, beer);
                await GetBreweries(connection, beer);
                await GetBrewers(connection, beer);
                if (beer.Recipe != null)
                {
                    await GetRecipeStepsAsync(connection, beer.Recipe);
                }
                return beer;
            }
        }

        public async Task AddAsync(Beer beer)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        beer.CreatedDate = DateTime.Now;
                        beer.UpdatedDate = DateTime.Now;
                        var result = await connection.ExecuteAsync(
                            "INSERT INTO Beers(name,beerstyle_id,created_date,updated_date,fork_of_id,is_commercial) " +
                            "VALUES(@Name,@BeerStyleId,@CreatedDate,@UpdatedDate,@ForkeOfId,@IsCommercial);", beer, transaction);
                        var beerId = await connection.QueryAsync<int>("SELECT last_value FROM beers_seq");
                        beer.BeerId = beerId.SingleOrDefault();

                        beer.SRM.SrmId = beer.BeerId;
                        await connection.ExecuteAsync("INSERT INTO srms(srm_id,standard,mosher,daniels,morey) VALUES(@SrmId,@Standard,@Mosher,@Daniels,@Morey);",
                            new { beer.SRM.SrmId, beer.SRM.Standard, beer.SRM.Mosher, beer.SRM.Daniels, beer.SRM.Morey }, transaction);

                        beer.ABV.AbvId = beer.BeerId;
                        await connection.ExecuteAsync("INSERT INTO abvs(abv_id,standard,miller,advanced,advanced_alternative,simple,simple_alternative) " +
                                        "VALUES(@AbvId,@Standard,@Miller,@Advanced,@AdvancedAlternative,@Simple,@AlternativeSimple);",
                            new { AbvId = beer.BeerId, beer.ABV.Standard, beer.ABV.Miller, beer.ABV.Advanced, beer.ABV.AdvancedAlternative, beer.ABV.Simple, beer.ABV.AlternativeSimple }, transaction);

                        beer.IBU.IbuId = beer.BeerId;
                        await connection.ExecuteAsync("INSERT INTO ibus(ibu_id,standard,tinseth,rager) " +
                                        "VALUES(@IbuId,@Standard,@Tinseth,@Rager);",
                            new { IbuId = beer.BeerId, beer.IBU.Standard, beer.IBU.Tinseth, beer.IBU.Rager }, transaction);

                        AddBrewers(beer, connection, transaction);

                        if (beer.Recipe != null)
                        {
                            beer.Recipe.RecipeId = beer.BeerId;
                            await AddRecipeAsync(beer.Recipe, connection, transaction);
                        }
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

        private void AddBrewers(Beer beer, DbConnection connection, DbTransaction transaction)
        {
            //TODO: FIX THIS.
            if (beer.Brewers == null || !beer.Brewers.Any()) return;
            //foreach (var userBeer in beer.Brewers)
            //{
            //    userBeer.Username = userBeer.Username.ToLower();
            //}
            var brewers = beer.Brewers.Distinct();
            var distinct = brewers.Select(b => new { beer.BeerId, b.UserId, b.Confirmed });
            connection.Execute("INSERT UserBeers(BeerId,Username,Confirmed) VALUES(@BeerId,@Username,@Confirmed);", distinct, transaction);
        }

        public async Task<int> UpdateAsync(Beer beer)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        beer.UpdatedDate = DateTime.Now;
                        var result = await connection.ExecuteAsync(
                            "UPDATE beers set name = @Name, beerstyle_id = @BeerStyleId, updated_date = @UpdatedDate, fork_of_id = @ForkeOfId, is_commercial = IsCommercial " + 
                            "WHERE beer_id = @BeerId;",
                            beer, transaction);

                        await connection.ExecuteAsync("UPDATE srms set standard = @Standard, mosher = @Mosher, daniels = @Daniels, morey = @Morey " +
                                        "WHERE srm_id = @SrmId;",
                            new { SrmId = beer.BeerId, beer.SRM.Standard, beer.SRM.Mosher, beer.SRM.Daniels, beer.SRM.Morey }, transaction);

                        await connection.ExecuteAsync("UPDATE abvs set standard = @Standard, miller = @Miller, advanced = @Advanced, advanced_alternative = @AdvancedAlternative, simple = @Simple, simple_alternative = @AlternativeSimple " +
                                        "WHERE abv_Id = @AbvId;",
                           new { AbvId = beer.BeerId, beer.ABV.Standard, beer.ABV.Miller, beer.ABV.Advanced, beer.ABV.AdvancedAlternative, beer.ABV.Simple, beer.ABV.AlternativeSimple }, transaction);

                        await connection.ExecuteAsync("UPDATE ibus set standard = @Standard,tinseth = @Tinseth,rager = @Rager WHERE ibu_Id = @IbuId;",
                            new { IbuId = beer.BeerId, beer.IBU.Standard, beer.IBU.Tinseth, beer.IBU.Rager }, transaction);

                        beer.Recipe.RecipeId = beer.BeerId;
                        await UpdateRecipe(connection, transaction, beer.Recipe);
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

        public Task RemoveAsync(Beer beer)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Beer>> GetLastAsync(int @from, int size)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {    
                var beers =
                    await
                        connection.QueryAsync<Beer, BeerStyle, Recipe, SRM, ABV, IBU, Beer>(
                           $"SELECT {BeerFields} FROM Beers AS b " +
                          "LEFT JOIN beerstyles bs ON bs.beerstyle_id = b.beerstyle_id " +
                          "LEFT JOIN recipes r ON r.recipe_Id = b.beer_id " +
                          "LEFT JOIN srms s ON s.srm_id = b.beer_id " +
                          "LEFT JOIN abvs a ON a.abv_id = b.beer_id " +
                          "LEFT JOIN ibus i ON i.ibu_id = b.beer_id " +
                          "ORDER BY created_date DESC LIMIT @Size OFFSET @From",
                             (beer, beerStyle, recipe, srm, abv, ibu) =>
                             {
                                 if (beerStyle != null)
                                     beer.BeerStyle = beerStyle;
                                 if (recipe != null)
                                     beer.Recipe = recipe;
                                 if (srm != null)
                                     beer.SRM = srm;
                                 if (abv != null)
                                     beer.ABV = abv;
                                 if (ibu != null)
                                     beer.IBU = ibu;
                                 return beer;
                             }, new { From = from, Size = size }, splitOn: "BeerStyleId,RecipeId,SrmId,AbvId,IbuId");

                foreach (var beer in beers)
                {
                    await GetForkOf(connection, beer);
                    await GetForks(connection, beer);
                    await GetBreweries(connection, beer);
                    await GetBrewers(connection, beer);
                    if (beer.Recipe != null)
                    {
                        await GetRecipeStepsAsync(connection, beer.Recipe);
                    }
                }

                return beers.ToList();
            }
        }

        public async Task<IEnumerable<Beer>> GetAllUserBeerAsync(string userId)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
            
                var sql = $"SELECT {BeerFields} FROM Beers AS b " +
                           "INNER JOIN user_beers ub ON ub.beer_id = b.beer_id " +
                           "LEFT JOIN beerstyles bs ON bs.beerstyle_id = b.beerstyle_id " +
                          "LEFT JOIN recipes r ON r.recipe_Id = b.beer_id " +
                          "LEFT JOIN srms s ON s.srm_id = b.beer_id " +
                          "LEFT JOIN abvs a ON a.abv_id = b.beer_id " +
                          "LEFT JOIN ibus i ON i.ibu_id = b.beer_id " +
                          "WHERE ub.user_id = @UserId; ";
                var beers =
                    await
                        connection.QueryAsync<Beer, BeerStyle, Recipe, SRM, ABV, IBU, Beer>(
                            sql,
                             (beer, beerStyle, recipe, srm, abv, ibu) =>
                             {
                                 if (beerStyle != null)
                                     beer.BeerStyle = beerStyle;
                                 if (recipe != null)
                                     beer.Recipe = recipe;
                                 if (srm != null)
                                     beer.SRM = srm;
                                 if (abv != null)
                                     beer.ABV = abv;
                                 if (ibu != null)
                                     beer.IBU = ibu;
                                 return beer;
                             }, new { UserId = userId }, splitOn: "BeerStyleId,RecipeId,SrmId,AbvId,IbuId");

                foreach (var beer in beers)
                {
                    await GetForkOf(connection, beer);
                    await GetForks(connection, beer);
                    await GetBreweries(connection, beer);
                    await GetBrewers(connection, beer);
                    if (beer.Recipe != null)
                    {
                        await GetRecipeStepsAsync(connection, beer.Recipe);
                    }
                }
                return beers;
            }
        }

        public async Task<IEnumerable<Beer>>  GetAllBreweryBeersAsync(int breweryId)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                var sql = $"SELECT {BeerFields} FROM Beers AS b " +
                          "INNER JOIN brewery_beers bb ON bb.beer_id = b.beer_id " +
                          "LEFT JOIN beerstyles bs ON bs.beerstyle_id = b.beerstyle_id " +
                          "LEFT JOIN recipes r ON r.recipe_Id = b.beer_id " +
                          "LEFT JOIN srms s ON s.srm_id = b.beer_id " +
                          "LEFT JOIN abvs a ON a.abv_id = b.beer_id " +
                          "LEFT JOIN ibus i ON i.ibu_id = b.beer_id " +
                          "WHERE bb.brewery_id = @BreweryId; ";
                var beers =
                    await connection.QueryAsync<Beer, BeerStyle, Recipe, SRM, ABV, IBU, Beer>(
                        sql,(beer, beerStyle, recipe, srm, abv, ibu) =>
                        {
                            if (beerStyle != null)
                                beer.BeerStyle = beerStyle;
                            if (recipe != null)
                                beer.Recipe = recipe;
                            if (srm != null)
                                beer.SRM = srm;
                            if (abv != null)
                                beer.ABV = abv;
                            if (ibu != null)
                                beer.IBU = ibu;
                            return beer;
                        }, new { BreweryId = breweryId }, splitOn: "BeerStyleId,RecipeId,SrmId,AbvId,IbuId");

                foreach (var beer in beers)
                {
                    await GetForkOf(connection, beer);
                    await GetForks(connection, beer);
                    await GetBreweries(connection, beer);
                    await GetBrewers(connection, beer);
                    if (beer.Recipe != null)
                    {
                        await GetRecipeStepsAsync(connection, beer.Recipe);
                    }
                }
                return beers;
            }
        }

        private async Task GetRecipeStepsAsync(DbConnection connection, Recipe recipe)
        {
            await GetMashSteps(connection, recipe);
            await GetBoilSteps(connection, recipe);
            await GetFermentationStepsAsync(connection, recipe);
            await GetSpargeStep(connection, recipe);
        }

        private async Task GetSpargeStep(DbConnection connection, Recipe recipe)
        {
            var fields =
                "s.recipe_id AS RecipeId, s.step_number AS StepNumber, s.temperature, s.amount, s.notes, s.type ";
            var spargeSteps = await connection.QueryAsync<SpargeStep>(
                $"SELECT {fields} FROM sparge_steps s WHERE recipe_id = @RecipeId;", new { recipe.RecipeId });
            foreach (var spargeStep in spargeSteps)
            {
                const string hopFields = @"s.recipe_id AS RecipeId, s.step_number AS StepNumber, s.hop_id AS HopId, s.aa_value AS AAValue, s.amount, s.hop_form_id AS HopFormId," +
                      "h.hop_id AS HopId ,h.name AS Name ,h.aa_low as AALow, h.aa_high AS AAHigh," +
                      "beta_low AS BetaLow, beta_high AS BetaHigh,notes AS Notes ,flavour_description AS FlavourDescription, " +
                      "custom AS Custom,h.origin_id AS OriginId,purpose AS Purpose,aliases AS Aliases ,total_oil_low AS TotalOilLow," +
                      "total_oil_high AS TotalOilHigh,bpinene_low AS BpineneLow,bpinene_high AS BpineneHigh,linalool_low AS LinaloolLow," +
                      "linalool_high AS LinaloolHigh,myrcene_low AS MyrceneLow,myrcene_high As MyrceneHigh,caryophyllene_low AS CaryophylleneLow," +
                      "caryophyllene_high AS CaryophylleneHigh ,farnesene_low AS FarneseneLow,farnesene_high AS FarneseneHigh," +
                      "humulene_low As HumuleneLow ,humulene_high AS HumuleneHigh,geraniol_low AS GeraniolLow,geraniol_high AS GeraniolHigh," +
                      "other_oil_low AS OtherOilLow,other_oil_high AS OtherOilHigh,hf.hop_form_id AS HopFormId, hf.name, o.origin_id AS OriginId, o.name";
                var spargeStepHops = await connection.QueryAsync<SpargeStepHop, Hop,Origin, SpargeStepHop>(
                 $"SELECT {hopFields} FROM SpargeStepHops s " +
                 "LEFT JOIN Hops h ON s.hop_id = h.hop_id " +
                 "LEFT JOIN origins 0 ON h.origin_id = o.origin_id" +
                 "WHERE recipe_id = @RecipeId and StepNumber = @StepNumber;", (spargeStepHop, hop,origin) =>
                 {
                     if (hop != null)
                         hop.Origin = origin;
                     spargeStepHop.Hop = hop;
                     return spargeStepHop;
                 }, new { recipe.RecipeId, spargeStep.StepNumber }, splitOn: "HopId,OriginId");
                spargeStep.Hops = spargeStepHops.ToList();
            }
            recipe.SpargeSteps = spargeSteps.ToList();
        }

        private async Task GetFermentationStepsAsync(DbConnection connection, Recipe recipe)
        {
            var fermentationSteps = connection.Query<FermentationStep>(
               "SELECT recipe_id AS RecipeId, step_number AS StepNumber, length, volume, notes FROM  fermentation_steps WHERE recipe_id = @RecipeId;", new { recipe.RecipeId });
            foreach (var fermentationStep in fermentationSteps)
            {
                var fermentablesField = "fs.recipe_id AS RecipeId, fs.step_number AS StepNumber, fs.fermentable_id AS FermentableId, " +
                          "fs.amount, fs.lovibond, fs.ppg, f.fermentable_id AS FermentableId, f.name, f.ebc, f.lovibond, " +
                          "f.ppg, f.supplier_id AS SupplierId, f.type, f.custom, f.super_fermentable_id AS SuperFermentableId," +
                          "s.supplier_id AS SupplierId, s.name, s.origin_id AS OriginId, o.origin_id AS OriginId, o.name";
                var fermentationStepFermentables = await connection.QueryAsync<FermentationStepFermentable, Fermentable, Supplier, Origin, FermentationStepFermentable>(
                    $"SELECT {fermentablesField} FROM fermentation_step_fermentables fs " +
                    "LEFT JOIN fermentables f ON fs.fermentable_id = f.fermentable_id " +
                    "LEFT JOIN suppliers s ON f.supplier_id = s.supplier_id " +
                    "LEFT JOIN origins o ON o.origin_id = s.origin_id " +
                    "WHERE recipe_id = @RecipeId and step_number = @StepNumber;", (fermentationStepFermentable, fermentable, supplier, origin) =>
                    {
                        fermentationStepFermentable.Fermentable = fermentable;
                        if (fermentationStepFermentable.Fermentable == null) return fermentationStepFermentable;
                        if (supplier != null)
                            supplier.Origin = origin;
                        fermentationStepFermentable.Fermentable.Supplier = supplier;
                        return fermentationStepFermentable;
                    }, new { recipe.RecipeId, fermentationStep.StepNumber }, splitOn: "FermentableId,SupplierId,OriginId");
                fermentationStep.Fermentables = fermentationStepFermentables.ToList();
                const string hopFields = @"fs.recipe_id AS RecipeId, fs.step_number AS StepNumber, fs.hop_id AS HopId, fs.aa_value AS AAValue, fs.amount, fs.hop_form_id AS HopFormId," +
                      "h.hop_id AS HopId ,h.name AS Name ,h.aa_low as AALow, h.aa_high AS AAHigh," +
                      "beta_low AS BetaLow, beta_high AS BetaHigh,notes AS Notes ,flavour_description AS FlavourDescription, " +
                      "custom AS Custom,h.origin_id AS OriginId,purpose AS Purpose,aliases AS Aliases ,total_oil_low AS TotalOilLow," +
                      "total_oil_high AS TotalOilHigh,bpinene_low AS BpineneLow,bpinene_high AS BpineneHigh,linalool_low AS LinaloolLow," +
                      "linalool_high AS LinaloolHigh,myrcene_low AS MyrceneLow,myrcene_high As MyrceneHigh,caryophyllene_low AS CaryophylleneLow," +
                      "caryophyllene_high AS CaryophylleneHigh ,farnesene_low AS FarneseneLow,farnesene_high AS FarneseneHigh," +
                      "humulene_low As HumuleneLow ,humulene_high AS HumuleneHigh,geraniol_low AS GeraniolLow,geraniol_high AS GeraniolHigh," +
                      "other_oil_low AS OtherOilLow,other_oil_high AS OtherOilHigh,hf.hop_form_id AS HopFormId, hf.name, o.origin_id AS OriginId, o.name";
                var fermentationStepHops = connection.Query<FermentationStepHop, Hop, HopForm, Origin, FermentationStepHop>(
                   $"SELECT {hopFields} FROM fermentation_step_hops fs " +
                   "LEFT JOIN hops h ON fs.hop_id = h.hop_id " +
                   "LEFT JOIN hop_forms hf ON fs.hop_form_id = hf.hop_form_id " +
                   "LEFT JOIN origins o ON h.origin_id = o.origin_id " +
                   "WHERE recipe_id = @RecipeId and step_number = @StepNumber;", (fermentationStepHop, hop, hopForm, origin) =>
                   {
                       fermentationStepHop.Hop = hop;
                       fermentationStepHop.HopForm = hopForm;
                       if (fermentationStepHop.Hop != null)
                           fermentationStepHop.Hop.Origin = origin;
                       return fermentationStepHop;
                   }, new { recipe.RecipeId, fermentationStep.StepNumber }, splitOn: "HopId,HopFormId,OriginId");
                fermentationStep.Hops = fermentationStepHops.ToList();

                const string otherFields = "fs.recipe_id AS RecipeId, fs.step_number AS StepNumber, fs.other_id AS OtherId, fs.amount, o.other_id AS OtherId, o.name";
                var fermentationStepOthers = connection.Query<FermentationStepOther, Other, FermentationStepOther>(
                  $"SELECT {otherFields} FROM fermentation_step_others fs " +
                  "LEFT JOIN others o ON fs.other_id = o.other_id " +
                  "WHERE recipe_id = @RecipeId and step_number = @StepNumber;", (fermentationStepOther, other) =>
                  {
                      fermentationStepOther.Other = other;
                      return fermentationStepOther;
                  }, new { recipe.RecipeId, fermentationStep.StepNumber }, splitOn: "OtherId");
                fermentationStep.Others = fermentationStepOthers.ToList();

                const string yeastFields = "fs.recipe_id AS RecipeId, fs.step_number AS StepNumber, fs.yeast_id AS YeastId, fs.amount," +
                                           "y.yeast_id AS YeastId, y.name as Name, y.temperature_low AS TemperatureLow, y.temperature_high AS TemperatureHigh," +
                             " y.flocculation, y.alcohol_tolerance AS AlcoholTolerance, y.product_code AS ProductCode, y.notes, " +
                             "y.type, y.brewery_source AS BrewerySource, y.species, y.attenution_range AS AttenutionRange, y.pitching_fermentation_notes As PitchingFermentationNotes, " +
                             "y.supplier_id AS SupplierId, y.custom, s.supplier_id AS SupplierId, s.name, s.origin_id AS OriginId, o.origin_id AS OriginId, o.name";
                var fermentationStepYeasts = connection.Query<FermentationStepYeast, Yeast, Supplier, Origin, FermentationStepYeast>(
                 $"SELECT {yeastFields} FROM fermentation_step_yeasts fs " +
                 "LEFT JOIN yeasts y ON fs.yeast_id = y.yeast_id " +
                 "LEFT JOIN suppliers s ON y.supplier_id = s.supplier_id " +
                 "LEFT JOIN origins o ON s.origin_id = o.origin_id " +
                 "WHERE recipe_id = @RecipeId and step_number = @StepNumber;", (fermentationStepYeast, yeast,supplier,origin) =>
                 {
                     fermentationStepYeast.Yeast = yeast;
                     if (supplier != null)
                         supplier.Origin = origin;
                     if (yeast != null)
                         yeast.Supplier = supplier;
                     return fermentationStepYeast;
                 }, new { recipe.RecipeId, fermentationStep.StepNumber }, splitOn: "YeastId,SupplierId,OriginId");
                fermentationStep.Yeasts = fermentationStepYeasts.ToList();
            }
            recipe.FermentationSteps = fermentationSteps.ToList();
        }

        private async Task GetBoilSteps(DbConnection connection, Recipe recipe)
        {
            var boilSteps = await connection.QueryAsync<BoilStep>(
                "SELECT recipe_id AS RecipeId, step_number AS StepNumber, length, volume, notes FROM boil_steps WHERE recipe_id = @RecipeId;", new { recipe.RecipeId });
            foreach (var boilStep in boilSteps)
            {
                var fermentablesField = "b.recipe_id AS RecipeId, b.step_number AS StepNumber, b.fermentable_id AS FermentableId, " +
                           "b.amount, b.lovibond, b.ppg, f.fermentable_id AS FermentableId, f.name, f.ebc, f.lovibond, " +
                           "f.ppg, f.supplier_id AS SupplierId, f.type, f.custom, f.super_fermentable_id AS SuperFermentableId," +
                           "s.supplier_id AS SupplierId, s.name, s.origin_id AS OriginId, o.origin_id AS OriginId, o.name";
                var boilStepFermentables = await connection.QueryAsync<BoilStepFermentable, Fermentable, Supplier, Origin, BoilStepFermentable>(
                    $"SELECT {fermentablesField} FROM boil_step_fermentables b " +
                    "LEFT JOIN fermentables f ON b.fermentable_id = f.fermentable_id " +
                    "LEFT JOIN suppliers s ON f.supplier_id = s.supplier_id " +
                    "LEFT JOIN origins o ON o.origin_id = s.origin_id " +
                    "WHERE recipe_id = @RecipeId and step_number = @StepNumber;", (boilStepFermentable, fermentable, supplier, origin) =>
                    {
                        boilStepFermentable.Fermentable = fermentable;
                        if (boilStepFermentable.Fermentable == null) return boilStepFermentable;
                        if (supplier != null)
                            supplier.Origin = origin;
                        boilStepFermentable.Fermentable.Supplier = supplier;
                        return boilStepFermentable;
                    }, new { recipe.RecipeId, boilStep.StepNumber }, splitOn: "FermentableId,SupplierId,OriginId");
                boilStep.Fermentables = boilStepFermentables.ToList();
                const string hopFields = @"b.recipe_id AS RecipeId, b.step_number AS StepNumber, b.hop_id AS HopId, b.aavalue AS AAValue, b.amount, b.hop_form_id AS HopFormId," +
                       "h.hop_id AS HopId ,h.name AS Name ,h.aa_low as AALow, h.aa_high AS AAHigh," +
                       "beta_low AS BetaLow, beta_high AS BetaHigh,notes AS Notes ,flavour_description AS FlavourDescription, " +
                       "custom AS Custom,h.origin_id AS OriginId,purpose AS Purpose,aliases AS Aliases ,total_oil_low AS TotalOilLow," +
                       "total_oil_high AS TotalOilHigh,bpinene_low AS BpineneLow,bpinene_high AS BpineneHigh,linalool_low AS LinaloolLow," +
                       "linalool_high AS LinaloolHigh,myrcene_low AS MyrceneLow,myrcene_high As MyrceneHigh,caryophyllene_low AS CaryophylleneLow," +
                       "caryophyllene_high AS CaryophylleneHigh ,farnesene_low AS FarneseneLow,farnesene_high AS FarneseneHigh," +
                       "humulene_low As HumuleneLow ,humulene_high AS HumuleneHigh,geraniol_low AS GeraniolLow,geraniol_high AS GeraniolHigh," +
                       "other_oil_low AS OtherOilLow,other_oil_high AS OtherOilHigh,hf.hop_form_id AS HopFormId, hf.name, o.origin_id AS OriginId, o.name";

                var boilStepHops = await connection.QueryAsync<BoilStepHop, Hop, HopForm, Origin, BoilStepHop>(
                   $"SELECT {hopFields} FROM boil_step_hops b " +
                   "LEFT JOIN Hops h ON b.hop_id = h.hop_id " +
                   "LEFT JOIN hop_forms hf ON b.hop_form_id = hf.hop_form_id " +
                   "LEFT JOIN origins o ON h.origin_id = o.origin_id " +
                   "WHERE recipe_id = @RecipeId and step_number = @StepNumber;", (boilStepHop, hop, hopForm, origin) =>
                   {
                       boilStepHop.Hop = hop;
                       boilStepHop.HopForm = hopForm;
                       if (boilStepHop.Hop != null)
                           boilStepHop.Hop.Origin = origin;
                       return boilStepHop;
                   }, new { recipe.RecipeId, boilStep.StepNumber }, splitOn: "HopId, HopFormId, OriginId");
                boilStep.Hops = boilStepHops.ToList();

                const string otherFields = "b.recipe_id AS RecipeId, b.step_number AS StepNumber, b.other_id AS OtherId, b.amount, o.other_id AS OtherId, o.name";
                var boilStepOthers = await connection.QueryAsync<BoilStepOther, Other, BoilStepOther>(
                  $"SELECT {otherFields} FROM boil_step_others b " +
                  "LEFT JOIN others o ON b.other_id = o.other_id " +
                  "WHERE recipe_id = @RecipeId and step_number = @StepNumber;", (boilStepOther, other) =>
                  {
                      boilStepOther.Other = other;
                      return boilStepOther;
                  }, new { recipe.RecipeId, boilStep.StepNumber }, splitOn: "OtherId");
                boilStep.Others = boilStepOthers.ToList();
            }
            recipe.BoilSteps = boilSteps.ToList();
        }

        private async Task GetMashSteps(DbConnection connection, Recipe recipe)
        {
            var mashSteps = await connection.QueryAsync<MashStep>(
                "SELECT recipe_id AS RecipeId, step_number AS StepNumber, temperature, type, length, volume, notes " +
                "FROM mash_steps WHERE recipe_id = @RecipeId;", new { recipe.RecipeId });
            foreach (var mashStep in mashSteps)
            {
                var field = "m.recipe_id AS RecipeId, m.step_number AS StepNumber, m.fermentable_id AS FermentableId, " +
                            "m.amount, m.lovibond, m.ppg, f.fermentable_id AS FermentableId, f.name, f.ebc, f.lovibond, " +
                            "f.ppg, f.supplier_id AS SupplierId, f.type, f.custom, f.super_fermentable_id AS SuperFermentableId," +
                            "s.supplier_id AS SupplierId, s.name, s.origin_id AS OriginId, o.origin_id AS OriginId, o.name";
                var mashStepFermentables = await connection.QueryAsync<MashStepFermentable, Fermentable, Supplier, Origin, MashStepFermentable>(
                    $"SELECT {field} FROM mash_step_fermentables m " +
                    "LEFT JOIN fermentables f ON m.fermentable_id = f.fermentable_id " +
                    "LEFT JOIN suppliers s ON f.supplier_id = s.supplier_id " +
                    "LEFT JOIN origins o ON o.origin_Id = s.origin_Id " +
                    "WHERE recipe_id = @RecipeId and step_number = @StepNumber;", (mashStepFermentable, fermentable, supplier, origin) =>
                    {
                        mashStepFermentable.Fermentable = fermentable;
                        if (mashStepFermentable.Fermentable == null) return mashStepFermentable;
                        if (supplier != null)
                            supplier.Origin = origin;
                        mashStepFermentable.Fermentable.Supplier = supplier;
                        return mashStepFermentable;
                    }, new { recipe.RecipeId, mashStep.StepNumber }, splitOn: "SupplierId,OriginId");
                mashStep.Fermentables = mashStepFermentables.ToList();
                const string hopFields = @"m.recipe_id AS RecipeId, m.step_number AS StepNumber, m.hop_id AS HopId, m.aavalue AS AAValue, m.amount, m.hop_form_id AS HopFormId," +
                        "h.hop_id AS HopId ,h.name AS Name ,h.aa_low as AALow, h.aa_high AS AAHigh," +
                        "beta_low AS BetaLow, beta_high AS BetaHigh,notes AS Notes ,flavour_description AS FlavourDescription, " +
                        "custom AS Custom,h.origin_id AS OriginId,purpose AS Purpose,aliases AS Aliases ,total_oil_low AS TotalOilLow," +
                        "total_oil_high AS TotalOilHigh,bpinene_low AS BpineneLow,bpinene_high AS BpineneHigh,linalool_low AS LinaloolLow," +
                        "linalool_high AS LinaloolHigh,myrcene_low AS MyrceneLow,myrcene_high As MyrceneHigh,caryophyllene_low AS CaryophylleneLow," +
                        "caryophyllene_high AS CaryophylleneHigh ,farnesene_low AS FarneseneLow,farnesene_high AS FarneseneHigh," +
                        "humulene_low As HumuleneLow ,humulene_high AS HumuleneHigh,geraniol_low AS GeraniolLow,geraniol_high AS GeraniolHigh," +
                        "other_oil_low AS OtherOilLow,other_oil_high AS OtherOilHigh,hf.hop_form_id AS HopFormId, hf.name, o.origin_id AS OriginId, o.name";

                var sql = $"SELECT {hopFields} FROM mash_step_hops m " +
                   "LEFT JOIN hops h ON m.hop_id = h.hop_id " +
                   "LEFT JOIN hop_forms hf ON hf.hop_form_id = m.hop_form_id " +
                   "LEFT JOIN origins o ON o.origin_id = h.origin_id " +
                   "WHERE recipe_id = @RecipeId and step_number = @StepNumber;";

                var mashStepHops = connection.Query<MashStepHop, Hop, HopForm, Origin, MashStepHop>(
                   sql, (mashStepHop, hop, hopForm, origin) =>
                   {
                       mashStepHop.Hop = hop;
                       mashStepHop.HopForm = hopForm;
                       if (mashStepHop.Hop != null)
                           mashStepHop.Hop.Origin = origin;
                       return mashStepHop;
                   }, new { recipe.RecipeId, mashStep.StepNumber }, splitOn: "HopId,HopFormId,OriginId");
                mashStep.Hops = mashStepHops.ToList();
                var otherFields = "recipe_id AS RecipeId, step_number AS StepNumber, m.other_id AS OtherId, amount, o.other_id AS OtherId, o.name";
                var mashStepOthers = await connection.QueryAsync<MashStepOther, Other, MashStepOther>(
                  $"SELECT {otherFields} FROM mash_step_others m " +
                  "LEFT JOIN others o ON m.other_id = o.other_id " +
                  "WHERE recipe_id = @RecipeId and step_number = @StepNumber;", (mashStepOther, other) =>
                  {
                      mashStepOther.Other = other;
                      return mashStepOther;
                  }, new { recipe.RecipeId, mashStep.StepNumber }, splitOn: "OtherId");
                mashStep.Others = mashStepOthers.ToList();
            }
            recipe.MashSteps = mashSteps.ToList();
        }

        private async Task GetForkOf(DbConnection connection, Beer beer)
        {
            var beerfields =
              "b.beer_id AS BeerId, b.name, b.beerstyle_id AS BeerStyleId, b.created_date AS CreatedDate, b.updated_date AS UpdatedDate, b.fork_of_id AS ForkOfId," +
              "bs.beerstyle_id AS BeerStyleId, bs.name, bs.superstyle_id AS SuperStyleId, bs.og_low AS OGLow , bs.og_high AS OGHigh, bs.fg_low AS FGLow, bs.fg_high FGHigh," +
              "bs.ibu_low AS IBULow, bs.ibu_high AS IBUHigh, bs.srm_low AS SRMLow, bs.srm_high AS SRMHig, bs.abv_low AS ABVLow, bs.abv_high AS ABVHigh, bs.comments, " +
              "s.srm_id AS SrmId, s.standard, s.mosher, s.daniels, s.morey, " +
              "a.abv_id AS AbvId, a.standard, a.miller, a.advanced, a.advanced_alternative AS AdvancedAlternative, a.simple, a.simple_alternative AS SimpleAlternative," +
              "i.ibu_id AS IbuId, i.standard, i.tinseth, i.rager ";
            var sql = $"SELECT {beerfields} FROM Beers AS b " +
                      "LEFT JOIN beerstyles bs ON bs.beerstyle_id = b.beerstyle_id " +
                      "LEFT JOIN srms s ON s.srm_id = b.beer_id " +
                      "LEFT JOIN abvs a ON a.abv_id = b.beer_id " +
                      "LEFT JOIN ibus i ON i.ibu_id = b.beer_id " +
                      "WHERE beer_id = @BeerId ";
            var forkOf = await connection.QueryAsync<Beer, BeerStyle, SRM, ABV, IBU, Beer>(
                    sql, (b, beerStyle, srm, abv, ibu) =>
                     {
                         if (beerStyle != null)
                             b.BeerStyle = beerStyle;
                         if (srm != null)
                             b.SRM = srm;
                         if (abv != null)
                             b.ABV = abv;
                         if (ibu != null)
                             b.IBU = ibu;
                         return b;
                     },
                    new { beer.BeerId },
                    splitOn: "BeerStyleId,SrmId,AbvId,IbuId"
                    );
            beer.ForkeOf = forkOf.SingleOrDefault();
        }

        private async Task GetBrewers(DbConnection connection, Beer beer)
        {
            if (connection == null) throw new ArgumentNullException("connection is missing");
            var brewers = await connection.QueryAsync<UserBeer, User, UserBeer>(
                "SELECT ub.beer_id AS BeerId, ub.user_id AS UserId, ub.confirmed, " +
                "u.username AS Username, u.email, u.settings, gravatar, longitude, latitude, header_image_url, " +
                "avatar_url AS Avatar FROM user_beers ub " +
                "LEFT JOIN Users u ON ub.user_id = u.user_id " +
                "WHERE ub.beer_id = @BeerId;",
                (userBeer, user) =>
                {
                    userBeer.User = user;
                    return userBeer;
                }, new { beer.BeerId }, splitOn: "UserId");
            beer.Brewers = brewers.ToList();
        }

        private async Task GetBreweries(DbConnection connection, Beer beer)
        {
            var sql =
                "SELECT bb.brewery_id AS BreweryId, bb.beer_id AS BeerId, b.brewery_id AS BreweryId, b.name, description, " +
                "type, created_date AS CreatedDate, updated_date AS UpdatedDate," +
                "longitude, latitude, website, established, header_image_url AS HeaderImageUrl," +
                "avatar_url AS AvatarUrl, b.origin_id AS OriginId, address " +
                "FROM brewery_beers bb " +
                "LEFT JOIN Breweries b ON bb.brewery_id = b.brewery_id " +
                "WHERE bb.beer_id = @BeerId";
            var breweries = await connection.QueryAsync<BreweryBeer, Brewery, BreweryBeer>(
                       sql , (breweryBeer, brewery) =>
                       {
                           breweryBeer.Brewery = brewery;
                           return breweryBeer;
                       }, new { beer.BeerId }, splitOn: "BreweryId");
            beer.Breweries = breweries.ToList();
        }

        private async Task GetForks(DbConnection connection, Beer beer)
        {
            var beerfields =
             "b.beer_id AS BeerId, b.name, b.beerstyle_id AS BeerStyleId, b.created_date AS CreatedDate, b.updated_date AS UpdatedDate, b.fork_of_id AS ForkOfId," +
             "bs.beerstyle_id AS BeerStyleId, bs.name, bs.superstyle_id AS SuperStyleId, bs.og_low AS OGLow , bs.og_high AS OGHigh, bs.fg_low AS FGLow, bs.fg_high FGHigh," +
             "bs.ibu_low AS IBULow, bs.ibu_high AS IBUHigh, bs.srm_low AS SRMLow, bs.srm_high AS SRMHig, bs.abv_low AS ABVLow, bs.abv_high AS ABVHigh, bs.comments, " +
             "s.srm_id AS SrmId, s.standard, s.mosher, s.daniels, s.morey, " +
             "a.abv_id AS AbvId, a.standard, a.miller, a.advanced, a.advanced_alternative AS AdvancedAlternative, a.simple, a.simple_alternative AS SimpleAlternative," +
             "i.ibu_id AS IbuId, i.standard, i.tinseth, i.rager ";
            var sql = $"SELECT {beerfields} FROM Beers AS b " +
                      "LEFT JOIN beerstyles bs ON bs.beerstyle_id = b.beerstyle_id " +
                      "LEFT JOIN srms s ON s.srm_id = b.beer_id " +
                      "LEFT JOIN abvs a ON a.abv_id = b.beer_id " +
                      "LEFT JOIN ibus i ON i.ibu_id = b.beer_id " +
                      "WHERE b.fork_of_id = @BeerId";
            var forks = await connection.QueryAsync<Beer, SRM, ABV, IBU, BeerStyle, Beer>(
                  sql, (fork, srm, abv, ibu, beerStyle) =>
                    {
                        if (srm != null)
                            fork.SRM = srm;
                        if (abv != null)
                            fork.ABV = abv;
                        if (ibu != null)
                            fork.IBU = ibu;
                        if (beerStyle != null)
                            fork.BeerStyle = beerStyle;
                        return beer;
                    }, new { beer.BeerId }, splitOn: "BeerStyleId,SrmId,AbvId,IbuId");
            beer.Forks = forks.ToList();
        }

        private async Task AddRecipeAsync(Recipe recipe, DbConnection connection, DbTransaction transaction)
        {

            await connection.ExecuteAsync("INSERT INTO recipes(recipe_id,volume,notes,og,fg,efficiency,total_boil_time) " +
                            "VALUES(@RecipeId,@Volume,@Notes,@OG,@FG,@Efficiency,@TotalBoilTime);",
                new
                {
                    recipe.RecipeId,
                    recipe.Volume,
                    recipe.Notes,
                    recipe.OG,
                    recipe.FG,
                    recipe.Efficiency,
                    recipe.TotalBoilTime
                }, transaction);

            //SetStepNumber(recipe);
            if (recipe.MashSteps != null)
            {
                foreach (var mashStep in recipe.MashSteps)
                {
                    mashStep.RecipeId = recipe.RecipeId;
                    await AddMashStep(connection, transaction, mashStep);
                }
            }
            if (recipe.BoilSteps != null)
            {
                foreach (var boilStep in recipe.BoilSteps)
                {
                    boilStep.RecipeId = recipe.RecipeId;
                    await AddBoilStep(connection, transaction, boilStep);
                }
            }

            if (recipe.FermentationSteps != null)
            {
                foreach (var fermentationStep in recipe.FermentationSteps)
                {
                    fermentationStep.RecipeId = recipe.RecipeId;
                    await AddFermentationStep(connection, transaction, fermentationStep);
                }
            }
        }

        private async Task AddMashStep(DbConnection connection, DbTransaction transaction, MashStep mashStep)
        {

            await connection.ExecuteAsync(
                "INSERT INTO mash_steps(temperature,type,length,volume,notes,recipe_id,step_number)" +
                "VALUES(@Temperature,@Type,@Length,@Volume,@Notes,@RecipeId,@StepNumber)", mashStep, transaction);
            if (mashStep.Fermentables != null)
            {
                var duplicates = mashStep.Fermentables.GroupBy(f => f.FermentableId).Where(g => g.Count() > 1).Select(g => g.Key);
                if (duplicates.Any())
                {
                    mashStep.Fermentables = mashStep.Fermentables.GroupBy(o => o.FermentableId)
                        .Select(g => g.Skip(1).Aggregate(
                        g.First(), (a, o) => { a.Amount += o.Amount; return a; })).ToList();
                }
                await connection.ExecuteAsync(
                    "INSERT INTO mash_step_fermentables(recipe_id,step_number,fermentable_id,amount,lovibond,ppg) " +
                    "VALUES(@RecipeId,@StepNumber,@FermentableId,@Amount,@Lovibond,@PPG);",
                    mashStep.Fermentables.Select(f => new
                    {
                        mashStep.RecipeId,
                        mashStep.StepNumber,
                        f.FermentableId,
                        f.Amount,
                        f.Lovibond,
                        f.PPG,
                    }), transaction);
            }

            if (mashStep.Hops != null)
            {
                var duplicates = mashStep.Hops.GroupBy(f => f.HopId).Where(g => g.Count() > 1).Select(g => g.Key);
                if (duplicates.Any())
                {
                    mashStep.Hops = mashStep.Hops.GroupBy(o => o.HopId)
                        .Select(g => g.Skip(1).Aggregate(
                        g.First(), (a, o) => { a.Amount += o.Amount; return a; })).ToList();
                }
                await connection.ExecuteAsync(
                    "INSERT INTO mash_step_hops(recipe_id,step_number,hop_Id,amount,aavalue,hop_form_id) " +
                    "VALUES(@RecipeId,@StepNumber,@HopId,@Amount,@AAValue,@HopFormId);",
                    mashStep.Hops.Select(h => new
                    {
                        mashStep.RecipeId,
                        mashStep.StepNumber,
                        h.HopId,
                        h.HopFormId,
                        h.AAValue,
                        h.Amount
                    }), transaction);
            }

            if (mashStep.Others != null)
            {
                var duplicates = mashStep.Others.GroupBy(f => f.OtherId).Where(g => g.Count() > 1).Select(g => g.Key);
                if (duplicates.Any())
                {
                    mashStep.Others = mashStep.Others.GroupBy(o => o.OtherId)
                        .Select(g => g.Skip(1).Aggregate(
                        g.First(), (a, o) => { a.Amount += o.Amount; return a; })).ToList();
                }
                await connection.ExecuteAsync(
                    "INSERT INTO mash_step_others(recipe_id,step_number,other_id,amount) " +
                    "VALUES(@RecipeId,@StepNumber,@OtherId,@Amount);",
                    mashStep.Others.Select(o => new
                    {
                        mashStep.RecipeId,
                        mashStep.StepNumber,
                        o.Amount,
                        o.OtherId
                    }), transaction);
            }
        }

        private async Task AddBoilStep(DbConnection connection, DbTransaction transaction, BoilStep boilStep)
        {
            await connection.ExecuteAsync(
                "INSERT INTO boil_steps(length,volume,notes,recipe_id,step_number)" +
                "VALUES(@Length,@Volume,@Notes,@RecipeId,@StepNumber)", boilStep, transaction);


            if (boilStep.Fermentables != null)
            {
                var duplicates = boilStep.Fermentables.GroupBy(f => f.FermentableId).Where(g => g.Count() > 1).Select(g => g.Key);
                if (duplicates.Any())
                {
                    boilStep.Fermentables = boilStep.Fermentables.GroupBy(o => o.FermentableId)
                        .Select(g => g.Skip(1).Aggregate(
                        g.First(), (a, o) => { a.Amount += o.Amount; return a; })).ToList();
                }
                await connection.ExecuteAsync(
                    "INSERT INTO boil_step_fermentables(recipe_id,step_number,fermentable_id,amount,lovibond,ppg) " +
                    "VALUES(@RecipeId,@StepNumber,@FermentableId,@Amount,@Lovibond,@PPG);",
                    boilStep.Fermentables.Select(f => new
                    {
                        boilStep.RecipeId,
                        boilStep.StepNumber,
                        f.FermentableId,
                        f.Amount,
                        f.Lovibond,
                        f.PPG,
                    }), transaction);
            }

            if (boilStep.Hops != null)
            {
                var duplicates = boilStep.Hops.GroupBy(f => f.HopId).Where(g => g.Count() > 1).Select(g => g.Key);
                if (duplicates.Any())
                {
                    boilStep.Hops = boilStep.Hops.GroupBy(o => o.Hop)
                        .Select(g => g.Skip(1).Aggregate(
                        g.First(), (a, o) => { a.Amount += o.Amount; return a; })).ToList();
                }
                await connection.ExecuteAsync(
                    "INSERT INTO boil_step_hops(recipe_Id,step_number,hop_id,amount,aavalue,hop_form_id) " +
                    "VALUES(@RecipeId,@StepNumber,@HopId,@Amount,@AAValue,@HopFormId);",
                    boilStep.Hops.Select(h => new
                    {
                        boilStep.RecipeId,
                        boilStep.StepNumber,
                        h.HopId,
                        h.HopFormId,
                        h.AAValue,
                        h.Amount
                    }), transaction);
            }

            if (boilStep.Others != null)
            {
                var duplicates = boilStep.Others.GroupBy(f => f.OtherId).Where(g => g.Count() > 1).Select(g => g.Key);
                if (duplicates.Any())
                {
                    boilStep.Others = boilStep.Others.GroupBy(o => o.OtherId)
                        .Select(g => g.Skip(1).Aggregate(
                        g.First(), (a, o) => { a.Amount += o.Amount; return a; })).ToList();
                }
                await connection.ExecuteAsync(
                    "INSERT INTO boil_step_others(recipe_id,step_number,other_id,amount) " +
                    "VALUES(@RecipeId,@StepNumber,@OtherId,@Amount);",
                    boilStep.Others.Select(o => new
                    {
                        boilStep.RecipeId,
                        boilStep.StepNumber,
                        o.Amount,
                        o.OtherId
                    }), transaction);
            }
        }

        private async Task AddFermentationStep(DbConnection connection, DbTransaction transaction, FermentationStep fermentationStep)
        {
            await connection.ExecuteAsync(
                "INSERT INTO fermentation_steps(temperature,length,volume,notes,recipe_Id,step_number)" +
                "VALUES(@Temperature,@Length,@Volume,@Notes,@RecipeId,@StepNumber)", fermentationStep, transaction);

            if (fermentationStep.Fermentables != null)
            {
                var duplicates = fermentationStep.Fermentables.GroupBy(f => f.FermentableId).Where(g => g.Count() > 1).Select(g => g.Key);
                if (duplicates.Any())
                {
                    fermentationStep.Fermentables = fermentationStep.Fermentables.GroupBy(o => o.FermentableId)
                        .Select(g => g.Skip(1).Aggregate(
                        g.First(), (a, o) => { a.Amount += o.Amount; return a; })).ToList();
                }
                await connection.ExecuteAsync(
                    "INSERT INTO fermentation_step_fermentables(recipe_id,step_number,fermentable_id,amount,lovibond,ppg) " +
                    "VALUES(@RecipeId,@StepNumber,@FermentableId,@Amount,@Lovibond,@PPG);",
                    fermentationStep.Fermentables.Select(f => new
                    {
                        fermentationStep.RecipeId,
                        fermentationStep.StepNumber,
                        f.FermentableId,
                        f.Amount,
                        f.Lovibond,
                        f.PPG,
                    }), transaction);
            }

            if (fermentationStep.Hops != null)
            {
                var duplicates = fermentationStep.Hops.GroupBy(f => f.HopId).Where(g => g.Count() > 1).Select(g => g.Key);
                if (duplicates.Any())
                {
                    fermentationStep.Hops = fermentationStep.Hops.GroupBy(o => o.HopId)
                        .Select(g => g.Skip(1).Aggregate(
                        g.First(), (a, o) => { a.Amount += o.Amount; return a; })).ToList();
                }
                await connection.ExecuteAsync(
                    "INSERT INTO fermentation_step_hops(recipe_id,step_number,hop_id,amount,aa_value,hop_form_id) " +
                    "VALUES(@RecipeId,@StepNumber,@HopId,@Amount,@AAValue,@HopFormId);",
                    fermentationStep.Hops.Select(h => new
                    {
                        fermentationStep.RecipeId,
                        fermentationStep.StepNumber,
                        h.HopId,
                        h.HopFormId,
                        h.AAValue,
                        h.Amount
                    }), transaction);
            }

            if (fermentationStep.Others != null)
            {
                var duplicates = fermentationStep.Others.GroupBy(f => f.OtherId).Where(g => g.Count() > 1).Select(g => g.Key);
                if (duplicates.Any())
                {
                    fermentationStep.Others = fermentationStep.Others.GroupBy(o => o.OtherId)
                        .Select(g => g.Skip(1).Aggregate(
                        g.First(), (a, o) => { a.Amount += o.Amount; return a; })).ToList();
                }
                await connection.ExecuteAsync(
                    "INSERT INTO fermentation_step_others(recipe_id,step_number,other_id,amount) " +
                    "VALUES(@RecipeId,@StepNumber,@OtherId,@Amount);",
                    fermentationStep.Others.Select(o => new
                    {
                        fermentationStep.RecipeId,
                        fermentationStep.StepNumber,
                        o.Amount,
                        o.OtherId
                    }), transaction);
            }

            if (fermentationStep.Yeasts != null)
            {
                var duplicates = fermentationStep.Yeasts.GroupBy(f => f.YeastId).Where(g => g.Count() > 1).Select(g => g.Key);
                if (duplicates.Any())
                {
                    fermentationStep.Yeasts = fermentationStep.Yeasts.GroupBy(o => o.YeastId)
                        .Select(g => g.Skip(1).Aggregate(
                        g.First(), (a, o) => { a.Amount += o.Amount; return a; })).ToList();
                }
                await connection.ExecuteAsync(
                    "INSERT INTO fermentation_step_yeasts(recipe_id,step_number,yeast_id,amount) " +
                    "VALUES(@RecipeId,@StepNumber,@YeastId,@Amount);",
                    fermentationStep.Yeasts.Select(o => new
                    {
                        fermentationStep.RecipeId,
                        fermentationStep.StepNumber,
                        o.Amount,
                        o.YeastId
                    }), transaction);
            }
        }

        private async Task UpdateRecipe(DbConnection connection, DbTransaction transaction, Recipe recipe)
        {
            await connection.ExecuteAsync("UPDATE recipes set volume = @Volume, notes = @Notes,og = @OG,fg = @FG, efficiency = @Efficiency, total_boil_time = @TotalBoilTime " +
                           "WHERE recipe_id = @RecipeId", recipe, transaction);
            //SetStepNumber(recipe);
            await UpdateMashSteps(connection, transaction, recipe);
            await UpdateBoilSteps(connection, transaction, recipe);
            await UpdateFermentationSteps(connection, transaction, recipe);
        }

        private async Task UpdateMashSteps(DbConnection connection, DbTransaction transaction, Recipe recipe)
        {
            //Remove mash steps 
            await connection.ExecuteAsync("DELETE FROM mash_step_fermentables WHERE recipe_id = @RecipeId;", new { recipe.RecipeId }, transaction);
            await connection.ExecuteAsync("DELETE FROM mash_step_hops WHERE recipe_id = @RecipeId;", new { recipe.RecipeId }, transaction);
            await connection.ExecuteAsync("DELETE FROM mash_step_others WHERE recipe_id = @RecipeId ;", new { recipe.RecipeId }, transaction);
            await connection.ExecuteAsync("DELETE FROM mash_steps WHERE recipe_id = @RecipeId;", new { recipe.RecipeId }, transaction);
            //Add mash steps
            foreach (var mashStep in recipe.MashSteps)
            {
                mashStep.RecipeId = recipe.RecipeId;
                await AddMashStep(connection, transaction, mashStep);
            }
        }

        private async Task UpdateBoilSteps(DbConnection connection, DbTransaction transaction, Recipe recipe)
        {
            //Remove mash steps 
            
            await connection.ExecuteAsync("DELETE FROM boil_step_fermentables WHERE recipe_id = @RecipeId;", new { recipe.RecipeId }, transaction);
            await connection.ExecuteAsync("DELETE FROM boil_step_hops WHERE recipe_id = @RecipeId;", new { recipe.RecipeId }, transaction);
            await connection.ExecuteAsync("DELETE FROM boil_step_others WHERE recipe_id = @RecipeId ;", new { recipe.RecipeId }, transaction);
            await connection.ExecuteAsync("DELETE FROM boil_steps WHERE recipe_id = @RecipeId;", new { recipe.RecipeId }, transaction);
            //Add mash steps
            foreach (var boilStep in recipe.BoilSteps)
            {
                boilStep.RecipeId = recipe.RecipeId;
                await AddBoilStep(connection, transaction, boilStep);
            }
        }

        private async Task UpdateFermentationSteps(DbConnection connection, DbTransaction transaction, Recipe recipe)
        {
            //Remove mash steps 
            await connection.ExecuteAsync("DELETE FROM fermentation_step_fermentables WHERE recipe_id = @RecipeId;", new { recipe.RecipeId }, transaction);
            await connection.ExecuteAsync("DELETE FROM fermentation_step_hops WHERE recipe_id = @RecipeId;", new { recipe.RecipeId }, transaction);
            await connection.ExecuteAsync("DELETE FROM fermentation_step_others WHERE recipe_id = @RecipeId ;", new { recipe.RecipeId }, transaction);
            await connection.ExecuteAsync("DELETE FROM fermentation_step_yeasts WHERE recipe_id = @RecipeId ;", new { recipe.RecipeId }, transaction);
            await connection.ExecuteAsync("DELETE FROM fermentation_steps WHERE recipe_id = @RecipeId;", new { recipe.RecipeId }, transaction);
            //Add mash steps
            foreach (var fermentationStep in recipe.FermentationSteps)
            {
                fermentationStep.RecipeId = recipe.RecipeId;
                await AddFermentationStep(connection, transaction, fermentationStep);
            }
        }
    }
}
