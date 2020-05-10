using System;
using System.Collections.Generic;
using Server.Models;

namespace Server.ModelsDTO
{
    public class RideDTO
    {
        public int Id { set; get; }
        
        public int From { set; get; }
        
        public int To { set; get; } 
        
        public List<RideStopDTO> TrainStops { set; get; }
        
        public DateTime StartTime { set; get; }
        
        public Train Train { set; get; }

        public int FreeTickets { set; get; }
        
        public int Price { set; get; }
        
    }

    public class RideStopDTO
    {
        public DateTime ArrivalTime { set; get; }
        
        public int StopId { set; get; }
        
        public string City { get; set; }
        
        public string Name { get; set; }
        
        public int StopNo { set; get; }
    }
}