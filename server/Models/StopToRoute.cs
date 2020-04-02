using System;
using Microsoft.EntityFrameworkCore;

namespace Server.Models
{
    public class StopToRoute
    {
        public Route RouteId { get; set; }
        public TrainStop StopId { get; set; }
        public int StopNo { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}