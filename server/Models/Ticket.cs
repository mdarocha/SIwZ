using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class Ticket
    {
        [Key]
        public int Id { set; get; }
        
        [Required]
        public virtual Ride Ride { set; get; }
        
        [Required]
        public int Price { set; get; }

        [Required]
        public virtual Discount Discount { set; get; }

        [Required]
        public virtual User User { set; get; }

        [Required]
        public virtual TrainStop From { set; get; }

        [Required]
        public virtual TrainStop To { set; get; }

        [Required]
        public virtual Train Train { set; get; }

        [Required]
        public int WagonNr { set; get; }

        [Required]
        public int SeatNr { set; get; }
    }
}