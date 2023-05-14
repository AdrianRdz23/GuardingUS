using Dapper;
using GuardingUS.Models;
using GuardingUS.Models.ViewModels;
using Microsoft.Data.SqlClient;

namespace GuardingUS.Services
{
    public interface IHomeRepository
    {
        Task Create(Homes homes);
        Task Delete(string id);
        Task<IEnumerable<Homes>> Get(string address);
        Task<IEnumerable<HomeVM>> Get();
        Task<Homes> GetById(string id);
        Task Update(AddHomeVM home);
    }
    public class HomeRepository: IHomeRepository
    {
        private readonly string connectionString;

        public HomeRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<HomeVM>> Get()
        {
            using var connection = new SqlConnection(connectionString);

            //return await connection.QueryAsync<Homes>("SELECT Id,Number,Cars,IdAddress,IdUser FROM Homes ORDER BY Id");
            return await connection.QueryAsync<HomeVM>("SELECT Homes.Id, Homes.Number, Homes.Cars, [Address].[Name] as [Address], AspNetUsers.UserName FROM Homes JOIN [Address] ON Homes.IdAddress = [Address].Id JOIN AspNetUsers ON Homes.IdUser = AspNetUsers.Id;");
        }

        public async Task<IEnumerable<Homes>> Get(string idaddress)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryAsync<Homes>("SELECT * FROM Homes WHERE IdAddress = @IdAddress", new {idaddress} );
        }

        public async Task Create(Homes home)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<string>(@"INSERT INTO Homes(Id,Number,Cars,IdAddress,IdUser,[Status],CreationDate,ModificationDate) VALUES (NEWID(),@Number,0,@IdAddress,'3cd9a931-c383-4867-8ec2-097caf93344a',0,GETDATE(),cast(-53690 as datetime)); SELECT SCOPE_IDENTITY();", home);
            home.Id = id;

        }

        public async Task<Homes> GetById(string id)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryFirstOrDefaultAsync<Homes>(@"SELECT Homes.Id, Homes.Number, Homes.Cars, Homes.IdAddress, Homes.IdUser,[Address].[Name] as [Address], AspNetUsers.UserName FROM Homes JOIN [Address] ON Homes.IdAddress = [Address].Id JOIN AspNetUsers ON Homes.IdUser = AspNetUsers.Id WHERE Homes.Id = @Id;", new { id });
        }

        public async Task Update(AddHomeVM home)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync(@"UPDATE Homes SET Number = @Number, Cars = @Cars, IdAddress = @IdAddress, IdUser = @IdUser, ModificationDate = GETDATE() WHERE Id = @Id", home);

        }

        public async Task Delete(string id)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync(@"DELETE Homes WHERE Id = @Id", new { id });
        }
    }
}
