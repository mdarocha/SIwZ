using System.Collections.Generic;
using System.Linq;
using Server.Database;
using Server.Models;
using Server.ModelsDTO;

namespace Server.Services
{
    
    public class StopToRouteService
    {
        private readonly TrainSystemContext _context;

        public StopToRouteService(TrainSystemContext context)
        {
            _context = context;
        }

        public List<StopToRoute> AddStops(List<RouteStopDTO> list, int routeId)
        {
            List<StopToRoute> stops = list.Select(x => new StopToRoute
            {
                RouteId = routeId,
                TrainStopId = x.StopId,
                HoursDiff = x.HoursDiff,
                MinutesDiff = x.MinutesDiff,
                StopNo = x.StopNo
            }).ToList();
            
            _context.AddRange(stops);
            _context.SaveChanges();

            return stops; 
        }

        public void DeleteStops(int id)
        {
            var stops = from s in _context.StopsToRoutes
                where s.RouteId == id
                select s;
            
            _context.RemoveRange(stops);
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