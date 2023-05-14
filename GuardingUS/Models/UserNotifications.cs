namespace GuardingUS.Models
{
    public class UserNotifications
    {
        public string IdNotification { get; set; }
        public string IdUser { get; set; }
        public byte Status { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ModificationDate { get; set; }
    }
}
