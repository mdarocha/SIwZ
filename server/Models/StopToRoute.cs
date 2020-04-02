using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Server.Models
{
    public class StopToRoute
    {
        [ForeignKey("RouteId")]
        public virtual Route RouteId { get; set; }
        [ForeignKey("StopId")]
        public virtual TrainStop StopId { get; set; }
        public int StopNo { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}