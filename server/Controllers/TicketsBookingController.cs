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
            if (date < DateTime.Today)
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
                DateTime startTime;
                if (r.IsEveryDayRide && date.Date > DateTime.Today)
                    startTime = ToRideDate(date, r.StartTime);
                else
                    startTime = r.StartTime;
                
                return new RideDTO
                {
                    Id = r.Id,
                    From = from,
                    To = to,
                    StartTime = startTime,
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
                    Train = r.Train,
                    Price = r.Price
                };
            }).ToList();

            return Ok(rideDTOs);
        }

        [HttpGet("rides/{id}/freeSeats")]
        public ActionResult<List<Wagon>> GetFreeSeats([FromRoute] int id, [FromQuery] int from, [FromQuery] int to, [FromQuery] DateTime date)
        {
            var ride = _rideService.GetRide(id);
            var route = _stopToRouteService.GetStops(ride.RouteId);

            var fromNo = route.Where(x => x.TrainStopId == from).Select(x => x.StopNo).Single();
            var toNo = route.Where(x => x.TrainStopId == to).Select(x => x.StopNo).Single();

            // all tickets for given ride on given route section
            var rideTickets = _ticketsService.GetRideTickets(id)
                .Where( t => t.RideDate.Date == date.Date).Where(t =>
            {
                var ticketFromNo = route.Where(x => x.TrainStopId == t.FromId).Select(x => x.StopNo).Single();
                var ticketToNo = route.Where(x => x.TrainStopId == t.ToId).Select(x => x.StopNo).Single();
                return !(fromNo > ticketToNo || ticketFromNo > toNo);
            });

            // all seats for given ride
            var seats = Enumerable.Range(1, ride.Train.Wagons).Select(i =>
                new Wagon
                {
                    wagonNo = i,
                    seats = Enumerable.Range(1, ride.Train.Seats).Select(j =>
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
        public ActionResult<RideDTO> GetRide([FromRoute] int id, [FromQuery] int from, [FromQuery] int to)
        {
            var ride = _rideService.GetRide(id);
            var stops = _stopToRouteService.GetStops(ride.RouteId);
            var startTime = ride.StartTime;
            return Ok(new RideDTO
            {
                Id = ride.Id,
                From = from,
                To = to,
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

            var firstStop = _stopToRouteService.GetStops(ride.RouteId).Single(str => str.TrainStopId == form.FromId);

            var rideDate = ToRideDate(form.RideDate, ride.StartTime).AddHours(firstStop.HoursDiff).AddMinutes(firstStop.MinutesDiff); 

            var ticket = new Ticket
            {
                RideId = ride.Id,
                Price = (int) price,
                DiscountId = form.DiscountId,
                FromId = form.FromId,
                ToId = form.ToId,
                WagonNo = form.WagonNo,
                SeatNo = form.SeatNo,
                RideDate = rideDate
            };

            var t = _ticketsService.CreateTicket(ticket, id);

            return Ok();
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
                From = t.From,
                To = t.To,
                TrainName = t.Ride.Train.Name,
                WagonNo = t.WagonNo,
                SeatNo = t.SeatNo,
                RideDate = t.RideDate
            }).ToList();
            return dtos;
        }

        [Authorize]
        [HttpDelete("tickets/{id}")]
        public ActionResult RevokeTicket([FromRoute] int id)
        {
            var ticket = _ticketsService.GetTicket(id);
            var userEmail = _userManager.GetUserId(User);

            if (_ticketsService.RevokeTicket(ticket, userEmail))
            {
                return Ok();
            }

            return Forbid();
        }
        
        private int GetRoutePart(int routeId, int from, int to)
        {
            var stops = _stopToRouteService.GetStops(routeId);
            var fromNo = stops.Single(s => s.TrainStopId == from).StopNo;
            var toNo = stops.Single(s => s.TrainStopId == to).StopNo;
            var le = stops.Count;
            return (le - fromNo - (le - toNo)) / le;
        }
        
        private DateTime ToRideDate(DateTime formTime, DateTime rideTime)
        {
            return new DateTime(
                formTime.Year,
                formTime.Month,
                formTime.Day,
                rideTime.Hour,
                rideTime.Minute,
                0
            );
        }
    }
}