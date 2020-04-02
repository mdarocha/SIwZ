using System;

namespace Server.Models
{
    public class Ride
    {
        public int ID_Ride { set; get; }
        
        public int Route_Id { set; get; }
        
        public DateTime Start_Time { set; get; }
        
        public int Train_ID { set; get; }
        
        public int Free_Tickets { set; get; }
    }
}