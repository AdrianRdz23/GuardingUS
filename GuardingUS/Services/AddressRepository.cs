using Dapper;
using GuardingUS.Models;
using Microsoft.Data.SqlClient;

namespace GuardingUS.Services
{
    public interface IAddressRepository
    {
        Task Create(Address address);
        Task Delete(string id);
        Task<IEnumerable<Address>> Get();
        Task<Address> GetById(string id);
        Task<IEnumerable<Address>> Search();
        Task Update(Address address);
    }
    public class AddressRepository : IAddressRepository
    {
        private readonly string connectionString;

        public AddressRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Create(Address address)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<string>(@"INSERT INTO Address(Id,[Name]) VALUES (NEWID(),@Name); SELECT SCOPE_IDENTITY();", address);

            address.Id = id;
        }

        public async Task<IEnumerable<Address>> Get()
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryAsync<Address>(@"SELECT * FROM Address");
        }

        public async Task<Address> GetById(string id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Address>(@"SELECT * FROM Address WHERE Id = @Id", new { id });
        }


        public async Task Update(Address address)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Address SET [Name] = @Name WHERE Id = @Id", address);
        }

        public async Task Delete(string id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"DELETE Address WHERE Id = @Id", new { id });
        }

        public async Task<IEnumerable<Address>> Search()
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryAsync<Address>(@"SELECT * FROM Address");
        }
    }
}
