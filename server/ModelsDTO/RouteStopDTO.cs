namespace Server.ModelsDTO
{
    public class RouteStopDTO
    {
        public int StopId { get; set; }
        public int StopNo { get; set; } 
        public int HoursDiff{ get; set; }
        public int MinutesDiff { get; set; }
    }
}