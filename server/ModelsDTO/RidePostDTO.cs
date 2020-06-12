using System;

namespace Server.ModelsDTO
{
    public class RidePostDTO
    {
        public int RouteId { set; get; }
        public DateTime StartTime { set; get; }
        public int TrainId { set; get; }
        public int Price { set; get; }
        
        public bool IsEveryDayRide { set; get; }
    }
}