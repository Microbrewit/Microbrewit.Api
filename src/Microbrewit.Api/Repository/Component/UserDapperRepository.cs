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
    public class UserDapperRepository : IUserRepository
    {
        private DatabaseSettings _databaseSettings;
        private ILogger<UserDapperRepository> _logger; 
        public UserDapperRepository(IOptions<DatabaseSettings> databaseSettings, ILogger<UserDapperRepository> logger)
        {
            _logger = logger;
            _databaseSettings = databaseSettings.Value;
        }

        public async Task<IList<User>> GetAllAsync()
        {
            using (DbConnection context = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                var users = await context.QueryAsync<User>("SELECT user_id AS UserId, username, email, settings, gravatar, longitude, latitude, " +
                                                           "header_image_url, avatar_url, firstname, lastname, is_email_comfirmed AS EmailConfirmed FROM users;");
                foreach (var user in users)
                {
                    await GetUserProperties(context, user);
                }
                return users.ToList();

            }
        }

        public async Task<User> GetSingleByUsernameAsync(string username)
        {
            using (DbConnection context = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                var users = await context.QueryAsync<User>("SELECT user_id AS UserId, username, email, settings, gravatar, longitude, latitude, " +
                                                            "header_image_url, avatar_url, firstname, lastname, is_email_comfirmed AS EmailConfirmed FROM users WHERE username = @Username;",
                                                            new { Username = username });
                var user = users.SingleOrDefault();
                if (user != null)
                    await GetUserProperties(context, user);
                return user;
            }
        }

        public async Task<User> GetSingleByUserIdAsync(string userId)
        {
            using (DbConnection context = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                var users = await context.QueryAsync<User>("SELECT user_id AS UserId, username, email, settings, gravatar, longitude, latitude, " +
                                                            "header_image_url, avatar_url, firstname, lastname, is_email_comfirmed AS EmailConfirmed FROM users WHERE user_id = @UserId;",
                                                            new { UserId = userId });
                var user = users.SingleOrDefault();
                if (user != null)
                    await GetUserProperties(context, user);
                return user;
            }
        }

        private static async Task GetUserProperties(DbConnection context, User user)
        {
            var userSocials =
                await
                    context.QueryAsync<UserSocial>(
                        "SELECT user_id AS UserId, social_id AS SocialId, site, url FROM user_socials WHERE user_id = @UserId",
                        new { user.UserId });
            user.Socials = userSocials.ToList();
            var sql = "SELECT bm.brewery_id AS BreweryId,bm.user_id AS UserId,role, confirmed, " +
                      " b.brewery_id AS BreweryId, b.name, description, type, created_date AS CreatedDate, updated_date AS UpdatedDate," +
                      " longitude, latitude, website, established, header_image_url AS HeaderImageUrl," +
                      " avatar_url AS AvatarUrl, b.origin_id AS OriginId, address, o.origin_id AS OriginId, o.name  " +
                      " FROM brewery_members bm " +
                      " LEFT JOIN Breweries b ON bm.Brewery_Id = b.Brewery_Id " +
                      " LEFT JOIN origins o ON b.origin_id = o.origin_id " +
                      " WHERE bm.user_id = @UserId";
            var breweryMembers =
                await context.QueryAsync<BreweryMember, Brewery, Origin, BreweryMember>(sql,
                    (breweryMember, brewery, origin) =>
                    {
                        breweryMember.Brewery = brewery;
                        if (brewery != null)
                            brewery.Origin = origin;
                        return breweryMember;
                    },
                    new { user.UserId }, splitOn: "BreweryId,OriginId");
            user.Breweries = breweryMembers.ToList();

            ;
            var userBeerSql =
                $"SELECT ub.user_id AS UserId,ub.beer_id AS BeerId, b.beer_id AS BeerId, b.name, b.beerstyle_id AS BeerStyleId, b.created_date AS CreatedDate, b.updated_date AS UpdatedDate, b.fork_of_id AS ForkOfId," +
                "bs.beerstyle_id AS BeerStyleId, bs.name, bs.superstyle_id AS SuperStyleId, bs.og_low AS OGLow , bs.og_high AS OGHigh, bs.fg_low AS FGLow, bs.fg_high FGHigh," +
                "bs.ibu_low AS IBULow, bs.ibu_high AS IBUHigh, bs.srm_low AS SRMLow, bs.srm_high AS SRMHig, bs.abv_low AS ABVLow, bs.abv_high AS ABVHigh, bs.comments, " +
                "r.recipe_id AS RecipeId, r.volume, r.notes, r.og, r.fg, r.efficiency, r.total_boil_time AS TotalBoilTime," +
                "s.srm_id AS SrmId, s.standard, s.mosher, s.daniels, s.morey, " +
                "a.abv_id AS AbvId, a.standard, a.miller, a.advanced, a.advanced_alternative AS AdvancedAlternative, a.simple, a.simple_alternative AS SimpleAlternative," +
                "i.ibu_id AS IbuId, i.standard, i.tinseth, i.rager " +
                "FROM user_beers ub " +
                " INNER JOIN beers b ON ub.beer_id = b.beer_id " +
                "LEFT JOIN beerstyles bs ON bs.beerstyle_id = b.beerstyle_id " +
                "LEFT JOIN recipes r ON r.recipe_Id = b.beer_id " +
                "LEFT JOIN srms s ON s.srm_id = b.beer_id " +
                "LEFT JOIN abvs a ON a.abv_id = b.beer_id " +
                "LEFT JOIN ibus i ON i.ibu_id = b.beer_id " +
                "WHERE ub.user_id = @UserId";

            var userBeers = await context.QueryAsync<UserBeer, Beer, SRM, ABV, IBU, BeerStyle, UserBeer>(
                userBeerSql, (userBeer, beer, srm, abv, ibu, beerStyle) =>
                {
                    userBeer.Beer = beer;
                    if (srm != null)
                        beer.SRM = srm;
                    if (abv != null)
                        beer.ABV = abv;
                    if (ibu != null)
                        beer.IBU = ibu;
                    if (beerStyle != null)
                        beer.BeerStyle = beerStyle;
                    return userBeer;
                }, new { user.UserId }, splitOn: "BeerId,BeerStyleId,SrmId,AbvId,IbuId");
            user.Beers = userBeers.ToList();
        }

        public async Task AddAsync(User user)
        {
            using (DbConnection context = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {

                        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password,12);
                        user.UserId = Guid.NewGuid().ToString();

                        await context.ExecuteAsync(
                            "INSERT INTO users(user_id,username,email,settings,gravatar,latitude,longitude,header_image_url,avatar_url,firstname,lastname,password)" +
                            "VALUES(@UserId,@Username,@Email,@Settings,@Gravatar,@Latitude,@Longitude,@HeaderImage,@Avatar,@Firstname,@Lastname,@Password);",
                            user, transaction);
                        if (user.Socials != null)
                        {
                            await context.ExecuteAsync(
                                "INSERT INTO user_socials(user_id,site,url) VALUES(@UserId,@Site,@Url);",
                                user.Socials.Select(u => new { user.UserId, u.Site, u.Url }), transaction);
                        }

                        //var userSocials =
                        //    await context.QueryAsync<UserSocial>("SELECT user_id AS UserId,site,url FROM UserSocials WHERE user_id = @UserId",
                        //       new { user.UserId }, transaction);
                        //user.Socials = userSocials.ToList();
                        transaction.Commit();
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

        public async Task<int> UpdateAsync(User user)
        {
            using (DbConnection context = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        var result = await context.ExecuteAsync(
                              "UPDATE Users set Username = @Username, Email = @Email, Settings = @Settings ,Gravatar = @Gravatar," +
                              " Latitude = @Latitude, Longitude = @Longitude, HeaderImage = @HeaderImage, Avatar = @Avatar " +
                              "WHERE Username = @Username;",
                              user, transaction);
                        await UpdateUserSocialsAsync(context, transaction, user);
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

        public Task RemoveAsync(User user)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserSocial> GetUserSocials(string username)
        {
            using (DbConnection context = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                return context.Query<UserSocial>("SELECT * FROM UserSocials WHERE Username = @Username",
                    new { Username = username });
            }
        }

        public async Task<IEnumerable<UserBeer>> GetAllUserBeersAsync(string username)
        {
            using (DbConnection context = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                var userBeers = await context.QueryAsync<UserBeer, Beer, SRM, ABV, IBU, BeerStyle, UserBeer>(
                    "SELECT * " +
                    "FROM UserBeers ub " +
                    "LEFT JOIN Beers b ON ub.BeerId = b.BeerId " +
                    "LEFT JOIN SRMs s ON s.SrmId = b.BeerId " +
                    "LEFT JOIN ABVs a ON a.AbvId = b.BeerId " +
                    "LEFT JOIN IBUs i ON i.IbuId = b.BeerId " +
                    "LEFT JOIN BeerStyles bs ON bs.BeerStyleId = b.BeerStyleId " +
                    "WHERE ub.Username = @Username", (userBeer, beer, srm, abv, ibu, beerStyle) =>
                    {
                        userBeer.Beer = beer;
                        if (srm != null)
                            beer.SRM = srm;
                        if (abv != null)
                            beer.ABV = abv;
                        if (ibu != null)
                            beer.IBU = ibu;
                        if (beerStyle != null)
                            beer.BeerStyle = beerStyle;
                        return userBeer;
                    }, new { Username = username }, splitOn: "BeerId,SrmId,AbvId,IbuId,BeerStyleId");
                return userBeers;
            }
        }

        public bool ExistsUsername(string username)
        {
            using (var context = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                var users = context.Query<User>("SELECT * FROM users WHERe username = @Username;", new { Username = username});
                return users.Any();
            }
        }

        private void UpdateUserSocials(DbConnection context, DbTransaction transaction, User user)
        {
            var userSocials = context.Query<UserSocial>("SELECT * FROM UserSocials WHERE Username = @Username",
                new { user.Username }, transaction);

            context.Execute("DELETE FROM UserSocials WHERE Username = @Username and SocialId = @SocialId;",
                userSocials.Where(
                    u => user.Socials.All(s => u.SocialId != s.SocialId)),
                transaction);

            context.Execute(
                "UPDATE UserSocials set Site = @Site, Url = @Url WHERE Username = @Username and SocialId = @SocialId;",
                user.Socials, transaction);

            context.Execute("INSERT UserSocials(Username,Site,Url) VALUES(@Username,@Site,@Url);",
                user.Socials.Where(
                    s => userSocials.All(u => s.UserId != u.UserId && u.SocialId != s.SocialId)).Select(s => new { user.Username, s.Site, s.Url }),
                transaction);
        }

        private async Task UpdateUserSocialsAsync(DbConnection context, DbTransaction transaction, User user)
        {
            var userSocials = await context.QueryAsync<UserSocial>("SELECT * FROM UserSocials WHERE Username = @Username",
                new { user.Username }, transaction);

            await context.ExecuteAsync("DELETE FROM UserSocials WHERE Username = @Username and SocialId = @SocialId;",
                userSocials.Where(
                    u => user.Socials.All(s => u.SocialId != s.SocialId)),
                transaction);

            await context.ExecuteAsync(
                "UPDATE UserSocials set Site = @Site, Url = @Url WHERE Username = @Username and SocialId = @SocialId;",
                user.Socials, transaction);

            await context.ExecuteAsync("INSERT UserSocials(Username,Site,Url) VALUES(@Username,@Site,@Url);",
                user.Socials.Where(
                    s => userSocials.All(u => s.UserId != u.UserId && u.SocialId != s.SocialId)).Select(s => new { user.Username, s.Site, s.Url }),
                transaction);
        }


    }
}
