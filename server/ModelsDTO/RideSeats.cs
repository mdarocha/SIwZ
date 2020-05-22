using System;
using System.Collections.Generic;

namespace Server.ModelsDTO
{
    public enum WagonType
    {
        Sectional,
        OpenCoach
    }
    
    public class Wagon
    {
        public int wagonNo { set; get; }
        public WagonType type { set; get; }
        public List<Boolean> seats { set; get; }
    }
}
