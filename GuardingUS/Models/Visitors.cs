using System.ComponentModel.DataAnnotations;

namespace GuardingUS.Models
{
    public class Visitors
    {
        
        public string Id { get; set; }

        public string Name { get; set; }

        public string CarPlate { get; set; }
        [Display(Name = "Address")]
        public string IdAddress { get; set; }

        [Display(Name = "Home Number")]
        public string HomeId { get; set; }
        public DateTime Entrance { get; set; }

        public DateTime Exit { get; set; }

        public string Identification { get; set; }
        

        public string Description { get; set; }


       
    }
}
