using Dapper;
using GuardingUS.Models;
using Microsoft.Data.SqlClient;

namespace GuardingUS.Services
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Roles>> Get();
    }
    public class RoleRepository : IRoleRepository
    {
        private readonly string connectionString;

        public RoleRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Roles>> Get()
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryAsync<Roles>(@"SELECT * FROM AspNetRoles;");
        }
    }
}
