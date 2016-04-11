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
    public class BreweryDapperRepository : IBreweryRepository
    {
        
       private DatabaseSettings _databaseSettings;
      public BreweryDapperRepository(IOptions<DatabaseSettings> databaseSettings)
      {
          _databaseSettings = databaseSettings.Value;
      }
      
      public async Task<IEnumerable<Brewery>> GetAllAsync(int from, int size)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                var sql =
                    "SELECT brewery_id AS BreweryId, b.name, description, type, created_date AS CreatedDate, updated_date AS UpdatedDate," +
                    "longitude, latitude, website, established, header_image_url AS HeaderImageUrl," +
                    "avatar_url AS AvatarUrl, b.origin_id AS OriginId, address, o.origin_id AS OriginId, o.name  " +
                    "FROM Breweries b LEFT JOIN Origins o ON b.origin_id = o.origin_id " +
                    "ORDER BY brewery_id LIMIT @Size OFFSET @From;";
                var breweries = (await connection.QueryAsync<Brewery, Origin, Brewery>(sql
                   , (brewery, origin) =>
                    {
                        if (origin != null)
                            brewery.Origin = origin;
                        return brewery;
                    }, new { From = from, Size = size }, splitOn: "OriginId")).ToList();

                foreach (var brewery in breweries)
                {
                    await GetBreweryProperties(connection, brewery);
                }
                return breweries.ToList();
            }
        }

        private static async Task GetBreweryProperties(DbConnection connection, Brewery brewery)
        {
            if (brewery == null) return;
            var breweryMembers =
                connection.Query<BreweryMember>(
                    "SELECT brewery_id AS BreweryId, member_username AS MemberUsername, role,confirmed FROM brewery_members bm " +
                    "WHERE bm.brewery_id = @BreweryId;",
                    new {brewery.BreweryId});
            brewery.Members = breweryMembers.ToList();

            var sql = "SELECT bb.beer_id AS BeerId, bb.brewery_id AS BreweryId," +
                      "b.beer_id AS BeerId, b.name, b.beerstyle_id AS BeerStyleId, b.created_date AS CreatedDate, b.updated_date AS UpdatedDate, b.fork_of_id AS ForkOfId," +
                      "s.srm_id AS SrmId, s.standard, s.mosher, s.daniels, s.morey, " +
                      "a.abv_id AS AbvId, a.standard, a.miller, a.advanced, a.advanced_alternative AS AdvancedAlternative, a.simple, a.simple_alternative AS SimpleAlternative," +
                      "i.ibu_id AS IbuId, i.standard, i.tinseth, i.rager, " +
                      "bs.beerstyle_id AS BeerStyleId, bs.name, bs.superstyle_id AS SuperStyleId, bs.og_low AS OGLow , bs.og_high AS OGHigh, bs.fg_low AS FGLow, bs.fg_high FGHigh," +
                      "bs.ibu_low AS IBULow, bs.ibu_high AS IBUHigh, bs.srm_low AS SRMLow, bs.srm_high AS SRMHig, bs.abv_low AS ABVLow, bs.abv_high AS ABVHigh, bs.comments " +
                      "FROM brewery_beers bb " +
                      "LEFT JOIN Beers b ON bb.beer_id = b.beer_id " +
                      "LEFT JOIN SRMs s ON s.srm_id = b.beer_id " +
                      "LEFT JOIN ABVs a ON a.abv_id = b.beer_id " +
                      "LEFT JOIN IBUs i ON i.ibu_id = b.beer_id " +
                      "LEFT JOIN BeerStyles bs ON bs.beerstyle_id = b.beerstyle_id " +
                      "WHERE bb.brewery_id = @BreweryId";
            var breweryBeers = await connection.QueryAsync<BreweryBeer, Beer, SRM, ABV, IBU, BeerStyle, BreweryBeer>(sql,
                (breweryBeer, beer, srm, abv, ibu, beerStyle) =>
                {
                    breweryBeer.Beer = beer;
                    if (srm != null)
                        beer.SRM = srm;
                    if (abv != null)
                        beer.ABV = abv;
                    if (ibu != null)
                        beer.IBU = ibu;
                    if (beerStyle != null)
                        beer.BeerStyle = beerStyle;
                    return breweryBeer;
                }, new {brewery.BreweryId}, splitOn: "BeerId,SrmId,AbvId,IbuId,BeerStyleId");
            brewery.Beers = breweryBeers.ToList();

            var brewerySocials =
                await
                    connection.QueryAsync<BrewerySocial>(
                        "SELECT brewery_id AS BreweryId, social_id AS SocialId, site, url FROM brewery_socials WHERE brewery_id = @BreweryId",
                        new {brewery.BreweryId});
            brewery.Socials = brewerySocials.ToList();
        }

        public async Task<Brewery> GetSingleAsync(int id)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {

                var sql =
                    "SELECT brewery_id AS BreweryId, b.name, description, type, created_date AS CreatedDate, updated_date AS UpdatedDate," +
                    "longitude, latitude, website, established, header_image_url AS HeaderImageUrl," +
                    "avatar_url AS AvatarUrl, b.origin_id AS OriginId, address, o.origin_id AS OriginId, o.name  " +
                    "FROM breweries b LEFT JOIN origins o ON b.origin_id = o.origin_id " +
                    "WHERE brewery_id = @BreweryId;";
                var breweries = await connection.QueryAsync<Brewery, Origin, Brewery>(sql,(b, origin) =>
                    {
                        if (origin != null)
                            b.Origin = origin;
                        return b;
                    }, new { BreweryId = id }, splitOn: "OriginId");

                var brewery = breweries.SingleOrDefault();
                await GetBreweryProperties(connection, brewery);
                return brewery;
            }
        }

        public async Task AddAsync(Brewery brewery)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        brewery.UpdatedDate = DateTime.Now;
                        brewery.CreatedDate = DateTime.Now;
                        var result =
                            await connection.ExecuteAsync(
                                "INSERT INTO breweries(name,description,type,created_date,updated_date,longitude,latitude,website,established,header_image_url,avatar_url,origin_id,address) " +
                                "VALUES (@Name,@Description,@Type,@CreatedDate,@UpdatedDate,@Longitude,@Latitude,@Website,@Established,@HeaderImage,@Avatar,@OriginId,@Address);", 
                                brewery, transaction);
                        var breweryId = await connection.QueryAsync<int>("SELECT last_value FROM breweries_seq;");
                        brewery.BreweryId = breweryId.SingleOrDefault();
                        if (brewery.Socials != null)
                        {
                            await connection.ExecuteAsync(
                                "INSERT INTO brewery_socials(brewery_id,site,url) VALUES(@BreweryId,@Site,@Url);",
                                brewery.Socials.Select(u => new { brewery.BreweryId, u.Site, u.Url }), transaction);
                        }
                        if (brewery.Members != null)
                        {
                            await connection.ExecuteAsync(
                                "INSERT INTO brewery_members(brewery_id,member_username,role) VALUES(@BreweryId,@MemberUsername,@Role);",
                                brewery.Members.Select(u => new { brewery.BreweryId, u.MemberUsername, u.Role }), transaction);
                        }
                        if (brewery.Beers != null)
                        {
                            await connection.ExecuteAsync(
                                "INSERT INTO brewery_beers(beer_id,brewery_id) VALUES(@BeerId,@BreweryId);",
                                brewery.Beers.Select(b => new { b.BeerId, brewery.BreweryId }), transaction);
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

        public async Task<int> UpdateAsync(Brewery brewery)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var result = await connection.ExecuteAsync(
                            "UPDATE Breweries set name = @Name,description = @Name, type = @Type, created_date = @CreatedDate, updated_date = @UpdatedDate," +
                            "longitude = @Longitude, latitude = @Latitude, website = @Website, established = @Established, header_image_url = @HeaderImage, avatar_url = @Avatar," +
                            "origin_id = @OriginId, address = @Address WHERE brewery_id = @BreweryId;", brewery,
                            transaction);
                        await UpdateBrewerySocialsAsync(connection, transaction, brewery);
                        await UpdateBreweryMembersAsync(connection, transaction, brewery);
                        transaction.Commit();
                        return result;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        throw;
                    }

                }
            }
        }

        public Task RemoveAsync(Brewery brewery)
        {
            throw new NotImplementedException();
        }

        public async Task<BreweryMember> GetSingleMemberAsync(int breweryId, string username)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                var members =
                    await connection.QueryAsync<BreweryMember>(
                        "SELECT brewery_id AS BreweryId, member_username AS MemberUsername FROM brewery_members WHERE brewery_id = @BreweryId and member_username = @MemberUsername;",
                        new { BreweryId = breweryId, MemberUsername = username });
                return members.SingleOrDefault();
            }
        }

        public async Task<IEnumerable<BreweryMember>> GetAllMembersAsync(int breweryId)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                var members =
                    await connection.QueryAsync<BreweryMember>(
                        "SELECT brewery_id AS BreweryId, member_username AS MemberUsername FROM brewery_members WHERE brewery_id = @BreweryId;",
                        new { BreweryId = breweryId });
                return members.ToList();
            }
        }

        public async Task DeleteMemberAsync(int breweryId, string username)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                using (var transaction = connection.BeginTransaction())
                {
                    await connection.ExecuteAsync("DELETE FROM brewery_Members WHERE brewery_id = @BreweryId and member_username = @MemberUsername;",
                            new { BreweryId = breweryId, MemberUsername = username }, transaction);
                }
            }
        }

        public async Task UpdateMemberAsync(BreweryMember breweryMember)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                using (var transaction = connection.BeginTransaction())
                {
                    await connection.ExecuteAsync(
                        "UPDATE brewery_members set Role = @Role, Confirmed = @Confirmed WHERE brewery_id = @BreweryId and member_username = @MemberUsername;",
                        breweryMember, transaction);
                }
            }
        }

        public async Task AddMemberAsync(BreweryMember breweryMember)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                using (var transaction = connection.BeginTransaction())
                {
                    await connection.ExecuteAsync("INSERT INTO brewery_members(brewery_id,member_username,Role,Confirmed) VALUES(@BreweryId,@MemberUsername,@Role,@Confirmed);",
                breweryMember, transaction);
                }
            }
        }

        public async Task<IEnumerable<BreweryMember>> GetMembershipsAsync(string username)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                var memberships =
                    await connection.QueryAsync<BreweryMember>("SELECT brewery_id AS BreweryId, member_username AS MemberUsername FROM brewery_members WHERE member_username = @MemberUsername;",
                        new {MemberUsername = username});
                return memberships.ToList();
            }
        }


        public async Task<IEnumerable<BreweryMember>> GetMembersAsyncAsync(int breweryId)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                var members =
                     await connection.QueryAsync<BreweryMember>(
                        "SELECT  brewery_id AS BreweryId, member_username AS MemberUsername FROM brewery_members WHERE brewery_id = @BreweryId;",
                        new { BreweryId = breweryId });
                return members.ToList();
            }
        }

        public async Task<IEnumerable<BrewerySocial>> GetBrewerySocialAsync(int breweryId)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                var socials =
                     await connection.QueryAsync<BrewerySocial>(
                        "SELECT * FROM BrewerySocials WHERE BreweryId = @BreweryId;",
                        new { BreweryId = breweryId });
                return socials;
            }
        }

        private async Task UpdateBrewerySocialsAsync(DbConnection connection, DbTransaction transaction, Brewery brewery)
        {
            var brewerySocials = await connection.QueryAsync<BrewerySocial>("SELECT brewery_id AS BreweryId, social_id AS SocialId, site, url FROM brewery_socials WHERE brewery_id = @BreweryId",
                new { brewery.BreweryId }, transaction);

            await connection.ExecuteAsync("DELETE FROM brewery_socials WHERE brewery_id = @BreweryId and social_id = @SocialId;",
                brewerySocials.Where(
                    u => brewery.Socials.All(s => u.SocialId != s.SocialId)),
                transaction);

            await connection.ExecuteAsync(
                "UPDATE brewery_socials set Site = @Site, Url = @Url WHERE brewery_id = @BreweryId and social_id = @SocialId;",
                brewery.Socials, transaction);

            await connection.ExecuteAsync("INSERT INTO brewery_socials(brewery_id,site,url) VALUES(@BreweryId,@Site,@Url);",
                brewery.Socials.Where(
                    s => brewerySocials.All(u => u.SocialId != s.SocialId)).Select(s => new { brewery.BreweryId, s.Site, s.Url }),
                transaction);
        }

        private async Task UpdateBreweryMembersAsync(DbConnection connection, DbTransaction transaction, Brewery brewery)
        {
            var breweryMembers = await connection.QueryAsync<BreweryMember>(
                "SELECT brewery_id AS BreweryId, member_username AS MemberUsername FROM brewery_members WHERE brewery_id = @BreweryId", new { brewery.BreweryId }, transaction);

            await connection.ExecuteAsync("DELETE FROM brewery_members WHERE brewery_id = @BreweryId and member_username = @MemberUsername;",
                breweryMembers.Where(bm => brewery.Members.All(m => bm.MemberUsername != m.MemberUsername)).Select(bm => new { brewery.BreweryId, bm.MemberUsername }), transaction);

            await connection.ExecuteAsync(
                "UPDATE brewery_members set role = @Role, confirmed = @Confirmed WHERE brewery_id = @BreweryId and member_username = @MemberUsername;",
                brewery.Members.Where(m => breweryMembers.Any(bm => m.MemberUsername == bm.MemberUsername)), transaction);

            await connection.ExecuteAsync("INSERT INTO brewery_members(brewery_id,member_username,role,confirmed) VALUES(@BreweryId,@MemberUsername,@Role,@Confirmed);",
                brewery.Members.Where(m => breweryMembers.All(bm => m.MemberUsername != bm.MemberUsername)).Select(bm => new { brewery.BreweryId, bm.MemberUsername,bm.Role,bm.Confirmed }), transaction);

        }

    }
}
