using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Server.Models;
using Server.ModelsDTO;
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
        public ActionResult<List<RideDTO>> GetRides([FromQuery] int from, [FromQuery] int to, [FromQuery] DateTime date)
        {
            var routes = _stopToRouteService.GetRoutes(from, to);
            var rides = _rideService.GetByIdsList(routes.Select(r => r.Id).ToList());
            
            //TODO
            // move date check somewhere else :)

            var rideDTOs = rides.Where(r => r.StartTime > date).Select( r => new RideDTO
            {
                Id = r.Id,
                From = from,
                To = to,
                TrainStops = _stopToRouteService.GetStops(r.RouteId).Select(s => new RideStopDTO
                {
                    StopId = s.TrainStop.Id,
                    City = s.TrainStop.City,
                    Name = s.TrainStop.Name,
                    StopNo = s.StopNo,
                    ArrivalTime = r.StartTime.AddHours(s.HoursDiff).AddMinutes(s.MinutesDiff)
                }).ToList(),
                StartTime = r.StartTime,
                Train = r.Train,
                FreeTickets = r.FreeTickets,
                Price = r.Price
            }).ToList(); 
            
            return Ok(rideDTOs);
        }
    }
}