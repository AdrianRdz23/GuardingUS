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
        Task Delete(string userid, string id);
        Task<Users> FindByEmail(string normalizedEmail);
        Task<IEnumerable<IdentityUser>> Get(string id);
        //Task<IEnumerable<Users>> Get();
        Task<Users> GetById(string id);
        Task<string> GetRoleId(string id);
        Task Update(Users user);
        Task UpdateUnknown(string iduser);
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

            return await connection.QueryAsync<IdentityUser>("SELECT * FROM AspNetUsers WHERE Id<> @Id", new { id });
        }



        public async Task<Users> GetById(string id)
        {
            using var connection = new SqlConnection(connectionString);
            return await connection.QueryFirstOrDefaultAsync<Users>(@"SELECT * FROM AspNetUsers WHERE Id = @Id;", new { id });
        }

        public async Task Update(Users user)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Users SET IdRole = @IdRole WHERE Id = @Id", user);
        }

        public async Task<string> GetRoleId(string id)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryFirstOrDefaultAsync<string>("SELECT AspNetUserRoles.RoleId FROM AspNetUserRoles WHERE UserId = @Id", new { id });
        }

        public async Task Delete(string userid,string id)
        {
            using var connection = new SqlConnection(connectionString);
            //await connection.QuerySingleAsync<string>(@"UPDATE Homes SET IdUser='3cd9a931-c383-4867-8ec2-097caf93344a'; DELETE FROM AspNetUserRoles WHERE UserId = @UserId; DELETE FROM AspNetUsers WHERE Id = @Id;", new { iduser, id });
            await connection.ExecuteAsync(@"DELETE FROM AspNetUserRoles WHERE UserId = @UserId; DELETE FROM AspNetUsers WHERE Id = @Id;", new { userid, id });
        }

        public async Task UpdateUnknown(string iduser)
        {
            using var connection = new SqlConnection(connectionString);
            await connection.ExecuteAsync(@"UPDATE Homes SET IdUser='3cd9a931-c383-4867-8ec2-097caf93344a' WHERE IdUser = @IdUser;", new {iduser});
            await connection.ExecuteAsync(@"UPDATE UserNotifications SET IdUser = '3cd9a931-c383-4867-8ec2-097caf93344a' WHERE IdUser = @IdUser;", new { iduser });
            await connection.ExecuteAsync(@"UPDATE Notifications SET IdUser = '3cd9a931-c383-4867-8ec2-097caf93344a' WHERE IdUser = @IdUser;", new { iduser });
        }
    }
}
