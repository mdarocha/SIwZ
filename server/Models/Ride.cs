using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Routing;

namespace Server.Models
{
    public class Ride
    {
        [Key]
        public int Id { set; get; }
        
        public int RouteId { get; set; }

        public virtual Route Route { set; get; }

        [Required]
        public DateTime StartTime { set; get; }

        public int TrainId { get; set; }
        
        public virtual Train Train { set; get; }
        
        public bool IsEveryDayRide { set; get; }
        
        [Required]
        public double Price { set; get; }
    }
}