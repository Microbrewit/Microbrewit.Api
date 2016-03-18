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

    public class OtherDapperRepository : IOtherRepository
    {
        private DatabaseSettings _databaseSettings;
        public OtherDapperRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings.Value;
        }
        public async Task<IEnumerable<Other>> GetAllAsync()
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                return await connection.QueryAsync<Other>(@"SELECT o.other_id AS OtherId, name, type, custom FROM Others o");
            }
        }

        public async Task<Other> GetSingleAsync(int id)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                var other = await connection.QueryAsync<Other>("SELECT other_id AS OtherId, name, type, custom FROM Others WHERE other_id = @OtherId", new { OtherId = id });
                return other.SingleOrDefault();
            };
        }

        public async Task AddAsync(Other other)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var result = await connection.ExecuteAsync("INSERT INTO others(name,type,custom) VALUES(@Name, @Type, @Custom);",
                              new { other.Name, other.Type, other.Custom }, transaction);
                        var otherId = await connection.QueryAsync<int>("SELECT last_value FROM others_seq");
                        other.OtherId = otherId.SingleOrDefault();
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        public async Task<int> UpdateAsync(Other other)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        var result = await connection.ExecuteAsync(
                                "UPDATE Others set name=@Name, type=@Type, custom=@Custom WHERE other_id = @Id;",
                                new { other.Name, other.Type, other.Custom, Id = other.OtherId }, transaction);
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

        public async Task RemoveAsync(Other other)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        await connection.ExecuteAsync("DELETE FROM others WHERE other_id = @OtherId", new { other.OtherId }, transaction);
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
