using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Server.Models;
using Server.ModelsDTO;
using Server.Services;


namespace Server.Controllers.Admin
{
    [ApiController]
    [Route("api/admin/rides")]
    public class AdminRideController : ControllerBase
    {
        private readonly RideService _rideService;
        private readonly TrainService _trainService;
        private readonly RouteService _routeService;
        private readonly StopToRouteService _stopToRouteService;


        public AdminRideController(RideService rideService, TrainService trainService, RouteService routeService,
            StopToRouteService stopToRouteService)
        {
            _rideService = rideService;
            _trainService = trainService;
            _routeService = routeService;
            _stopToRouteService = stopToRouteService;
        }

        [HttpGet]
        [Route("")]
        public ActionResult<List<Ride>> GetAll()
        {
            var r = _rideService.GetAll();
            var rides = r.Select(x => new Ride
            {
                Id = x.Id,
                Price = x.Price,
                Route = _routeService.GetById(x.RouteId),
                RouteId = x.RouteId,
                StartTime = x.StartTime,
                Train = _trainService.GetById(x.TrainId),
                TrainId = x.TrainId,
                IsEveryDayRide = x.IsEveryDayRide
            }).ToList();

            return Ok(rides);
        }

        [HttpGet("{id}")]
        public ActionResult<Ride> GetById([FromRoute] int id)
        {
            var ride = _rideService.GetRide(id);
            ride.Train = _trainService.GetById(ride.TrainId);
            ride.Route = _routeService.GetById(ride.RouteId);


            return Ok(ride);
        }

        [HttpPost]
        [Route("")]
        public ActionResult<Train> Add(RidePostDTO ridePost)
        {
            if (ModelState.IsValid)
            {
                Ride ride = new Ride();
                ride.Price = ridePost.Price;
                ride.Route = _routeService.GetById(ridePost.RouteId);
                ride.Train = _trainService.GetById(ridePost.TrainId);
                ride.RouteId = ridePost.RouteId;
                ride.TrainId = ridePost.TrainId;
                ride.IsEveryDayRide = ridePost.IsEveryDayRide;
                ride.StartTime = Convert.ToDateTime(ridePost.StartTime);
                _rideService.Create(ride);
                return Ok(ride);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPatch]
        [Route("")]
        public ActionResult<Train> Edit(RidePatchDTO ridePatch)
        {
            if (ModelState.IsValid)
            {
                var ride = _rideService.GetRide(ridePatch.Id);
                ride.Price = ridePatch.Price;
                ride.Route = _routeService.GetById(ridePatch.RouteId);
                ride.Train = _trainService.GetById(ridePatch.TrainId);
                ride.RouteId = ridePatch.RouteId;
                ride.TrainId = ridePatch.TrainId;
                ride.IsEveryDayRide = ridePatch.IsEveryDayRide;
                ride.StartTime = Convert.ToDateTime(ridePatch.StartTime);
                _rideService.Edit(ride);
                
                return Ok(ride);
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpDelete("{id}")]
        [Route("")]
        public ActionResult Delete([FromRoute] int id)
        {
            var deleteRide = _rideService.GetRide(id);
            _rideService.Delete(id);
            
            return Ok(deleteRide);
        }
    }
}