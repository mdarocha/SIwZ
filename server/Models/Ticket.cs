namespace Server.Models
{
    public class Ticket
    {
        public int ID_Ticket { set; get; }
        
        public int Ride_ID { set; get; }
        
        public int Price { set; get; }
        
        public int Discount_ID { set; get; } //fk to discount
        
        public int User_ID { set; get; }
        
        public int From { set; get; } //fk to train stop
        
        public int To { set; get; } //fk to train stop
        
        public int Train_ID { set; get; } // fk to Train
        
        public int Wagon_Nr { set; get; }
        
        public  int Seat_Nr { set; get; }
    }
}