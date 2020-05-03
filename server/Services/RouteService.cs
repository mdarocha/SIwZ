using System.Collections.Generic;
using System.Linq;
using Server.Models;
using Server.Database;
using Server.ModelsDTO;

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
        
        public Route Create(RouteDto routeDto) 
        {
            Route route = new Route();
            route.Name = routeDto.Name;
            
            _context.Routes.Add(route);
            _context.SaveChanges();

            var routeId = route.Id;

            foreach (var item in routeDto.Stops)
            {
                
            }
            
            
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