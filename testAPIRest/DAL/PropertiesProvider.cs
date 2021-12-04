using Dapper;
using PruebaApiREST.Configurations;
using PruebaApiREST.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PruebaApiREST.DAL
{
    public class PropertiesProvider : IPropertiesProvider
    {
        private PostgreSQLConfiguration _connectionString;
        public PropertiesProvider(PostgreSQLConfiguration connectionString)
        {
            _connectionString = connectionString;
        }

        protected Npgsql.NpgsqlConnection dbConnection()
        {
            return new Npgsql.NpgsqlConnection(_connectionString.ConnectionString);
        }

        public async Task<IEnumerable<Property>> GetAllPropertiesAsync()
        {
            var db = dbConnection();
            var sqlQuery = $@"SELECT * FROM public.""Property""";
            var result = await db.QueryAsync<Property>(sqlQuery, null);
            return result;
        }

        public async Task<Property> GetPropertyAsync(int id)
        {
            var db = dbConnection();
            var sqlQuery = $@"
                            SELECT * FROM public.""Property"" 
                            WHERE id = @Id";
            var result = await db.QueryFirstOrDefaultAsync<Property>(sqlQuery, new { Id = id });
            return result;
        }

        public async Task<bool> DeletePropertyAsync(int id)
        {
            var db = dbConnection();
            var sqlQuery = $@"
                            DELETE FROM public.""Property"" 
                            WHERE id = @Id
            ";
            var result = await db.ExecuteAsync(sqlQuery, new
            {
                Id = id
            });
            return result > 0;
        }

        public async Task<bool> InsertPropertyAsync(Property property)
        {
            var db = dbConnection();
            var sqlQuery = $@"
                            INSERT INTO public.""Property"" 
                            (title,address,description,createdat,status)
                            VALUES (@Title,@Address,@Description,@CreatedAt,@Status)
            ";
            var result = await db.ExecuteAsync(sqlQuery, new
            {
                Title = property.Title,
                Address = property.Address,
                Description = property.Description,
                CreatedAt = property.CreatedAt,
                Status = property.Status
            });
            return result > 0;
        }

        public async Task<bool> UpdatePropertyAsync(int id, Property property)
        {
            var db = dbConnection();
            var sqlQuery = $@"
                                UPDATE public.""Property"" SET
                                title = @Title, address = @Address, description = @Description, disabledat = @DisabledAt, status = @Status, UpdatedAt = @UpdatedAt
                                WHERE id = @Id
                            ";
            var result = await db.ExecuteAsync(sqlQuery, new
            {
                UpdatedAt = DateTime.Now,
                Id = id,
                Title = property.Title,
                Address = property.Address,
                Description = property.Description,
                DisabledAt = property.DisabledAt,
                Status = property.Status
            });
            return result > 0;
        }
    }
}
