using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class Train
    {
        public enum TrainTypes
        {
            Sectional,
            OpenCoach
        }
        
        [Key]
        public int Id { set; get; }
        
        [Required]
        public string Name { get; set; } 

        [Required]
        public int Type { set; get; }

        [Required]
        public int Seats { set; get; }

        [Required]
        public int Wagons { set; get; }
    }
}