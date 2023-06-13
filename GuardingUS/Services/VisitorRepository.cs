using Dapper;
using GuardingUS.Models;
using GuardingUS.Models.ViewModels;
using Microsoft.Data.SqlClient;

namespace GuardingUS.Services
{
    public interface IVisitorRepository
    {
        Task Create(Visitors visitors);
        Task Update(Visitors visitor);
        Task<IEnumerable<VisitorHomeVM>> Get();
    }
    public class VisitorRepository: IVisitorRepository
    {
        private readonly string connectionString;

        public VisitorRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task Create(Visitors visitors)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<string>(@"INSERT INTO Visitors(Id,[Name],CarPlate,IdAddress,Entrance,[Exit],Identification,HomeId,[Description]) VALUES (NEWID(),@Name,@CarPlate,@IdAddress,GETDATE(),cast(-53690 as datetime),@Identification,@HomeId,@Description); SELECT SCOPE_IDENTITY();", visitors);

            visitors.Id = id;

        }

        public async Task<IEnumerable<VisitorHomeVM>> Get()
        {
            using var connection = new SqlConnection(connectionString);

            //return await connection.QueryAsync<Visitors>("SELECT Id,[Name],CarPlate,Entrance,[Exit],Identification,HomeId,[Description] FROM Visitors ORDER BY Entrance");
            return await connection.QueryAsync<VisitorHomeVM>("SELECT v.Id,v.[Name],v.CarPlate, v.Entrance, v.[Exit], v.[Identification], h.Number, h.[IdAddress] from Visitors v join Homes h on v.HomeId = h.Id ORDER BY Entrance");
        }

        public async Task<Visitors> GetById(string id)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryFirstOrDefaultAsync<Visitors>(@"SELECT * FROM Visitors WHERE Id = @Id ", new { id });
        }

        public async Task Update(Visitors visitor)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync(@"UPDATE Visitors SET [Exit] = GETDATE() WHERE Id = @Id",visitor);

        }

        



        

    }
}
