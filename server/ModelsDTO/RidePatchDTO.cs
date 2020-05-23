using System;

namespace Server.ModelsDTO
{
    public class RidePatchDTO
    {
        public int Id { set; get; }
        public int RouteId { set; get; }
        public DateTime StartTime { set; get; }
        public int TrainId { set; get; }
        public double Price { set; get; }
        public bool IsEveryDayRide { set; get; }
    }
}