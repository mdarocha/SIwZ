using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    public class Ticket
    {
        [Key]
        public int Id { set; get; }
        
        [ForeignKey("RideId")]
        [Required]
        public virtual Ride RideId { set; get; }
        
        [Required]
        public int Price { set; get; }

        [ForeignKey("DiscountId")]
        [Required]
        public virtual Discount DiscountId { set; get; } //fk to discount

        [ForeignKey("UserId")]
        [Required]
        public virtual User UserId { set; get; }

        [ForeignKey("From")]
        [Required]
        public virtual TrainStop From { set; get; } //fk to train stop

        [ForeignKey("To")]
        [Required]
        public virtual TrainStop To { set; get; } //fk to train stop

        [ForeignKey("TrainId")]
        [Required]
        public virtual Train TrainId { set; get; }

        [Required]
        public int WagonNr { set; get; }

        [Required]
        public int SeatNr { set; get; }
    }
}