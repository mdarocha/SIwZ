using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;

namespace Server.Controllers.Admin
{
    
    [Route("/api/admin/routes")]
    [ApiController]
    public class AdminRouteController : ControllerBase
    {
        private readonly RouteService _routeService;
        
        public AdminRouteController(RouteService service)
        {
            _routeService = service;
        }

        [HttpPost]
        [Route("Create")]
        public ActionResult<Route> Create([FromBody] Route route)
        {
            var r = _routeService.Create(route);
            return Ok(r);
        }
    }
}