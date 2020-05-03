using System.Collections.Generic;
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
        private readonly RouteService _routeService;
        private readonly TrainStopService _trainStopService;
        private readonly RideService _rideService;
        private readonly StopToRouteService _stopToRouteService;

        public TicketsBookingController(RouteService routeService, TrainStopService trainStopService,
            RideService rideService, StopToRouteService stopToRouteService)
        {
            _routeService = routeService;
            _trainStopService = trainStopService;
            _rideService = rideService;
            _stopToRouteService = stopToRouteService;
        }

        [HttpGet("routes")]
        public ActionResult<List<Route>> GetRoutes() =>
            _routeService.GetAll();

        [HttpGet("stops")]
        public ActionResult<List<TrainStop>> GetStops() =>
            _trainStopService.GetAll();

        [HttpGet("rides")]
        public ActionResult<List<Ride>> GetRides([FromQuery] int from, [FromQuery] int to, [FromQuery] int route)
        {
            if (route.Equals(null))
            {
                var routesIds = _stopToRouteService.GetRoutes(from, to);
                return _rideService.GetByIdsList(routesIds);
            }
            else
            {
                return _rideService.GetByRouteId(route);
            }
        }
    }
}