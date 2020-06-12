using System;
using Server.Models;

namespace Server.ModelsDTO
{
    public class TicketDTO
    {
        public int Id { set; get; }

        public int Price { set; get; }

        public String Discount { set; get; }

        public TrainStop From { set; get; }

        public TrainStop To { set; get; }

        public string TrainName { set; get; }

        public int WagonNo { set; get; }

        public int SeatNo { set; get; }

        public DateTime RideDate { set; get; }
    }
}