using System.Collections.Generic;
using Microsoft.AspNetCore.Server.Kestrel.Core;

namespace Server.ModelsDTO
{
    public class RouteDto
    {
        public string Name { set; get; }
        public List<RouteStopDTO> Stops;
    }
}