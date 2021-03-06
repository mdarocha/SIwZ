using System.Collections.Generic;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Server.ModelsDTO
{
    public class RouteDTO
    {
        public string Name { set; get; }
        public List<RouteStopDTO> Stops { set; get; }
    }
}