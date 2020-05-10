using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.ModelsDTO;
using Server.Services;

namespace Server.Controllers.Admin
{
    
    [Route("/api/admin/routes")]
    [ApiController]
    public class AdminRouteController : ControllerBase
    {
        private readonly RouteService _RouteService;
        private readonly StopToRouteService _StopToRouteService;
        
        public AdminRouteController(RouteService service, StopToRouteService service2)
        {
            _RouteService = service;
            _StopToRouteService = service2;

        }
        
        [HttpGet]
        [Route("")]
        public ActionResult<List<RoutePatchDTO>> GetAll() 
        {
            var routes = _RouteService.GetAll();
            List<RoutePatchDTO> list = routes.Select(x => new RoutePatchDTO
            {
                Id = x.Id,
                Name = x.Name,
                Stops = _StopToRouteService.GetStops(x.Id).Select(stop => new RouteStopDTO
                {
                    StopId = stop.TrainStopId,
                    StopNo = stop.StopNo,
                    HoursDiff = stop.HoursDiff,
                    MinutesDiff = stop.MinutesDiff
                }).ToList()
            }).ToList();

            return Ok(list); 
        }
        
        [HttpGet("{id}")]
        public ActionResult<RoutePatchDTO> GetById([FromRoute] int id) // To do zmiany
        {
            RoutePatchDTO rs = new RoutePatchDTO();
            var route = _RouteService.GetById(id);
            var stops = _StopToRouteService.GetById(id); //should return all 
            rs.Id = route.Id;
            rs.Name = route.Name;
            rs.Stops = stops;
            
            
            return Ok(rs);
        }
        

        [HttpPost]
        [Route("")]
        public ActionResult<Route> Create([FromBody] RouteDTO route)
        {
            
            var r = _RouteService.Create(route);
            var id = r.Id;
            
            var list = _StopToRouteService.AddStops(route.Stops, id);
            
            return Ok(r);
        }
        
        [HttpPatch]
        [Route("")] 
        public ActionResult<Route> Edit(RoutePatchDTO route)
        {
            var patchRoute = _RouteService.ChangeName(route.Id, route.Name);
            
            _StopToRouteService.DeleteStops(route.Id);
            _StopToRouteService.AddStops(route.Stops, route.Id);

            return Ok(route); 
        }
        
        [HttpDelete("{id}")]
        [Route("")]
        public ActionResult Delete([FromRoute]int id)
        {
            var deleteRoute = _RouteService.GetById(id);
            _RouteService.Delete(deleteRoute);
            
            _StopToRouteService.DeleteStops(id);
            
            return Ok(deleteRoute); // should return all deleted object?
        }
    }
}