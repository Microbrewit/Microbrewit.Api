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
    public class UserDapperRepository : IUserRepository
    {
       private DatabaseSettings _databaseSettings;
       public UserDapperRepository(IOptions<DatabaseSettings> databaseSettings)
       {
           _databaseSettings = databaseSettings.Value;
       }

        public async Task<IList<User>> GetAllAsync()
        {
            using (DbConnection context = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                var users = await context.QueryAsync<User>("SELECT user_id AS Id, username, email, settings, gravatar, longitude, latitude, " +
                                                           "header_image_url, avatar_url, firstname, lastname FROM Users;");
                foreach (var user in users)
                {
                    var userSocials = await context.QueryAsync<UserSocial>("SELECT * FROM UserSocials WHERE user_id = @Id",
                        new { user.Id });
                    user.Socials = userSocials.ToList();
                    var breweryMembers =
                        await context.QueryAsync<BreweryMember, Brewery, BreweryMember>("SELECT * FROM BreweryMembers bm " +
                                                                             "LEFT JOIN Breweries b ON bm.BreweryId = b.BreweryId " +
                                                                             "WHERE bm.member_user_id = @Id",
                            (breweryMember, brewery) =>
                            {
                                breweryMember.Brewery = brewery;
                                return breweryMember;
                            },
                            new { user.Id }, splitOn: "BreweryId");
                    user.Breweries = breweryMembers.ToList();

                    var userBeers = await context.QueryAsync<UserBeer, Beer, SRM, ABV, IBU, BeerStyle, UserBeer>(
                        "SELECT * " +
                        "FROM UserBeers ub " +
                        "LEFT JOIN Beers b ON ub.BeerId = b.BeerId " +
                        "LEFT JOIN SRMs s ON s.SrmId = b.BeerId " +
                        "LEFT JOIN ABVs a ON a.AbvId = b.BeerId " +
                        "LEFT JOIN IBUs i ON i.IbuId = b.BeerId " +
                        "LEFT JOIN BeerStyles bs ON bs.BeerStyleId = b.BeerStyleId " +
                        "WHERE ub.user_id = @Id", (userBeer, beer, srm, abv, ibu, beerStyle) =>
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
                        }, new { user.Id }, splitOn: "BeerId,SrmId,AbvId,IbuId,BeerStyleId");
                    user.Beers = userBeers.ToList();
                }
                return users.ToList();

            }
        }

        public async Task<User> GetSingleAsync(string userId)
        {
            using (DbConnection context = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                //Dapper returns IEnumerable, but result should always be single row.
                var users = await context.QueryAsync<User>("SELECT * FROM Users WHERE Username = @Username;", new { Username = userId });
                var user = users.SingleOrDefault();
                if (user == null) return null;

                var userSocials = await context.QueryAsync<UserSocial>("SELECT * FROM UserSocials WHERE Username = @Username",
                    new { user.Username });
                user.Socials = userSocials.ToList();
                var breweryMembers =
                    context.Query<BreweryMember, Brewery, BreweryMember>(
                        "SELECT * FROM BreweryMembers bm " +
                        "LEFT JOIN Breweries b ON bm.BreweryId = b.BreweryId " +
                        "WHERE bm.MemberUsername = @Username",
                        (breweryMember, brewery) =>
                        {
                            breweryMember.Brewery = brewery;
                            return breweryMember;
                        },
                        new { user.Username }, splitOn: "BreweryId");
                user.Breweries = breweryMembers.ToList();

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
                    }, new { user.Username }, splitOn: "BeerId,SrmId,AbvId,IbuId,BeerStyleId");
                user.Beers = userBeers.ToList();
                user.Roles = new List<string>();
                // var identityUser = await _authRepository.FindUser(user.Username);
                // if (identityUser != null && identityUser.Roles.Any())
                // {
                //     if (identityUser.Roles.Any(r => r.RoleId == "1"))
                //         user.Roles.Add("Admin");
                // }
                return user;
            }
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
                        await context.ExecuteAsync(
                            "INSERT users(user_id,username,email,settings,gravatar,latitude,longitude,header_image_url,avatar_url,firstname,lastname)" +
                            "VALUES(@Username,@Email,@Settings,@Gravatar,@Latitude,@Longitude,@HeaderImage,@Avatar,@Firstname,@Lastname);",
                            user, transaction);
                        if (user.Socials != null)
                        {
                            await context.ExecuteAsync(
                                "INSERT UserSocials(user_id,site,url) VALUES(@Id,@Site,@Url);",
                                user.Socials.Select(u => new { user.Id, u.Site, u.Url }), transaction);
                        }

                        var userSocials =
                            await context.QueryAsync<UserSocial>("SELECT * FROM UserSocials WHERE Username = @Username",
                               new { user.Username }, transaction);
                        user.Socials = userSocials.ToList();
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
                    catch (Exception e)
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
                    s => userSocials.All(u => s.Username != u.Username && u.SocialId != s.SocialId)).Select(s => new { user.Username, s.Site, s.Url }),
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
                    s => userSocials.All(u => s.Username != u.Username && u.SocialId != s.SocialId)).Select(s => new { user.Username, s.Site, s.Url }),
                transaction);
        }
    }
}
