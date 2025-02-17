using Dapper;
using Microsoft.Data.SqlClient;
using WebApi.api.Models;

namespace WebApi.api.Repositories
{
    public class UserRepository
    {
        private readonly string sqlConnectionString;

        public UserRepository(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }

        public async Task<User> InsertAsync(User user)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                var environmentId = await sqlConnection.ExecuteAsync("INSERT INTO [Users] (Id, username, password) VALUES (@Id, @Username, @Password)", user);
                return user;
            }
        }

        public async Task<User?> ReadAsync(Guid id)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<User>("SELECT * FROM [Users] WHERE Id = @Id", new { id });
            }
        }

        public async Task<IEnumerable<User>> ReadAsync()
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<User>("SELECT * FROM [Users]");
            }
        }

        public async Task UpdateAsync(User environment)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("UPDATE [Users] SET " +
                                                 "username = @Username, " +
                                                 "password = @Password"
                                                 , environment);

            }
        }

        public async Task DeleteAsync(Guid id)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [Users] WHERE Id = @Id", new { id });
            }
        }

    }
}