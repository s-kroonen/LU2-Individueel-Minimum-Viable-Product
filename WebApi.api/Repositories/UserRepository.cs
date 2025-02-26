using Dapper;
using System.Data;
using System.Data.Common;

#if DEBUG
using Microsoft.Data.SqlClient; // Gebruik MSSQL in DEBUG
#else
using MySql.Data.MySqlClient; // Gebruik MySQL in productie
#endif

using WebApi.api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.api.Repositories
{
    public class UserRepository
    {
        private readonly string sqlConnectionString;
        private readonly ILogger<UserRepository> _logger;
        private readonly bool isMySql; // Controle of we MySQL gebruiken

        public UserRepository(string sqlConnectionString, ILogger<UserRepository> logger)
        {
            this.sqlConnectionString = sqlConnectionString;
            _logger = logger;
#if DEBUG
            isMySql = false;  // MSSQL in debug-modus
#else
            isMySql = true;   // MySQL in productie
#endif
        }

        // Helper method om een database-verbinding te maken
        private DbConnection CreateConnection()
        {
#if !DEBUG
            return new MySqlConnection(sqlConnectionString);
#else
            return new SqlConnection(sqlConnectionString);
#endif
        }

        // INSERT: Returns the inserted User, or null if failed
        public async Task<User?> InsertAsync(User user)
        {
            try
            {
                using (var dbConnection = CreateConnection())
                {
                    await dbConnection.OpenAsync();
                    string query = isMySql
                        ? "INSERT INTO Users (username, password) VALUES (@Username, @Password)"
                        : "INSERT INTO Users (username, password) VALUES (@Username, @Password)";

                    return await dbConnection.QuerySingleOrDefaultAsync<User>(query, user);
                }

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        // READ: Returns the found User, or null if not found
        public async Task<User?> ReadAsync(string username)
        {
            try
            {
                using (var dbConnection = CreateConnection())
                {
                    await dbConnection.OpenAsync();

                    string query = isMySql
                        ? "SELECT * FROM Users WHERE username = @Username"
                        : "SELECT * FROM Users WHERE username = @Username";

                    return await dbConnection.QuerySingleOrDefaultAsync<User>(query, new { Username = username });
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
                using (var dbConnection = CreateConnection())
                {
                    await dbConnection.OpenAsync();

                    string query = isMySql
                        ? "SELECT * FROM Users"
                        : "SELECT * FROM Users";

                    return await dbConnection.QueryAsync<User>(query);
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
                using (var dbConnection = CreateConnection())
                {
                    await dbConnection.OpenAsync();

                    string query = isMySql
                        ? "UPDATE Users SET password = @Password WHERE username = @Username"
                        : "UPDATE Users SET password = @Password WHERE username = @Username";

                    var rowsAffected = await dbConnection.ExecuteAsync(query, user);
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
                using (var dbConnection = CreateConnection())
                {
                    await dbConnection.OpenAsync();

                    var userToDelete = await ReadAsync(username);
                    if (userToDelete == null) return null;

                    string query = isMySql
                        ? "DELETE FROM Users WHERE username = @Username"
                        : "DELETE FROM Users WHERE username = @Username";

                    var rowsAffected = await dbConnection.ExecuteAsync(query, new { Username = username });
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
