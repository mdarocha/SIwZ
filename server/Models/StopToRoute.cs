using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Server.Models
{
    public class StopToRoute
    {
        public int RouteId { get; set; }
        public int TrainStopId { get; set; }
        
        public virtual Route Route { get; set; }
        public virtual TrainStop TrainStop { get; set; }
        public int StopNo { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
    
    public class StopToRouteComparer : IEqualityComparer<StopToRoute>
    {
        public bool Equals(StopToRoute x, StopToRoute y)
        {
            return x.RouteId == y.RouteId;
        }

        public int GetHashCode(StopToRoute obj)
        {
            return obj.RouteId;
        }
    }
}