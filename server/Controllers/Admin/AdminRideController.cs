using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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
    
    
        public AdminRideController (RideService rideService, TrainService trainService, RouteService routeService, StopToRouteService stopToRouteService)
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
                FreeTickets = x.FreeTickets,
                Price = x.Price,
                Route = _routeService.GetById(x.RouteId),
                RouteId = x.RouteId,
                StartTime = x.StartTime,
                Train = _trainService.GetById(x.TrainId),
                TrainId = x.TrainId
            }).ToList();
            
            return Ok(rides);
        }
        
        [HttpGet("{id}")]
        public ActionResult<Ride> GetById([FromRoute] int id)
        {
            var ride = _rideService.GetById(id);
            ride.Train = _trainService.GetById(ride.TrainId);
            ride.Route = _routeService.GetById(ride.RouteId);

            
            return Ok(ride);
        }
        
        [HttpPost]
        [Route("")]
        public ActionResult<Train> Add(RidePostDTO ridePost)  
        {
            Ride ride = new Ride();
            ride.Price = ridePost.Price;
            ride.Route = _routeService.GetById(ridePost.RouteId);
            ride.Train = _trainService.GetById(ridePost.TrainId);
            ride.FreeTickets = ride.Train.Seats * ride.Train.Wagons;
            ride.RouteId = ridePost.RouteId;
            ride.TrainId = ridePost.TrainId;

           
            var dt = _rideService.ValidateDate(ridePost.StartTime); // Need to extend validation

            if (dt != null)
            {
                ride.StartTime = Convert.ToDateTime(ridePost.StartTime);
            }
            else
            {
                return BadRequest();
            }

            _rideService.Create(ride);
            return Ok(ride);
        }



        [HttpPatch]
        [Route("")] 
        public ActionResult<Train> Edit(Train train)
        {
            return Ok();
        }
        
        [HttpDelete("{id}")]
        [Route("")]
        public ActionResult Delete([FromRoute]int id)
        {
            return Ok();
        }

    
    }
}