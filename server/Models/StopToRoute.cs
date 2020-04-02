using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Server.Models
{
    public class StopToRoute
    {
        public int Id { set; get; }
        public Route Route { get; set; }
        public TrainStop TrainStop { get; set; }
        public int StopNo { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}