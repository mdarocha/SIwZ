namespace Server.Models
{
    public class Ticket
    {
        public int Id { set; get; }

        public Ride RideId { set; get; }

        public int Price { set; get; }

        public Discount DiscountId { set; get; } //fk to discount

        public User UserId { set; get; }

        public TrainStop From { set; get; } //fk to train stop

        public TrainStop To { set; get; } //fk to train stop

        public Train TrainId { set; get; }

        public int WagonNr { set; get; }

        public int SeatNr { set; get; }
    }
}