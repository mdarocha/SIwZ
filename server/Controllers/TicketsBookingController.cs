using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            if (date < DateTime.Now)
            {
                return BadRequest("We cannot travel back in time");
            }
            
            var routes = _stopToRouteService.GetRoutes(from, to);
            var rides = _rideService.GetByIdsList(routes.Select(r => r.Id).ToList());

            var rideDTOs = rides.Where(
                r => (r.StartTime.Date == date.Date && r.StartTime.TimeOfDay > date.TimeOfDay) ||
                                            (date.Date > DateTime.Today && r.IsEveryDayRide)
                                            ).Select(r =>
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
                    Price = r.Price
                };
            }).ToList();

            return Ok(rideDTOs);
        }

        [HttpGet("rides/{id}/freeSeats")]
        public ActionResult<List<Wagon>> GetFreeSeats([FromRoute] int rideId, [FromQuery] int from, [FromQuery] int to)
        {
            var ride = _rideService.GetRide(rideId);
            var route = _stopToRouteService.GetStops(ride.RouteId);

            var fromNo = route.Where(x => x.TrainStopId == from).Select(x => x.StopNo).Single();
            var toNo = route.Where(x => x.TrainStopId == to).Select(x => x.StopNo).Single();

            // all tickets for given ride on given route section
            var rideTickets = _ticketsService.GetRideTickets(rideId).Where(t =>
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

        [HttpGet("rides/{id}")]
        public ActionResult<RideDTO> GetRide([FromRoute] int id)
        {
            var ride = _rideService.GetRide(id);
            var stops = _stopToRouteService.GetStops(ride.RouteId);
            var startTime = ride.StartTime;
            return Ok(new RideDTO
            {
                Id = ride.Id,
                From = stops.First().TrainStopId,
                To = stops.Last().TrainStopId,
                TrainStops = stops.Select(s =>
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
                StartTime = ride.StartTime,
                Train = ride.Train,
                Price = ride.Price
            });
        }

        [HttpGet("discounts")]
        public ActionResult<List<Discount>> GetDiscounts()
            => _discountService.GetAll();

        [Authorize]
        [HttpPost("tickets")]
        public ActionResult<Ticket> BookTicket([FromBody] TicketFormDTO form)
        {
            var id = _userManager.GetUserId(User);
            var ride = _rideService.GetRide(form.RideId);
            var price = ride.Price * GetRoutePart(ride.RouteId, form.FromId, form.ToId);

            if (!form.DiscountId.Equals(null))
            {
                price = _discountService.ApplyDiscount(price, form.DiscountId);
            }

            var ticket = new Ticket
            {
                RideId = ride.Id,
                Price = (int) price,
                DiscountId = form.DiscountId,
                FromId = form.FromId,
                ToId = form.ToId,
                WagonNo = form.WagonNo,
                SeatNo = form.SeatNo
            };

            var t = _ticketsService.CreateTicket(ticket, id);

            return Ok();
        }

        private int GetRoutePart(int routeId, int from, int to)
        {
            var stops = _stopToRouteService.GetStops(routeId);
            var fromNo = stops.Single(s => s.TrainStopId == from).StopNo;
            var toNo = stops.Single(s => s.TrainStopId == to).StopNo;
            var le = stops.Count;
            return (le - fromNo - (le - toNo)) / le;
        }

        [Authorize]
        [HttpGet("tickets")]
        public List<TicketDTO> GetUserTickets()
        {
            var id = _userManager.GetUserId(User);
            var tickets = _ticketsService.GetUserTickets(id);
            var dtos = tickets.Select(t => new TicketDTO
            {
                Id = t.Id,
                Price = t.Price,
                Discount = t.Discount.Type,
                From = t.From.City + "-" + t.From.Name,
                To = t.To.City + "-" + t.To.Name,
                TrainName = t.Ride.Train.Name,
                WagonNo = t.WagonNo,
                SeatNo = t.SeatNo
            }).ToList();
            return dtos;
        }

        [Authorize]
        [HttpGet("tickets/{id}")]
        public ActionResult RevokeTicket([FromRoute] int id)
        {
            var ticket = _ticketsService.GetTicket(id);
            var userId = _userManager.GetUserId(User);

            if (ticket.UserId.Equals(userId))
            {
                return Ok(_ticketsService.DeleteTicket(id));
            }

            return Forbid();
        }
    }
}