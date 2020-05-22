using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class Ticket
    {
        [Key]
        public int Id { set; get; }
        
        [Required]
        public int RideId { get; set; }
        
        [Required]
        public virtual Ride Ride { set; get; }
        
        [Required]
        public double Price { set; get; }
        
        [Required]
        public int DiscountId { set; get; }

        [Required]
        public virtual Discount Discount { set; get; }
        
        [Required]
        public virtual int UserId { set; get; }

        [Required]
        public virtual User User { set; get; }

        [Required]
        public int FromId { set; get; } 
            
        [Required]
        public virtual TrainStop From { set; get; }

        [Required]
        public int ToId { set; get; }
        
        [Required]
        public virtual TrainStop To { set; get; }

        [Required]
        public string TrainName { set; get; }

        [Required]
        public int WagonNo { set; get; }

        [Required]
        public int SeatNo { set; get; }
    }
}