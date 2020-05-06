using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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

        public List<Route> GetRoutes(int from, int to)
        {
            var fromRoutes = _context.StopsToRoutes.Include(x => x.Route).Where(str => str.TrainStopId == from).ToList();
            var toRoutes = _context.StopsToRoutes.Include(x => x.Route).Where(str => str.TrainStopId == to).ToList();

            var routesIds = fromRoutes.Intersect(toRoutes, new StopToRouteComparer());

            return routesIds.AsEnumerable().Where(route =>
            {
                var fromNo = fromRoutes.Single(str => str.RouteId == route.RouteId).StopNo;
                var toNo = toRoutes.Single(str => str.RouteId == route.RouteId).StopNo;
                return fromNo < toNo;
            }).Select(route => route.Route).ToList();
        }
    }
}