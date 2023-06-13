using System.ComponentModel.DataAnnotations;

namespace GuardingUS.Models
{
    public class Notifications
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Title { get; set; }

        public string IdUser { get; set; }

        public string Message { get; set; }


        //status of the user
        public byte Status { get; set; }

        //date when the user has been created
        public DateTime CreationDate { get; set; }

        //date if the user has benn modified
        public DateTime ModificationDate { get; set; }
    }
}
