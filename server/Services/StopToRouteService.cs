using System.Collections.Generic;
using System.Linq;
using Server.Database;
using Server.Models;

namespace Server.Services
{
    
    public class StopToRouteService
    {
        private readonly TrainSystemContext _context;

        public StopToRouteService(TrainSystemContext context)
        {
            _context = context;
        }

        public List<int> GetRoutes(int from, int to)
        {
            var fromRoutes = _context.StopsToRoutes.Where(str => str.TrainStopId == from);
            var toRoutes = _context.StopsToRoutes.Where(str => str.TrainStopId == to);
            List<int> routesIds = new List<int>();
            
            foreach (StopToRoute stopToRoute in fromRoutes)
            {
                if (toRoutes.Contains(stopToRoute))
                {
                    routesIds.Add(stopToRoute.RouteId);
                }
            }

            foreach (var routeId in routesIds)
            {
                if (!ValidateRoute(routeId, from, to))
                {
                    routesIds.Remove(routeId);
                }
            }

            return routesIds;
        }

        private bool ValidateRoute(int route, int from, int to)
        {
            var fromNo = _context.StopsToRoutes.First(str => str.RouteId == route && str.TrainStopId == @from).StopNo;
            var toNo = _context.StopsToRoutes.First(str => str.RouteId == route && str.TrainStopId == @to).StopNo;
            return fromNo < toNo;
        }
    }
}