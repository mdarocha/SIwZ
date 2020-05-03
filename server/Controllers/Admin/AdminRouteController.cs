using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;

namespace Server.Controllers.Admin
{
    
    [Route("/api/admin/routes")]
    [ApiController]
    public class AdminRouteController : ControllerBase
    {
        private readonly RouteService _service;
        
        public AdminRouteController(RouteService service)
        {
            _service = service;
        }
        
        [HttpGet]
        [Route("")]
        public ActionResult<List<Route>> GetAll() 
        {
            var routes = _service.GetAll();
            return Ok(routes);
        }
        
        [HttpGet("{id}")]
        public ActionResult<List<Train>> GetById([FromRoute] int id) 
        {
            var routes = _service.GetById(id);
            return Ok(routes);
        }

        [HttpPost]
        [Route("")]
        public ActionResult<Route> Create([FromBody] Route route)
        {
            var r = _service.Create(route);
            return Ok(r);
        }
        
        [HttpPatch]
        [Route("")] 
        public ActionResult<Route> Edit(Route route)
        {
            _service.Edit((route));
            return Ok(); 
        }
        
        [HttpDelete("{id}")]
        [Route("")]
        public ActionResult Delete([FromRoute]int id)
        {
            var deleteRoute = _service.GetById(id);
            _service.Delete(deleteRoute);
            
            return Ok();
        }
    }
}