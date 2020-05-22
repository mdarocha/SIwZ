using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
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
        private readonly TicketsService _ticketsService;
        private readonly DiscountService _discountService;
        private readonly UserManager<User> _userManager;

        public TicketsBookingController(
            TrainStopService trainStopService,
            RideService rideService,
            StopToRouteService stopToRouteService,
            TicketsService ticketsService,
            DiscountService discountService,
            UserManager<User> userManager
        )
        {
            _trainStopService = trainStopService;
            _rideService = rideService;
            _stopToRouteService = stopToRouteService;
            _ticketsService = ticketsService;
            _discountService = discountService;
            _userManager = userManager;
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
            
            var rideDTOs = rides.Where(r => r.StartTime > date).Select(r =>
            {
                var startTime = r.StartTime;
                return new RideDTO
                {
                    Id = r.Id,
                    From = @from,
                    To = to,
                    TrainStops = _stopToRouteService.GetStops(r.RouteId).Select(s =>
                    {
                        startTime = startTime.AddHours(s.HoursDiff).AddMinutes(s.MinutesDiff);
                        return new RideStopDTO
                        {
                            StopId = s.TrainStop.Id,
                            City = s.TrainStop.City,
                            Name = s.TrainStop.Name,
                            StopNo = s.StopNo,
                            ArrivalTime = startTime
                        };
                    }).ToList(),
                    StartTime = r.StartTime,
                    Train = r.Train,
                    FreeTickets = r.FreeTickets,
                    Price = r.Price
                };
            }).ToList();

            return Ok(rideDTOs);
        }

        [HttpGet("rides/{id}/freeSeats")]
        public ActionResult<List<Wagon>> GetFreeSeats([FromRoute] int id, [FromQuery] int from, [FromQuery] int to)
        {
            var ride = _rideService.GetRide(id);
            var route = _stopToRouteService.GetStops(ride.RouteId);

            var fromNo = route.Where(x => x.TrainStopId == from).Select(x => x.StopNo).Single();
            var toNo = route.Where(x => x.TrainStopId == to).Select(x => x.StopNo).Single();

            // all tickets for given ride on given route section
            var rideTickets = _ticketsService.GetRideTickets(id).Where(t =>
            {
                var ticketFromNo = route.Where(x => x.TrainStopId == t.FromId).Select(x => x.StopNo).Single();
                var ticketToNo = route.Where(x => x.TrainStopId == t.ToId).Select(x => x.StopNo).Single();
                return !(fromNo > ticketToNo || ticketFromNo > toNo);
            });

            // all seats for given ride
            var seats = Enumerable.Range(1, ride.Train.Wagons + 1).Select(i =>
                new Wagon
                {
                    wagonNo = i,
                    seats = Enumerable.Range(1, ride.Train.Seats + 1).Select(j =>
                    {
                        if (!rideTickets.Any())
                            return true;
                        return rideTickets.SingleOrDefault(t => t.SeatNo == j && t.WagonNo == i) == null;
                    }).ToList(),
                    type = (WagonType) ride.Train.Type
                }
            ).ToList();

            return seats;
        }

        [HttpGet("discounts")]
        public ActionResult<List<Discount>> GetDiscounts()
            => _discountService.GetAll();

        [Authorize]
        [HttpPost("tickets")]
        public ActionResult<Ticket> BookTicket(TicketFormDTO form)
        {
            var id = _userManager.GetUserId(User);
            var ride = _rideService.GetRide(form.RideId);
            var price = ride.Price * GetRoutePart(ride.RouteId, form.FromId, form.ToId);

            if (!form.DiscountId.Equals(null))
            {
                price = _discountService.ApplyDiscount(price, form.DiscountId);
                price = price < 0 ? 0 : price;
            }

            var ticket = new Ticket
            {
                RideId = ride.Id,
                Price = price,
                DiscountId = form.DiscountId,
                UserId = int.Parse(id),
                FromId = form.FromId,
                ToId = form.ToId,
                TrainName = ride.Train.Name,
                WagonNo = form.WagonNo,
                SeatNo = form.SeatNo
            };

            return _ticketsService.CreateTicket(ticket);
        }

        private double GetRoutePart(int routeId, int from, int to)
        {
            var stops = _stopToRouteService.GetStops(routeId);
            var fromNo = stops.Single(s => s.TrainStopId == from).StopNo;
            var toNo = stops.Single(s => s.TrainStopId == to).StopNo;
            var le = stops.Count;
            return (double) (le - fromNo - (le - toNo)) / le;
        }
        
        [Authorize]
        [HttpGet("tickets")]
        public List<Ticket> GetUserTickets()
        {
            var id = _userManager.GetUserId(User);
            return _ticketsService.GetUSerTickets(int.Parse(id));
        }

        [Authorize]
        [HttpGet("tickets/{id}")]
        public ActionResult RevokeTicket([FromRoute] int id)
        {
            var ticket = _ticketsService.GetTicket(id);
            var userId = _userManager.GetUserId(User);
         
            if (ticket.UserId.Equals(int.Parse(userId)))
            {
                return Ok(_ticketsService.DeleteTicket(id));
            }

            return Forbid();
        }
    }
}