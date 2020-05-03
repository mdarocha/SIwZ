using System.Collections.Generic;
using System.Linq;
using Server.Models;
using Server.Database;

namespace Server.Services
{
    public class RouteService
    {
        private readonly TrainSystemContext _context;

        public RouteService(TrainSystemContext context)
        {
            _context = context;
        }
        
        public List<Route> GetAll()
        {
            return _context.Routes.Where(d => true).ToList();
        }


        public Route GetById(int id)
        {
            return _context.Routes.Find(id);
        }
        
        public Route Create(Route route)
        {
            _context.Routes.Add(route);
            _context.SaveChanges();
            return route;
        }
        
        public void Edit(Route route)
        {
            var r = _context.Routes.Find(route.Id);

            if (r != null)
            {
                r.Name = route.Name;
                
                _context.Routes.Update(r);
                _context.SaveChanges();
            }
        }
        
        public void Delete(Route route)
        {
            _context.Routes.Remove(route);
            _context.SaveChanges();
        }
        
    }
}