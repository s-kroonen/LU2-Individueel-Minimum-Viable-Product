﻿using Dapper;
using Microsoft.Data.SqlClient;
using WebApi.api.Models;
namespace WebApi.api.Repositories
{
    public class Object2DRepository
    {
        private readonly string sqlConnectionString;

        public Object2DRepository(string sqlConnectionString)
        {
            this.sqlConnectionString = sqlConnectionString;
        }

        public async Task<Object2D> InsertAsync(Object2D object2D, Guid environmentId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                if (object2D.ObjectId == Guid.Empty)
                {
                    object2D.ObjectId = Guid.NewGuid();
                }

                object2D.EnvironmentId = environmentId;

                await sqlConnection.ExecuteAsync("INSERT INTO [Object2D] (ObjectId, EnvironmentId, PrefabId, PositionX, PositionY, ScaleX, ScaleY, RotationZ, SortingLayer) VALUES (@ObjectId, @EnvironmentId, @PrefabId, @PositionX, @PositionY, @ScaleX, @ScaleY, @RotationZ, @SortingLayer)", object2D);
                return object2D;
            }
        }

        public async Task<Object2D?> ReadAsync(Guid object2DId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QuerySingleOrDefaultAsync<Object2D>("SELECT * FROM [Object2D] WHERE ObjectId = @ObjectId", new { ObjectId = object2DId });
            }
        }

        public async Task<IEnumerable<Object2D>> ReadAllAsync()
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                return await sqlConnection.QueryAsync<Object2D>("SELECT * FROM [Object2D]");
            }
        }

        public async Task UpdateAsync(Object2D object2D)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("UPDATE [Object2D] SET " +
                                                 "PrefabId = @PrefabId, " +
                                                 "PositionX = @PositionX, " +
                                                 "PositionY = @PositionY, " +
                                                 "ScaleX = @ScaleX, " +
                                                 "ScaleY = @ScaleY, " +
                                                 "RotationZ = @RotationZ, " +
                                                 "SortingLayer = @SortingLayer " +
                                                 "WHERE ObjectId = @ObjectId", object2D);
            }
        }

        public async Task DeleteAsync(Guid object2DId)
        {
            using (var sqlConnection = new SqlConnection(sqlConnectionString))
            {
                await sqlConnection.ExecuteAsync("DELETE FROM [Object2D] WHERE ObjectId = @ObjectId", new { ObjectId = object2DId });
            }
        }
    }
}