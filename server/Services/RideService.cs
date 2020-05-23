using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Server.Database;
using Server.Models;

namespace Server.Services
{
    public class RideService
    {
        private readonly TrainSystemContext _context;

        public RideService(TrainSystemContext context)
        {
            _context = context;
        }

        public List<Ride> GetAll()
        {
            var rides = _context.Rides.Where(x => true).ToList();
            return rides;
        }

        public Ride GetById(int id)
        {
            return _context.Rides.Find(id);
        }

        public Ride Create(Ride ride)
        {
            _context.Rides.Add(ride);
            _context.SaveChanges();
            return ride;
        }


        public Ride GetRide(int id) =>
            _context.Rides.Include(x => x.Train).Include(x => x.Route).Single(r => r.Id == id);

        public List<Ride> RideSearch() =>
            _context.Rides.Where(ride => true).ToList();

        public List<Ride> GetByRouteId(int routeId) =>
            _context.Rides.Where(ride => ride.RouteId == routeId).ToList();

        public List<Ride> GetByIdsList(List<int> ids) =>
            _context.Rides.Include(x => x.Train).Where(ride => ids.Contains(ride.Id)).ToList();
    }
}