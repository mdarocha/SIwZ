using System.Collections.Generic;
using System.Linq;
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

        public List<Ride> RideSearch() =>
            _context.Rides.Where(ride => true).ToList();

        public List<Ride> GetByRouteId(int routeId) =>
            _context.Rides.Where(ride => ride.RouteId == routeId).ToList();

        public List<Ride> GetByIdsList(List<int> ids) =>
            _context.Rides.Where(ride => ids.Contains(ride.Id)).ToList();
    }
}