using System;
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
}