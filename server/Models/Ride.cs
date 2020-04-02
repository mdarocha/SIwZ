using System;
using Microsoft.AspNetCore.Routing;

namespace Server.Models
{
    public class Ride
    {
        public int Id { set; get; }

        public Route RouteId { set; get; }

        public DateTime StartTime { set; get; }

        public Train TrainId { set; get; }

        public int FreeTickets { set; get; }
    }
}