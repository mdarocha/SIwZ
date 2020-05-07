using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
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

        public List<RouteStopDTO> GetById(int id) 
        {
            var stops = _context.StopsToRoutes.Where(str => str.RouteId == id).OrderBy(str => str.StopNo);

            List<RouteStopDTO> s = stops.Select(x => new RouteStopDTO
            {
                StopId = x.TrainStopId,
                StopNo = x.StopNo,
                HoursDiff = x.HoursDiff,
                MinutesDiff = x.MinutesDiff
            }).ToList(); 
            
            return s;
        }

        public List<StopToRoute> AddStops(List<RouteStopDTO> list, int routeId) // to nie bd to moze?
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

        public List<StopToRoute> GetStops(int routeId)
        {
            return _context.StopsToRoutes.Where(str => str.RouteId == routeId).OrderBy(str => str.StopNo).ToList();
        }


    }
}