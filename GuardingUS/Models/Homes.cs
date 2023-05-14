namespace GuardingUS.Models
{
    public class Homes
    {
        public string Id { get; set; }

        public int Number { get; set; }

        public int Cars { get; set; }

        public string IdAddress { get; set; }

        public string IdUser { get; set; }


        //status of the user
        public byte Status { get; set; }

        //date when the user has been created
        public DateTime CreationDate { get; set; }

        //date if the user has benn modified
        public DateTime ModificationDate { get; set; }
    }
}
