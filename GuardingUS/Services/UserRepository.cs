using Dapper;
using GuardingUS.Models;
using GuardingUS.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;

namespace GuardingUS.Services
{
    public interface IUserRepository
    {
        Task<string> CreateUser(Users user);
        Task Delete(int id);
        Task<Users> FindByEmail(string normalizedEmail);
        Task<IEnumerable<IdentityUser>> Get(string id);
        //Task<IEnumerable<Users>> Get();
        Task<Users> GetById(int id);
        Task Update(Users user);
    }
    public class UserRepository : IUserRepository
    {
        private readonly string connectionString;

        public UserRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<string> CreateUser(Users user)
        {
            //user.NormalizedEmail = user.Email.ToUpper();
            using var connection = new SqlConnection(connectionString);

            var id = await connection.QuerySingleAsync<string>(@"INSERT INTO Users([Name], PhoneNumber,Email, NormalizedEmail, PasswordHash, IdRole) VALUES (@Name, @PhoneNumber,@Email, @NormalizedEmail, @PasswordHash, 3); SELECT SCOPE_IDENTITY();", user);

            return id;


        }

        public async Task<Users> FindByEmail(string normalizedEmail)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QuerySingleOrDefaultAsync<Users>("SELECT * FROM Users WHERE NormalizedEmail = @normalizedEmail", new { normalizedEmail });
        }

        public async Task<IEnumerable<IdentityUser>> Get(string id)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryAsync<IdentityUser>("SELECT * FROM AspNetUsers WHERE Id<> @Id", new {id});
        }

        

        public async Task<Users> GetById(int id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Users>(@"SELECT * FROM Users WHERE Id = @Id;",new { id });
        }

        public async Task Update(Users user)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Users SET IdRole = @IdRole WHERE Id = @Id", user);
        }

        public async Task Delete(int id)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Homes SET IdUser=10;DELETE Users WHERE Id = @Id", new { id });
        }
    }
}
