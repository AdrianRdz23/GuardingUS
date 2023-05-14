using Dapper;
using GuardingUS.Models;
using GuardingUS.Models.ViewModels;
using Microsoft.Data.SqlClient;

namespace GuardingUS.Services
{
    public interface INotificationRepository
    {
        Task Create(Notifications notification);
        Task<IEnumerable<NotificationIndexVM>> Get(string iduser);
        Task<IEnumerable<NotificationIndexVM>> Get(string idnotification, string iduser);
        Task Send(UserNotifications usernotification);
    }
    public class NotificationRepository : INotificationRepository
    {
        private readonly string connectionString;

        public NotificationRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<NotificationIndexVM>> Get(string iduser)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryAsync<NotificationIndexVM>("SELECT un.IdNotification, n.Title, u.UserName, n.CreationDate from UserNotifications un join Notifications n on  un.IdNotification = n.Id join AspNetUsers u on n.IdUser = u.Id WHERE un.IdUser=@IdUser;", new { iduser });
        }

        public async Task<IEnumerable<NotificationIndexVM>> Get(string idnotification, string iduser)
        {
            using var connection = new SqlConnection(connectionString);

            return await connection.QueryAsync<NotificationIndexVM>("SELECT un.IdNotification, n.Title, u.UserName, n.[Message],n.CreationDate from UserNotifications un join Notifications n on  un.IdNotification = n.Id join AspNetUsers u on n.IdUser = u.Id WHERE un.IdUser=@IdUser AND un.IdNotification = @IdNotification", new { iduser,idnotification });
        }

        

        public async Task Create(Notifications notification)
        {
            using var connection = new SqlConnection(connectionString);
            var id = await connection.QuerySingleAsync<string>(@"INSERT Notifications (Id, Title, [Message],IdUser,[Status], CreationDate, ModificationDate) VALUES (NEWID(), @Title,@Message,@IdUser,0,GETDATE(),cast(-53690 as datetime)); SELECT SCOPE_IDENTITY();", notification);
            notification.Id = id;

        }

        public async Task Send(UserNotifications usernotification)
        {
            using var connection = new SqlConnection(connectionString);

            await connection.ExecuteAsync(@"INSERT INTO UserNotifications(IdNotification,IdUser,[status], CreationDate, ModificationDate) VALUES (@IdNotification, @IdUser, 0,GETDATE(),cast(-53690 as datetime));", usernotification);

        }





    }
}
