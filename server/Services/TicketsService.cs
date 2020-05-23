using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Server.Database;
using Server.Models;

namespace Server.Services
{
    public class TicketsService
    {
        private readonly TrainSystemContext _context;

        public TicketsService(TrainSystemContext context)
        {
            this._context = context;
        }

        public IEnumerable<Ticket> GetRideTickets(int rideId)
        {
            return _context.Tickets.Where(t => t.RideId == rideId);
        }

        public Ticket CreateTicket(Ticket ticket, string email)
        {
            var usr = _context.Users.Single(u => u.Email == email);
            ticket.UserId = usr.Id;
            _context.Tickets.Add(ticket);
            _context.SaveChanges();

            return ticket;
        }

        public List<Ticket> GetUserTickets(string email)
        {
            var user = _context.Users.Single(u => u.Email == email);
            return _context.Tickets
                .Include(x => x.Discount)
                .Include(x => x.From)
                .Include(x => x.To)
                .Include(x => x.Ride.Train)
                .Where(t => t.UserId == user.Id).ToList();
        }
            

        public Ticket GetTicket(int id) =>
            _context.Tickets.Find(id);

        public Ticket DeleteTicket(int id)
        {
            var ticket = GetTicket(id);
            _context.Tickets.Remove(ticket);
            _context.SaveChanges();

            return ticket;
        }
    }
}