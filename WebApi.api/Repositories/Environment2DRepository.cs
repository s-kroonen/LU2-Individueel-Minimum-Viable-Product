using Dapper;
using Microsoft.Data.SqlClient;
using WebApi.api.Models;

namespace WebApi.api.Repositories
{
    public class Environment2DRepository
    {
        private readonly string sqlConnectionString;

        public Environment2DRepository(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }

        public async Task<Environment2D> InsertAsync(Environment2D environment2D, string userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                if (environment2D.id == Guid.Empty)
                {
                    environment2D.id = Guid.NewGuid();
                }

                environment2D.UserId = userId;

                await sqlConnection.ExecuteAsync("INSERT INTO [environmentTable] (id, UserId, Name) VALUES (@id, @UserId, @Name)", environment2D);
                return environment2D;
            }
        }

        public async Task<Environment2D?> ReadAsync(Guid environment2DGuid)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                string environment2DString = environment2DGuid.ToString();
                return await sqlConnection.QuerySingleOrDefaultAsync<Environment2D>("SELECT * FROM [environmentTable] WHERE id = @id", new { EnvironmentId = environment2DString });
            }
        }

        public async Task<Environment2D?> ReadAsync(string environment2DString)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                Guid environment2DGuid = Guid.Parse(environment2DString);
                return await sqlConnection.QuerySingleOrDefaultAsync<Environment2D>("SELECT * FROM [environmentTable] WHERE id = @id", new { EnvironmentId = environment2DGuid });
            }
        }

        public async Task<IEnumerable<Environment2D>> ReadByUserAsync(string userId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                var environments = await sqlConnection.QueryAsync<Environment2D>("SELECT * FROM [environmentTable] WHERE UserId = @UserId", new { UserId = userId });
                return environments.Select(e => new Environment2D
                {
                    id = e.id,
                    UserId = e.UserId,
                    Name = e.Name
                });
            }
        }

        public async Task<IEnumerable<Environment2D>> ReadAllAsync()
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                var environments = await sqlConnection.QueryAsync<Environment2D>("SELECT * FROM [environmentTable]");
                return environments.Select(e => new Environment2D
                {
                    id = e.id,
                    UserId = e.UserId,
                    Name = e.Name
                });
            }
        }

        public async Task UpdateAsync(Environment2D environment2D)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("UPDATE [environmentTable] SET " +
                                                 "Name = @Name " +
                                                 "WHERE id = @id", environment2D);
            }
        }

        public async Task DeleteAsync(Guid environment2D)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [environmentTable] WHERE id = @id", new { EnvironmentId = environment2D });
            }
        }

        public async Task DeleteAsync(string environment2D)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [environmentTable] WHERE id = @id", new { EnvironmentId = environment2D });
            }
        }
    }
}