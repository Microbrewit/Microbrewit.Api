using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;
using Microbrewit.Api.Model.Database;
using Microbrewit.Api.Repository.Interface;
using Microbrewit.Api.Settings;

namespace Microbrewit.Api.Repository.Component
{
    public class GlassDapperRepository : IGlassRepository
    {
        private DatabaseSettings _databaseSettings;
        public GlassDapperRepository(IOptions<DatabaseSettings> databaseSettings)
        {
            _databaseSettings = databaseSettings.Value;
        }
        public async Task<IEnumerable<Glass>> GetAllAsync()
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                var glasss = await connection.QueryAsync<Glass>(@"SELECT glass_id AS GlassId, name FROM Glasses");
                return glasss.ToList();
            }
        }

        public async Task<Glass> GetSingleAsync(int id)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                var glass =
                    await
                        connection.QueryAsync<Glass>(@"SELECT glass_id AS GlassId, name FROM Glasses WHERE glass_id = @GlassId",
                            new { GlassId = id });
                return glass.SingleOrDefault();
            }
        }

        public async Task AddAsync(Glass glass)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    await connection.QueryAsync<int>(@"INSERT INTO Glasses(name) VALUES(@Name);", new { glass.Name}, transaction);
                    var glassId = await connection.QueryAsync<int>("SELECT last_value FROM glasses_seq", transaction: transaction);
                    glass.GlassId = glassId.SingleOrDefault();
                    transaction.Commit();
                }
            }
        }

        public async Task<int> UpdateAsync(Glass glass)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    var result = await connection.ExecuteAsync(@"UPDATE glasses set name = @Name WHERE glass_id = @GlassId", new { glass.Name, glass.GlassId }, transaction);
                    transaction.Commit();
                    return result;
                }
            }
        }

        public async Task RemoveAsync(Glass glass)
        {
            using (DbConnection connection = new NpgsqlConnection(_databaseSettings.DbConnection))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    await connection.ExecuteAsync(@"DELETE FROM glasses WHERE glass_id = @GlassId;", new { glass.GlassId },
                        transaction);
                    transaction.Commit();
                }
            }
        }
    }
}
