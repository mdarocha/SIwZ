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

        public Ticket CreateTicket(Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            _context.SaveChanges();

            return ticket;
        }

        public List<Ticket> GetUSerTickets(int userId) =>
            _context.Tickets.Where(t => t.UserId == userId).ToList();

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