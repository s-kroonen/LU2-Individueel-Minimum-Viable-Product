using Dapper;
using Microsoft.Data.SqlClient;
using WebApi.api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.api.Repositories
{
    public class UserRepository
    {
        private readonly string sqlConnectionString;

        public UserRepository(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }

        // INSERT: Returns the inserted User, or null if failed
        public async Task<User?> InsertAsync(User user)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    await sqlConnection.OpenAsync();
                    var rowsAffected = await sqlConnection.ExecuteAsync(
                        "INSERT INTO [Users] (username, password) VALUES (@Username, @Password)",
                        user
                    );

                    return rowsAffected > 0 ? user : null;
                }
            }
            catch
            {
                return null;
            }
        }

        // READ: Returns the found User, or null if not found
        public async Task<User?> ReadAsync(string username)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    return await sqlConnection.QuerySingleOrDefaultAsync<User>(
                        "SELECT * FROM [Users] WHERE username = @Username",
                        new { Username = username }
                    );
                }
            }
            catch
            {
                return null;
            }
        }

        // READ ALL: Returns all users, or an empty list if none found
        public async Task<IEnumerable<User>> ReadAsync()
        {
            try
            {
                using (var sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    return await sqlConnection.QueryAsync<User>("SELECT * FROM [Users]");
                }
            }
            catch
            {
                return new List<User>();
            }
        }

        // UPDATE: Returns the updated User, or null if update failed
        public async Task<User?> UpdateAsync(User user)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    var rowsAffected = await sqlConnection.ExecuteAsync(
                        "UPDATE [Users] SET password = @Password WHERE username = @Username",
                        user
                    );

                    return rowsAffected > 0 ? user : null;
                }
            }
            catch
            {
                return null;
            }
        }

        // DELETE: Returns the deleted User, or null if delete failed
        public async Task<User?> DeleteAsync(string username)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(sqlConnectionString))
                {
                    // Read the user before deleting (to return it if successful)
                    var userToDelete = await ReadAsync(username);
                    if (userToDelete == null) return null;

                    var rowsAffected = await sqlConnection.ExecuteAsync(
                        "DELETE FROM [Users] WHERE username = @Username",
                        new { Username = username }
                    );

                    return rowsAffected > 0 ? userToDelete : null;
                }
            }
            catch
            {
                return null;
            }
        }
    }
}
