using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class Train
    {
        [Key]
        public int Id { set; get; }

        [Required]
        public string Type { set; get; }

        [Required]
        public int Seats { set; get; }

        [Required]
        public int Wagons { set; get; }
    }
}