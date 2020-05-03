using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class RouteController : ControllerBase
    {
        private readonly RouteService _routeService;

        public RouteController(RouteService service)
        {
            _routeService = service;
        }

        [HttpGet]
        [Route("Get")]
        public ActionResult<List<Route>> Get() =>
            _routeService.GetAll();

        
    }
}