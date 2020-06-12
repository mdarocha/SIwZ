using System;

namespace Server.ModelsDTO
{
    public class TicketFormDTO
    {
        public int RideId { set; get; }
        public int DiscountId { set; get; }
        public int FromId { set; get; }
        public int ToId { set; get; }
        public int WagonNo { set; get; }
        public int SeatNo { set; get; }
        
        public DateTime RideDate { set; get; }
    }
}