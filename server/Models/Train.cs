using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    public class Train
    {
        public enum TrainType
        {
            Sectional,
            OpenCoach
        }
        
        [Key]
        public int Id { set; get; }
        
        [Required]
        public string Name { get; set; } 

        [Required]
        public TrainType Type { set; get; }

        [Required]
        public int Seats { set; get; }

        [Required]
        public int Wagons { set; get; }
    }
}