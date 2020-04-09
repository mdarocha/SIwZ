using System.Collections.Generic;
using System.Linq;
using Server.Models;
using server.PostgreSQL;

namespace Server.Services
{
    public class RouteService
    {
        private readonly TrainSystemContext _context;

        public RouteService(TrainSystemContext context)
        {
            _context = context;
        }

        public List<Route> Get() =>
            _context.Routes.Where(route => true).ToList();

        public Route Create(Route route)
        {
            _context.Routes.Add(route);
            _context.SaveChanges();
            return route;
        }
    }
}