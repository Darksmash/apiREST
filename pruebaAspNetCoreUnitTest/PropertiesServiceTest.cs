using PruebaApiREST.Configurations;
using PruebaApiREST.DAL;
using System.Threading.Tasks;
using Xunit;

namespace pruebaAspNetCoreUnitTest
{
    public class PropertiesServiceTest
    {
        PostgreSQLConfiguration connectionString = new PostgreSQLConfiguration("Server=127.0.0.1;Port=5432;DataBase=Activities;User Id=postgres; Password=root;");
        protected Npgsql.NpgsqlConnection dbConnection()
        {
            return new Npgsql.NpgsqlConnection(connectionString.ConnectionString);
        }

        [Fact]
        public async Task GetAllPropertiesAsync()
        {
            var properties = new PropertiesProvider(connectionString);
            var result = await properties.GetAllPropertiesAsync();

            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetPropertyAsync()
        {
            var properties = new PropertiesProvider(connectionString);
            var realResult = await properties.GetPropertyAsync(14);
            var fakeResult = await properties.GetPropertyAsync(999);

            Assert.NotNull(realResult);
            Assert.Null(fakeResult);
        }

        [Fact]
        public async Task DeletePropertyAsync()
        {
            var properties = new PropertiesProvider(connectionString);
            var fakeResult = await properties.DeletePropertyAsync(999);

            Assert.False(fakeResult);
        }
    }
}
