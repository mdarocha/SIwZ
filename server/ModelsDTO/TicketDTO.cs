using System;

namespace Server.ModelsDTO
{
    public class TicketDTO
    {
        public int Id { set; get; }
        
        public int Price { set; get; }
        
        public String Discount { set; get; }
        
        public string From { set; get; } 

        public string To { set; get; }
        
        public string TrainName { set; get; }

        public int WagonNo { set; get; }

        public int SeatNo { set; get; }
    }
}