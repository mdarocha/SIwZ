using System.Collections.Generic;

namespace Server.ModelsDTO
{
    public class RoutePatchDTO
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public List<RouteStopDTO> Stops { set; get; }
    }
}