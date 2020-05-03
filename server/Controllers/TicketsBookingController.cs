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

        public TicketsBookingController(RouteService routeService, TrainStopService trainStopService,
            RideService rideService)
        {
            _routeService = routeService;
            _trainStopService = trainStopService;
            _rideService = rideService;
        }

        [HttpGet("routes")]
        public ActionResult<List<Route>> GetRoutes() =>
            _routeService.Get();

        [HttpGet("stops")]
        public ActionResult<List<TrainStop>> GetStops() =>
            _trainStopService.Get();

        [HttpGet("rides")]
        public ActionResult<List<Ride>> GetRides() =>
            _rideService.RideSearch();
    }
}
