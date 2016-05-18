using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Settings;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Microbrewit.Api.Repository.Component
{
    public class OriginDapperRepository : IOriginRespository
    {
        private DatabaseSettings _databaseSettings;
        public OriginDapperRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings.Value;
        }
     public async Task<IEnumerable<Origin>> GetAllAsync(int from, int size)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                var origins = await connection.QueryAsync<Origin>(@"SELECT origin_id AS OriginId, name FROM origins LIMIT @Size OFFSET @From;", new {From = from, Size = size});
                return origins.ToList();
            }
        }

        public async Task<Origin> GetSingleAsync(int id)
        {
            using (var context = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                var origin =
                    await
                        context.QueryAsync<Origin>(@"SELECT origin_id AS OriginId, name FROM Origins WHERE origin_id = @OriginId",
                            new {OriginId = id});
                return origin.SingleOrDefault();
            }
        }

        public async Task AddAsync(Origin origin)
        {
            using (var context = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    var result = await context.QueryAsync<int>(@"INSERT INTO Origins(name) VALUES(@Name);", new { origin.Name }, transaction);
                    var originId = await context.QueryAsync<int>("SELECT last_value FROM origins_seq;");
                    origin.OriginId = originId.SingleOrDefault();
                    transaction.Commit();
                }
            }
        }

        public async Task<int> UpdateAsync(Origin origin)
        {
            using (var context = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    var result = await context.ExecuteAsync(@"UPDATE origins set name = @Name WHERE origin_id = @OriginId", new { origin.Name, origin.OriginId }, transaction);
                    transaction.Commit();
                    return result;
                }
            }
        }

        public async Task RemoveAsync(Origin origin)
        {
            using (var context = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    await context.ExecuteAsync(@"DELETE FROM origins WHERE origin_id = @OriginId;", new { origin.OriginId },
                        transaction);
                    transaction.Commit();
                }
            }
        }
    }
}
