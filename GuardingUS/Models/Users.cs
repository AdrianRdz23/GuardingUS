namespace GuardingUS.Models
{
    public class Users
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        //public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string NormalizedEmail { get; set; }
        public string PasswordHash { get; set; }
        public string RoleId { get; set; }

        //Temporal
        public string UserId { get; set; }

    }
}
