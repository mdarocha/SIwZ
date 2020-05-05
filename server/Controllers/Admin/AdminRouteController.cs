using System.Collections.Generic;
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
        public ActionResult<List<Route>> GetAll() 
        {
            var routes = _RouteService.GetAll();
            return Ok(routes);
        }
        
        [HttpGet("{id}")]
        public ActionResult<List<Train>> GetById([FromRoute] int id) 
        {
            var routes = _RouteService.GetById(id);
            return Ok(routes);
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