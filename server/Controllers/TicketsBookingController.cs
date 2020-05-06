using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Server.Models;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("/api")]
    public class TicketsBookingController : ControllerBase
    {
        private readonly TrainStopService _trainStopService;
        private readonly RideService _rideService;
        private readonly StopToRouteService _stopToRouteService;

        public TicketsBookingController(RouteService routeService, TrainStopService trainStopService,
            RideService rideService, StopToRouteService stopToRouteService)
        {
            _trainStopService = trainStopService;
            _rideService = rideService;
            _stopToRouteService = stopToRouteService;
        }

        [HttpGet("stops")]
        public ActionResult<List<TrainStop>> GetStops() =>
            _trainStopService.GetAll();

        [HttpGet("rides")]
        public ActionResult<List<Ride>> GetRides([FromQuery] int from, [FromQuery] int to)
        {
            var routes = _stopToRouteService.GetRoutes(from, to);
            return Ok(_rideService.GetByIdsList(routes.Select(r => r.Id).ToList()));
        }
    }
}