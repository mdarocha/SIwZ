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

        [ForeignKey("RouteId")]
        public virtual Route RouteId { set; get; }

        [Required]
        public DateTime StartTime { set; get; }

        [ForeignKey("TrainId")]
        public Train TrainId { set; get; }

        [Required]
        public int FreeTickets { set; get; }
    }
}